using System;
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
    TowerType currentTowerType = TowerType.None;
    [SerializeField] WaveScriptableObject[] waves;

    public void WaveFinished()
    {
        choiceUI.SetActive(true);
    }

    public void Choosing(bool isSeal)
    {
        TowerType towerType = isSeal ? TowerType.SealTower : TowerType.ButterflyTower;
        choiceUI.SetActive(false);
        if (towerType == currentTowerType) return;
        currentTowerType = towerType;
        controlsManager.ChangeTowers(towerType);
    } 

    public void StartNextWave()
    {
        if (currentWave >= waves.Length)
        {
            Debug.Log("All waves completed!");
            return;
        }
        WaveScriptableObject wave = waves[currentWave];
        StartCoroutine(enemyManager.SpawnWaveRoutine(wave));
        currentWave++;
    }

}
