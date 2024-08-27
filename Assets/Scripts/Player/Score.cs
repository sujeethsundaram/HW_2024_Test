using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverText;

    //hold player score and game time values for the scoreboard
    private int playerScore = 0;
    private float gameTime = 0f;

    private void Awake()
    {
        Death.OnPlayerDeath += ShowGameOverScreen;

        // remocve game over screen at the start of the game
        gameOverScreen.SetActive(false);
    }

    void Start()
    {
        // stasrt scoreboard text
        scoreText.text = "Score\n" + playerScore.ToString();
        timeText.text = "Time\n" + gameTime.ToString("#.#") + "s";
    }

    void Update()
    {
        // update game time
        gameTime += Time.deltaTime;

        // update scoreboard text values for player score and time
        scoreText.text = "Score\n" + playerScore.ToString();
        timeText.text = "Time\n" + gameTime.ToString("#.#") + "s";
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If collision object is a pulpit,and ONLY if the player hasn't touched the same pulpit instance before, update player score 

        if (collision.gameObject.CompareTag("Pulpit") && 
            !collision.gameObject.GetComponent<PulpitBehavior>().hasContactedPlayer)
        {
            playerScore++;
        }
    }

    private void OnDestroy()
    {
        Death.OnPlayerDeath -= ShowGameOverScreen;
    }

    void ShowGameOverScreen()
    {
        // summarize score and add it as text on the game over screen & disable scoreboard UI and how game over screen 

        gameOverText.text = "Game Over!" + "\nScore: " + playerScore.ToString();
        gameOverScreen.SetActive(true);
        scoreboard.SetActive(false);
    }
}


