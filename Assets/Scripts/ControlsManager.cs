using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;


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

    private ControlMode currentMode = ControlMode.TowerPlacer;
    private Dictionary<Vector3Int, GameObject> occupiedTiles = new();
    [SerializeField] GameplayManager gameplayManager;
    GameObject newTower;
    Tower selectedTower;

    public void ChangeTowers(TowerType newTowerType) {
        towerPrefab = newTowerType == TowerType.ButterflyTower ? gameplayManager.ButterflyTowerPrefab : gameplayManager.sealTowerPrefab;

        var entries = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<Vector3Int, GameObject>>(occupiedTiles);
        foreach (var kv in entries)
        {
            Vector3 towerPos = kv.Value.transform.position;
            Destroy(kv.Value);
            GameObject towerObj = Instantiate(towerPrefab, towerPos, Quaternion.identity);
            occupiedTiles[kv.Key] = towerObj;
            towerObj.GetComponent<Tower>().Placed();
        }
    }

    public void SetMode(bool toggleValue)
    {
        currentMode = toggleValue ? ControlMode.TowerPlacer : ControlMode.Selector;
    }

    void Update()
    {
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

            if (occupiedTiles.TryGetValue(cellPosition, out GameObject towerObj))
            {
                if (selectedTower != null)
                {
                    selectedTower.Select(false);
                }
                selectedTower = towerObj.GetComponent<Tower>();
                selectedTower.Select(true);
            }
            else
            {
                if (selectedTower != null)
                {
                    selectedTower.Select(false);
                    selectedTower = null;
                }
            }
        }
    }

    void TowerPlacingModeUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        Vector3 centerPosition = grid.GetCellCenterWorld(cellPosition);

        if (Input.GetMouseButton(0)) 
        {
            if (newTower == null)
            {
                newTower = Instantiate(towerPrefab, centerPosition, Quaternion.identity);
            } else
            {
                newTower.transform.position = centerPosition;
                newTower.SetActive(IsValidPosition(cellPosition));
            }
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

    private void PlaceTower(Vector3Int cellPos)
    {
        newTower.GetComponent<Tower>().Placed();
        occupiedTiles.Add(cellPos, newTower);

    }
}