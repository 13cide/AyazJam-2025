using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
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

    private ControlMode currentMode = ControlMode.TowerPlacer;
    private Dictionary<Vector3Int, GameObject> occupiedTiles = new();
    GameObject newTower;
    ButterflyTower selectedTower;

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
                selectedTower = towerObj.GetComponent<ButterflyTower>();
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
            if (IsValidPosition(cellPosition))
            {
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
        newTower.GetComponent<ButterflyTower>().Placed();
        occupiedTiles.Add(cellPos, newTower);

    }
}