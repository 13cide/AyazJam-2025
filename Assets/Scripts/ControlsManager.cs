using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;


public enum ControlMode
{
    Selector,
    TowerPlacer,

}

public class ControlsManager : MonoBehaviour
{
    [Header("References")]
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap pathTilemap;
    public GameObject towerPrefab;
    public EconomyManager economyManager;

    private ControlMode currentMode = ControlMode.Selector;
    private Dictionary<Vector3Int, GameObject> occupiedTiles = new();
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] GameObject runeAquiredUI;
    [SerializeField] GameObject runeBuyUI;
    [SerializeField] Button BuyRuneButton;
    [SerializeField] KeyCode placingKey = KeyCode.P;
    GameObject newTower;
    Tower selectedTower;

    public void ChangeTowers(TowerType newTowerType) {
        towerPrefab = newTowerType == TowerType.ButterflyTower ? gameplayManager.ButterflyTowerPrefab : gameplayManager.sealTowerPrefab;

        var entries = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<Vector3Int, GameObject>>(occupiedTiles);
        foreach (var kv in entries)
        {
            if (kv.Value.GetComponent<Tower>().isTimeLocked) continue;
            Vector3 towerPos = kv.Value.transform.position;
            Destroy(kv.Value);
            GameObject towerObj = Instantiate(towerPrefab, towerPos, Quaternion.identity);
            occupiedTiles[kv.Key] = towerObj;
            towerObj.GetComponent<Tower>().Placed();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(placingKey))
        {
            if (currentMode == ControlMode.TowerPlacer && newTower != null)
            {
                Destroy(newTower);
                newTower = null;
            }
            currentMode = currentMode == ControlMode.Selector ? ControlMode.TowerPlacer : ControlMode.Selector;
        }
        switch (currentMode)
        {
            case ControlMode.Selector:
                SelectorModeUpdate();
                break;
            case ControlMode.TowerPlacer:
                TowerPlacingModeUpdate();
                break;
        }
    }

    void SelectorModeUpdate()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        if (occupiedTiles.TryGetValue(cellPosition, out GameObject towerObj))
        {
            if (selectedTower != null)
            {
                selectedTower.Select(false);
                runeBuyUI.SetActive(false);
                runeAquiredUI.SetActive(false);
            }
            selectedTower = towerObj.GetComponent<Tower>();
            selectedTower.Select(true);

            runeBuyUI.SetActive(!selectedTower.isTimeLocked);
            runeAquiredUI.SetActive(selectedTower.isTimeLocked);
            if (!selectedTower.isTimeLocked)
            {
                BuyRuneButton.interactable = economyManager.CanAfford(20);
            }
        }
        else
        {
            if (selectedTower != null)
            {
                selectedTower.Select(false);
                selectedTower = null;
                runeBuyUI.SetActive(false);
                runeAquiredUI.SetActive(false);
            }
        }
    }

    void TowerPlacingModeUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        Vector3 centerPosition = grid.GetCellCenterWorld(cellPosition);

        if (newTower == null)
        {
            newTower = Instantiate(towerPrefab, centerPosition, Quaternion.identity);
        } else
        {
            newTower.transform.position = centerPosition;
            newTower.SetActive(IsValidPosition(cellPosition));
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (IsValidPosition(cellPosition) && economyManager.CanAfford(newTower.GetComponent<Tower>().price))
            {
                economyManager.TakeMoney(-newTower.GetComponent<Tower>().price);
                PlaceTower(cellPosition);
            }
            else
            {
                Destroy(newTower);
            }
            newTower = null;
            currentMode = ControlMode.Selector;
        }
    }


    private bool IsValidPosition(Vector3Int cellPos)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        if (!groundTilemap.HasTile(cellPos)) return false;
        if (occupiedTiles.ContainsKey(cellPos)) return false;
        if (pathTilemap.HasTile(cellPos)) return false;
        
        return true; 
    }

    public void BuyRune()
    {
        if (selectedTower == null) return;
        if (selectedTower.isTimeLocked) return;
        if (!economyManager.CanAfford(10)) return;

        economyManager.TakeMoney(-10);
        selectedTower.TimeLock();
        runeBuyUI.SetActive(false);
        runeAquiredUI.SetActive(true);
    }

    private void PlaceTower(Vector3Int cellPos)
    {
        newTower.GetComponent<Tower>().Placed();
        occupiedTiles.Add(cellPos, newTower);

    }
}