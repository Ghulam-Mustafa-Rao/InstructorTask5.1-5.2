using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject barelHolder;
    public GameObject barelPrefab;
    public GameObject gorila;
    public Animator gorilaAnimator;
    public GameObject LeftBound;
    public PlayerController player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    int score = 0;
    int lives = 0;
    bool gameOver = false;


    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        livesText.text = "Lives : " + lives;
        UpdateScore(0);
        StartCoroutine(SpwanBarrels());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpwanBarrels()
    {
        while (!gameOver)
        {
            gorilaAnimator.SetBool("move_b", true);
            
            yield return new WaitForSeconds(0.67f);
            gorilaAnimator.SetBool("move_b", false);
            Instantiate(barelPrefab, barelHolder.transform.position, barelPrefab.transform.rotation).transform.SetParent(barelHolder.transform);
            yield return new WaitForSeconds(7);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score : " + score;
    }

    public void UpdateLives()
    {
        lives--;
        livesText.text = "Lives : " + lives;
        player.ResetPlayer();
    }
}
