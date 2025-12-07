using UnityEngine;

public class PresentationHelper : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] EconomyManager economyManager;
    [SerializeField] GameObject CVUI;
    [SerializeField] GameObject oneMoreThingSlideUI;
    KeyCode speedUpKey = KeyCode.U;
    KeyCode slowDownKey = KeyCode.D;
    KeyCode normalSpeedKey = KeyCode.N;
    KeyCode nextWaveKey = KeyCode.W;
    KeyCode setMoneyKey = KeyCode.M;
    KeyCode oneMoreThingSlide = KeyCode.O;
    KeyCode showCV = KeyCode.C;
    
    void Update()
    {
        if (Input.GetKeyDown(speedUpKey))
        {
            Time.timeScale *= 2f;
        }
        if (Input.GetKeyDown(slowDownKey))
        {
            Time.timeScale *= 0.5f;
        }
        if (Input.GetKeyDown(normalSpeedKey))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(nextWaveKey))
        {
            enemyManager.EndWave();
        }
        if (Input.GetKeyDown(oneMoreThingSlide))
        {
            oneMoreThingSlideUI.SetActive(oneMoreThingSlideUI.activeSelf ? false : true);
        }
        if (Input.GetKeyDown(showCV))
        {
            CVUI.SetActive(CVUI.activeSelf ? false : true);
        }
        if (Input.GetKeyDown(setMoneyKey))
        {
            economyManager.TakeMoney(100);
        }
    }
}
