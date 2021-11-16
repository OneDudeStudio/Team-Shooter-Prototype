using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //SpawnSettings
    [SerializeField] GameObject teamMate;
    [SerializeField] Vector3[] blueSpawnPoints;
    [SerializeField] Vector3[] redSpawnPoints;

    //UI Settings
    public GameObject StartGamePanel;
    public GameObject GamePanel;
    public Button restartButton;
    public int blueTeamCounter;
    public int redTeamCounter;
    public TextMeshProUGUI redTextCounter;
    public TextMeshProUGUI blueTextCounter;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI timeText;

    //Timer
    float timer = 0f;

    //Global Settings
    public bool StartGame = false;
    public bool EndGame = false;
    // Start is called before the first frame update
    void Awake()
    {
        GenerateTeam();
        StartGamePanel.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
        GamePanel.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void GameStart()
    {
        StartGame = true;
        Time.timeScale = 1f;
        GamePanel.gameObject.SetActive(true);
        StartGamePanel.gameObject.SetActive(false);
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }
  
    void GenerateTeam()
    {
        for (int i = 0; i < blueSpawnPoints.Length; i++)
        {
            Instantiate(teamMate, blueSpawnPoints[i], Quaternion.identity);
            blueTeamCounter++;
        }
        for (int i = 0; i < redSpawnPoints.Length; i++)
        {
            Instantiate(teamMate, redSpawnPoints[i], Quaternion.identity);
            redTeamCounter++;
        }
    }
    void Update()
    {
        if (StartGame == true)
        {
            TeamWinner();
            Timer();
        }
    }

    void TeamWinner()
    {
        if (redTeamCounter == 0 && blueTeamCounter == 0)
        {
            winText.gameObject.SetActive(true);
            winText.text = "NOBODY WIN";
            EndGame = true;
            restartButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
            //print("NOBODY WINS");
        }
        if (blueTeamCounter == 0 && redTeamCounter != 0)
        {
            winText.gameObject.SetActive(true);
            winText.text = "RED TEAM WIN";
            EndGame = true;
            restartButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
            //print("RED WINS");
        }
        else if (redTeamCounter == 0 && blueTeamCounter != 0)
        {
            winText.gameObject.SetActive(true);
            winText.text = "BLUE TEAM WIN";
            EndGame = true;
            restartButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
            //print("BLUE WINS");
        }
    }

    void Timer()
    {
        if (EndGame != true)
        {
            timer += Time.deltaTime;
            timeText.text =  Mathf.RoundToInt(timer).ToString() + " sec";
        }
        else if (EndGame == true)
        {
            timeText.text = "Round was finished by :" + Mathf.RoundToInt(timer).ToString() + " second!";
        }
        redTextCounter.text = "X" + redTeamCounter.ToString();
        blueTextCounter.text = "X" + blueTeamCounter.ToString();
    }
   
}
