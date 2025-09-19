using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver;
    public bool isCoopMode = false;
    //private bool _isPaused = false;
    private Animator _pauseAnimator;
    [SerializeField]
    private GameObject _pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //_isPaused = true;
            _pauseMenu.SetActive(true);
            _pauseAnimator.SetBool("is_paused",true);
            Time.timeScale = 0;
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

    public void ResumeGame()
    {
        //_isPaused = false;
        _pauseMenu.SetActive(false); 
        Time.timeScale = 1;
    }

    public void ReturnMainMenu()
    {
        isGameOver = true;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
