using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject defeatScreen;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            victoryScreen.SetActive(false);
            defeatScreen.SetActive(false);
        }
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        EventManager.StartListening("ShowVictoryScreen", ShowVictoryScreen);
        EventManager.StartListening("ShowDeathScreen", ShowDeathScreen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        EventManager.StopListening("ShowVictoryScreen", ShowVictoryScreen);
        EventManager.StopListening("ShowDeathScreen", ShowDeathScreen);
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0;

    }

    public void ShowDeathScreen()
    {
        defeatScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
