using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class BuildingSystem : MonoBehaviour
{
    [Header("References")]
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap pathTilemap;
    public GameObject towerPrefab;


    private Dictionary<Vector3Int, GameObject> occupiedTiles = new();
    GameObject newTower;

    private void Update()
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
        newTower.GetComponent<Tower>().Placed();
        occupiedTiles.Add(cellPos, newTower);

    }
}