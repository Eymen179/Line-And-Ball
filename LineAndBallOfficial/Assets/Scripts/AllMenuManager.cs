using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllMenuManager : MonoBehaviour
{
    //All menu, object, ball and manager variables
    public GameObject mainMenu, optionsMenu, pauseMenu, afterDeathMenu;
    public GameObject usableObjectsManager, harmfulObjectsManager;
    public GameObject ball, drawingManager, allWalls;

    //Extra object manager script variables
    private AllUsableObjectsPositions allUsableObjects;
    private AllHarmfulObjects allHarmfulObjects;
    private BallPositions ballPositions;
    private BallCollisions ballCollisions;

    private Camera cam;//Camera variables

    //Ball variables
    private Rigidbody2D ballRigidbody;
    private Vector2 storedBallVelocity;
    private float storedBallAngularVelocity;
    private int clickCounter = 0;

    //Menu Sfx
    private AudioSource menuAudio;

    //Variables For Sound Options
    private bool isSoundOn = true;
    public Button btnSoundControl;
    public AudioSource drawingAudio, ballInteractionAudio;
    public Sprite soundOn, soundOff;

    //Variables For Background Options
    public Button btnBackgroundColorSeting;
    public GameObject backgroundWhite, backgroundBlack;
    public Color clrWhite, clrWhite2, clrBlack;
    public GameObject leftBck, rightBck;
    void Start()//Baþlangýç durumlarý için gerekli ayarlar ve instance ayarlarý
    {

        cam = Camera.main;

        menuAudio = GetComponent<AudioSource>();

        allHarmfulObjects = FindObjectOfType<AllHarmfulObjects>();
        allUsableObjects = FindObjectOfType<AllUsableObjectsPositions>();
        ballPositions = FindObjectOfType<BallPositions>();
        ballCollisions = FindObjectOfType<BallCollisions>();

        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        afterDeathMenu.SetActive(false);

        ball.SetActive(false);
        drawingManager.SetActive(false);

        ballRigidbody = ball.GetComponent<Rigidbody2D>();

        //Background color and sound settings data pull
        LoadBackgroundColor();
        LoadSoundSettings();
    }
    //------------------Main Menu-----------------------------------------------------------------------
    public void PlayButton()
    {
        menuAudio.Play();
        //Main menu kapatýldý ve oyun baþladý, top ve obje oluþturucularý aktif edildi.
        ball.transform.localScale = new Vector3 (1f, 1f, 0f);
        cam.transform.position = new Vector3(0,0,-10);
        allWalls.transform.position = new Vector3(0, 0, 0);
        clickCounter++;
        if(clickCounter > 1)//Topun önceki hýz verileri yüklenir.
        {
            ballRigidbody.velocity = storedBallVelocity;
            ballRigidbody.angularVelocity = storedBallAngularVelocity;
            ballRigidbody.isKinematic = false;

            allUsableObjects.UsableObjectSpawnerOnStart();
            allHarmfulObjects.HarmfulObjectSpawnerOnStart();
        }
    }
    public void OptionsButton()
    {
        menuAudio.Play();
        //Main menu kapatýldý ve options menu açýldý.
    }
    public void QuitButton()//Oyundan çýkýþ
    {
        menuAudio.Play();
        Application.Quit();
    }
    //-----------------Options Menu--------------------------------------------------------------------
    public void SoundControlButton()
    {
        menuAudio.Play();
        SoundChanger();//Ses ayar metodu
    }
    public void BackgroundColorSettingButton()
    {
        menuAudio.Play();
        BackgroundColorChanger();//Arka plan ayar metodu
    }
    public void MainMenuInOptionsButton()
    {
        menuAudio.Play();
        //Options menu kapatýldý ve main menu açýldý.
    }
    //-----------------In Game UI----------------------------------------------------------------------
    //Pause Menu
    public void PauseButton()
    {
        menuAudio.Play();
        //Oyun(topun hareketi) durduruldu ve pause menu açýldý.

        //Topun güncel hýz verileri saklanýr.
        storedBallVelocity = ballRigidbody.velocity;
        storedBallAngularVelocity = ballRigidbody.angularVelocity;

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = 0;
        ballRigidbody.isKinematic = true;

        //Top ve çizgi görünümleri kapatýlýr.
        ball.SetActive(false);
        ballPositions.txtCountdown.text = "";

        foreach (GameObject obj in DrawingLineScript.lines)
            obj.SetActive(false);
    }
    public void MainMenuInPauseButton()
    {
        
        //Pause menu kapatýldý ve main menu açýldý, topun aktifliði kapatýldý.
        //ReturnMainMenu();
        //Sahne baþtan yüklenir, mürekkep sýfýrlanýr ve çizgiler silinir.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DrawingLineScript.incAmount = 100f;
        DrawingLineScript.lines.Clear();
        menuAudio.Play();
    }
    public void ContinueButton()
    {
        menuAudio.Play();
        //Oyun(topun hareketi) aktif edildi ve pause menu kapatýldý.

        ballRigidbody.velocity = storedBallVelocity;
        ballRigidbody.angularVelocity = storedBallAngularVelocity;
        ballRigidbody.isKinematic = false;

        ball.SetActive(true);
        //Çizgi görünümleri aktif edildi.
        foreach (GameObject obj in DrawingLineScript.lines)
            obj.SetActive(true);
    }
    //After Death Menu
    public void MainMenuInAfterDeathButton()
    {
        //After Death menu kapatýldý ve main menu açýldý, topun aktifliði kapatýldý.
        //Sahne baþtan yüklenir, mürekkep sýfýrlanýr ve çizgiler silinir.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DrawingLineScript.incAmount = 100f;
        DrawingLineScript.lines.Clear();
        
        menuAudio.Play();
    }
    public void RestartButton()
    {
        menuAudio.Play();
        //Oyun içinde yapýlmýþ güncel deðiþiklikler sýfýrlanýr ve oyuna baþtan baþlanýr.
        ObjectCleaner();
        DrawingLineScript.incAmount = 100f;
        ballCollisions.s = 0;
        Time.timeScale = 1.0f;
        ballPositions.scoreText.text = ballPositions.score.ToString();
        ballCollisions.ballTrail.GetComponent<TrailRenderer>().enabled = true;
        PlayButton();
    }
    //-------------------Supporting methods-------------------------------------------------------------------
    private void LoadSoundSettings()//Oyun baþýnda PlayerPrefs'te bulunan ses verilerini yükler
    {
        string soundSetting = PlayerPrefs.GetString("Sound", "soundOn");

        if(soundSetting == "soundOff")
        {
            btnSoundControl.GetComponent<Image>().sprite = soundOff;
            menuAudio.mute = ballInteractionAudio.mute = drawingAudio.mute = true;
            isSoundOn = false;
        }
        else
        {
            btnSoundControl.GetComponent<Image>().sprite = soundOn;
            menuAudio.mute = ballInteractionAudio.mute = drawingAudio.mute = false;
            isSoundOn = true;
        }
    }
    private void SoundChanger()//Ses butonu için ses ayarlarý
    {
        if (isSoundOn)
        {
            btnSoundControl.GetComponent<Image>().sprite = soundOff;
            menuAudio.mute = ballInteractionAudio.mute = drawingAudio.mute = true;
            isSoundOn = false;
            PlayerPrefs.SetString("Sound", "soundOff");
        }
        else
        {
            btnSoundControl.GetComponent<Image>().sprite = soundOn;
            menuAudio.mute = ballInteractionAudio.mute = drawingAudio.mute = false;
            isSoundOn = true;
            PlayerPrefs.SetString("Sound", "soundOn");
        }
    }
    private void LoadBackgroundColor()//Oyun baþýnda PlayerPrefs'te bulunan arka plan verilerini yükler
    {
        string colorName = PlayerPrefs.GetString("Background", "white");

        if (colorName == "black")
        {
            btnBackgroundColorSeting.GetComponent<Image>().color = clrWhite;
            mainMenu.GetComponent<Image>().color = optionsMenu.GetComponent<Image>().color = clrBlack;
            //leftBck.GetComponent<Image>().color = rightBck.GetComponent<Image>().color = clrBlack;
            cam.backgroundColor = clrBlack;
        }
        else
        {
            btnBackgroundColorSeting.GetComponent<Image>().color = clrBlack;
            mainMenu.GetComponent<Image>().color = optionsMenu.GetComponent<Image>().color = clrWhite2;
            //leftBck.GetComponent<Image>().color = rightBck.GetComponent<Image>().color = clrWhite;
            cam.backgroundColor = clrWhite;
        }
    }
    private void BackgroundColorChanger()//Arka plan butonu için arka plan ayarlarý
    {
        if (cam.backgroundColor == clrWhite)
        {
            btnBackgroundColorSeting.GetComponent<Image>().color = clrWhite;
            mainMenu.GetComponent<Image>().color = optionsMenu.GetComponent<Image>().color = clrBlack;
            //leftBck.GetComponent<Image>().color = rightBck.GetComponent<Image>().color = clrBlack;
            cam.backgroundColor = clrBlack;

            PlayerPrefs.SetString("Background", "black");
        }
        else if (cam.backgroundColor == clrBlack)
        {
            btnBackgroundColorSeting.GetComponent<Image>().color = clrBlack;
            mainMenu.GetComponent<Image>().color = optionsMenu.GetComponent<Image>().color = clrWhite2;
            //leftBck.GetComponent<Image>().color = rightBck.GetComponent<Image>().color = clrWhite;
            cam.backgroundColor = clrWhite;

            PlayerPrefs.SetString("Background", "white");
        }
        PlayerPrefs.Save();
    }

    private void ObjectCleaner()//Oyunda bulunan tüm yararlý ve zararlý objeler ile çizgiler silinir ve skor sýfýrlanýr.
    {
        ballPositions.score = 0;

        foreach (GameObject obj in AllUsableObjectsPositions.createdUsableObjects)
            Destroy(obj);
        foreach (GameObject obj in AllHarmfulObjects.createdHarmfulObjects)
            Destroy(obj);
        foreach (GameObject obj in DrawingLineScript.lines)
            Destroy(obj);
        DrawingLineScript.lines.Clear();
    }

}
