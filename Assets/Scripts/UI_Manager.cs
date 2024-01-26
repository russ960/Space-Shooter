using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // Start is called before the first frame update
    void Start()
    {
        _gameOver.gameObject.SetActive(false);
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
        _gameOver.gameObject.SetActive(true);
    }
}
