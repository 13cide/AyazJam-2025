using System;
using System.Collections;
using Microsoft.Win32.SafeHandles;
using TMPro;
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
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject Anim;
    [SerializeField] GameObject hintPanel;
    [SerializeField] TMP_Text hintText;
    [SerializeField] String[] hints;

    [SerializeField] protected AudioClip ound;
    [SerializeField] protected AudioSource audioSource;
    
    

    public void WaveFinished()
    {
        Anim.SetActive(true);
        choiceUI.SetActive(true);   
        StartCoroutine(PlayAnim());
        audioSource.PlayOneShot(ound);
    }

    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(3.5f);
        Anim.SetActive(false);
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
            winUI.SetActive(true);
            return;
        }
        WaveScriptableObject wave = waves[currentWave];
        StartCoroutine(enemyManager.SpawnWaveRoutine(wave));
        currentWave++;
        if (currentWave - 1 < hints.Length)
        {
            if (hints[currentWave - 1] == "")
            {
                hintPanel.SetActive(false);
                return;
            }
            hintPanel.SetActive(true);
            hintText.text = hints[currentWave - 1];
        }
    }

}
