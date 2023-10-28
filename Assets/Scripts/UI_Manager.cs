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
   
    // Start is called before the first frame update
    void Start()
    {

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
}
