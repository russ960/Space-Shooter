using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver;
    public bool isCoopMode = false;
    private bool _isPaused = false;
    [SerializeField]
    private GameObject _pauseMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _isPaused = true;
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            SceneManager.LoadScene(0);
        } 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            Application.Quit();
        }         
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
