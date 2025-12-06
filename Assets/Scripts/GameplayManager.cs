using Microsoft.Win32.SafeHandles;
using UnityEngine;

public enum TowerType
{
    SealTower,
    ButterflyTower,
    None
}

public class GameplayManager : MonoBehaviour
{
    int currentWave = 0;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] GameObject choiceUI;
    public GameObject sealTowerPrefab;
    public GameObject ButterflyTowerPrefab;
    [SerializeField] ControlsManager controlsManager;
    [HideInInspector] public TowerType currentTowerType = TowerType.None;
    
    public void Choosing(bool isSeal)
    {
        TowerType towerType = isSeal ? TowerType.SealTower : TowerType.ButterflyTower;
        choiceUI.SetActive(false);
        if (towerType == currentTowerType) return;
        currentTowerType = towerType;
        controlsManager.ChangeTowers(towerType);
    } 

}
