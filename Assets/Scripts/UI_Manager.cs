using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // Text handle
    [SerializeField]
    private Text _scoreText;
    private Player _player;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    private int _score;
   [SerializeField]
   private Text _gameOver;
   [SerializeField]
   private Text _resetGame;
   private float _flickerWait = .5f;
   [SerializeField]
   private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameOver.gameObject.SetActive(false);
        _resetGame.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void SetLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOverText()
    {
        _resetGame.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerCoroutine());
    }

   IEnumerator GameOverFlickerCoroutine()
   {
        while (true)
        {
            if (_gameOver.gameObject.activeSelf)
                {
                    _gameOver.gameObject.SetActive(false);
                }
            else 
                {
                    _gameOver.gameObject.SetActive(true);
                }
            yield return new WaitForSeconds(_flickerWait);
        }
   }
}
