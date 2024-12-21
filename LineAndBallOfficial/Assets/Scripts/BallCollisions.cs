using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallCollisions : MonoBehaviour
{
    public static Rigidbody2D ballRigidbody;
    public float bouncePower;
    public TrailRenderer ballTrail;
    //ball sfx
    private AudioSource ballAudio;
    public AudioClip ballHitSound, ballShrinkSound, ballMagnifySound, incBallSound, threeSecSlowerSound, spikeSound, jumpSound;
    public float ballHitVolume, ballShrinkVolume, ballMagnifyVolume, incBallVolume, threeSecSlowerVolume, spikeVolume, jumpVolume;

    //variables that get interacting with ball
    public GameObject drawingManager;
    private AllMenuManager allMenuManager;

    

    //shrink controller
    public int s = 0;
    public TextMeshProUGUI txtShrinkWarning;

    //ball and objects positions
    private Transform ball;
    private float randomX;
    private float randomY;

    private float originTimeScale;
    public static Vector3 ballVelocity;
    public static float angularVelocity;
    private void Start()
    {
        txtShrinkWarning.text = "";
        ballAudio = GetComponent<AudioSource>();

        allMenuManager = FindObjectOfType<AllMenuManager>();

        originTimeScale = Time.timeScale;

        ball = GetComponent<Transform>();
        ballTrail = GetComponent<TrailRenderer>();

        ballRigidbody = GetComponent<Rigidbody2D>();
        ballVelocity = ballRigidbody.velocity;
        angularVelocity = ballRigidbody.angularVelocity;
        ballRigidbody.isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)//Temas edildiðinde ýþýnlanacak objeler için gerekli iþlemler
    {
        if (collision.gameObject.tag == "IncBall")
        {
            DrawingLineScript.incAmount += 10;

            randomX = Random.Range(-1.6f, 1.6f);
            randomY = Random.Range(12f, 20f);
            collision.gameObject.transform.position = new Vector2(randomX, collision.gameObject.transform.position.y - randomY);

            ballAudio.PlayOneShot(incBallSound, incBallVolume);
        }

        if (collision.gameObject.tag == "Minimiser")
        {
            randomX = Random.Range(-1.6f, 1.6f);
            randomY = Random.Range(15f, 40f);
            collision.gameObject.transform.position = new Vector2(randomX, collision.gameObject.transform.position.y - randomY);
            ball.localScale = new Vector3(ball.localScale.x / 1.5f, ball.localScale.y / 1.5f, 0);
            ballTrail.GetComponent<TrailRenderer>().startWidth = ballTrail.GetComponent<TrailRenderer>().startWidth / 2;
            s++;

            ballAudio.PlayOneShot(ballShrinkSound, ballShrinkVolume);
            if (s != 2)
                txtShrinkWarning.text = "";
            else
                txtShrinkWarning.text = "TOO SMALL";
            if (s == 3)//Game Over
                BallResetter();
        }
        if (collision.gameObject.tag == "Magnifyer")
        {
            randomX = Random.Range(-1.6f, 1.6f);
            randomY = Random.Range(15f, 40f);
            collision.gameObject.transform.position = new Vector2(randomX, collision.gameObject.transform.position.y - randomY);

            ball.localScale = new Vector3(ball.localScale.x * 1.5f, ball.localScale.y * 1.5f, 0);
            ballTrail.GetComponent<TrailRenderer>().startWidth = ballTrail.GetComponent<TrailRenderer>().startWidth * 2;
            s--;
            txtShrinkWarning.text = "";
            ballAudio.PlayOneShot(ballMagnifySound, ballMagnifyVolume);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//Zaman yavaþlatýcýsý ile temas iþlemi bittiðinde yapýlacak iþlemler
    {
        if (collision.gameObject.tag == "ThreeSecSlower")
        {
            StartCoroutine(SlowTimeForSec(3f, 0.5f));
            ballAudio.PlayOneShot(threeSecSlowerSound, threeSecSlowerVolume);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//Topun dokunduðu objeler ile ilgili iþlemler
    {   //Game Over
        if (collision.gameObject.tag == "Spike" || collision.gameObject.tag == "LeftWallSpike" || collision.gameObject.tag == "RightWallSpike")
        {
            ballAudio.PlayOneShot(spikeSound, spikeVolume);
            ballTrail.GetComponent<TrailRenderer>().enabled = false;
            BallResetter();
        }
        if(collision.gameObject.tag == "Tramboline")
            ballAudio.PlayOneShot(jumpSound, jumpVolume);

        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "MovingPlatform" || collision.gameObject.tag == "SpikedPlatform" ||
            collision.gameObject.tag == "LeftWall" || collision.gameObject.tag == "RightWall" || collision.gameObject.tag == "Line" || collision.gameObject.tag == "Tramboline" ||
            collision.gameObject.name == "BasicPlatform" || collision.gameObject.name == "BasicPlatform (1)")
            ballAudio.PlayOneShot(ballHitSound, ballHitVolume);
    }
    public void BallResetter()//Top ile ilgili hýz, iz gibi veriler ile çigiler sýfýrlanýr.
    {
        txtShrinkWarning.text = "";

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = 0;
        ballRigidbody.isKinematic = true;

        ballTrail.GetComponent<TrailRenderer>().startWidth = 0.5f;

        allMenuManager.afterDeathMenu.SetActive(true);
        foreach (GameObject obj in DrawingLineScript.lines)
            Destroy(obj);
        drawingManager.SetActive(false);
    }
    private IEnumerator SlowTimeForSec(float duration, float newTimeScale)//Zaman yavaþlatýcýsýnýn kullandýðý metot
    {
        Time.timeScale = newTimeScale;
        for (int i = (int)duration; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            //tickSound.Play();
        }
        ballAudio.PlayOneShot(ReverseScript.ReverseAudioClip(threeSecSlowerSound), threeSecSlowerVolume);
        
        Time.timeScale = originTimeScale;
    }
}
