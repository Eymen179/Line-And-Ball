using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallPositions : MonoBehaviour
{
    private Transform ball;

    private Rigidbody2D ballRigidbody;

    //Geri sayýmla ilgili veriler
    public TextMeshProUGUI txtCountdown;
    private float countdownTime = 7f;
    private Coroutine countdownCoroutine;

    private BallCollisions ballCollisions;

    //Menü deðiþkenleri
    public TextMeshProUGUI scoreText, afterDeathScoreText, highscoreText, highscoreInMainMenuText;
    public int score = 0;
    public int highscore;

    private Vector2 ballStartPosition;
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        txtCountdown.text = "";

        ballCollisions = FindObjectOfType<BallCollisions>();

        BallStarting();//Ball Starting Options

        highscore = PlayerPrefs.GetInt("highscore", 0);

    }

    private void Update()
    {
        ScoreMeter();

        //Topun hareket etmediðinde çalýþacak iþlemler
        if (ballRigidbody.velocity.magnitude < 0.01f)
        {
            if (countdownCoroutine == null)
                countdownCoroutine = StartCoroutine(Countdown());
        }
        else
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                txtCountdown.text = "";
            }
        }
    }
    public void ScoreMeter()//Skor ölçer
    {
        if (ball.position.y < 0 && ball.position.y <= -score)
        {
            score = (int)-ball.position.y;
            scoreText.text = score.ToString();

            afterDeathScoreText.text = score.ToString();
            if(score > highscore)
            {
                highscore = score;
                highscoreText.text = highscore.ToString();

                PlayerPrefs.SetInt("highscore", highscore);
            }
            else
            {
                highscoreText.text = highscore.ToString();

            }
            PlayerPrefs.Save();
        }
    }
    public void BallStarting()//Oyun baþýnda top konumlandýrýcý
    {
        ballStartPosition = new Vector2(Random.Range(-1.6f, 1.6f), 6f);

        ball = GetComponent<Transform>();
        ball.position = ballStartPosition;
    }
    public IEnumerator Countdown()//Top belirtilen süre kadar sabit kalýrsa yapýlacak iþlemler
    {
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            if (currentTime <= 5f)
            {
                txtCountdown.text = currentTime.ToString("F0");
            }

            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        txtCountdown.text = "";

        ballCollisions.BallResetter();
    }
}
