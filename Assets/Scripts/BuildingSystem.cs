using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingSystem : MonoBehaviour
{
    [Header("References")]
    public Grid grid;
    public Tilemap groundTilemap;
    public GameObject towerPrefab;
    
    [Header("Visuals")]
    public GameObject previewGhost;

    private void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);

        Vector3 centerPosition = grid.GetCellCenterWorld(cellPosition);

        // --- PREVIEW LOGIC ---
        if (previewGhost != null)
        {
            previewGhost.transform.position = centerPosition;
        }

        // --- PLACEMENT LOGIC ---
        // Check if user clicked Left Mouse Button
        if (Input.GetMouseButtonDown(0)) 
        {
            if (IsValidPosition(cellPosition))
            {
                PlaceTower(centerPosition, cellPosition);
            }
        }
    }

    private bool IsValidPosition(Vector3Int cellPos)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        if (!groundTilemap.HasTile(cellPos)) return false;

        // Check C: Is there already a tower here? 
        // (See "Occupancy Logic" section below for implementation)
        
        return true; 
    }

    private void PlaceTower(Vector3 position, Vector3Int cellPos)
    {
        Instantiate(towerPrefab, position, Quaternion.identity);
        // Mark this cell as occupied (See below)
    }
}