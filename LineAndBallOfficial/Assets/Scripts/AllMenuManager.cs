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
    void Start()//Ba�lang�� durumlar� i�in gerekli ayarlar ve instance ayarlar�
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
        //Main menu kapat�ld� ve oyun ba�lad�, top ve obje olu�turucular� aktif edildi.
        ball.transform.localScale = new Vector3 (1f, 1f, 0f);
        cam.transform.position = new Vector3(0,0,-10);
        allWalls.transform.position = new Vector3(0, 0, 0);
        clickCounter++;
        if(clickCounter > 1)//Topun �nceki h�z verileri y�klenir.
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
        //Main menu kapat�ld� ve options menu a��ld�.
    }
    public void QuitButton()//Oyundan ��k��
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
        //Options menu kapat�ld� ve main menu a��ld�.
    }
    //-----------------In Game UI----------------------------------------------------------------------
    //Pause Menu
    public void PauseButton()
    {
        menuAudio.Play();
        //Oyun(topun hareketi) durduruldu ve pause menu a��ld�.

        //Topun g�ncel h�z verileri saklan�r.
        storedBallVelocity = ballRigidbody.velocity;
        storedBallAngularVelocity = ballRigidbody.angularVelocity;

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = 0;
        ballRigidbody.isKinematic = true;

        //Top ve �izgi g�r�n�mleri kapat�l�r.
        ball.SetActive(false);
        ballPositions.txtCountdown.text = "";

        foreach (GameObject obj in DrawingLineScript.lines)
            obj.SetActive(false);
    }
    public void MainMenuInPauseButton()
    {
        
        //Pause menu kapat�ld� ve main menu a��ld�, topun aktifli�i kapat�ld�.
        //ReturnMainMenu();
        //Sahne ba�tan y�klenir, m�rekkep s�f�rlan�r ve �izgiler silinir.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DrawingLineScript.incAmount = 100f;
        DrawingLineScript.lines.Clear();
        menuAudio.Play();
    }
    public void ContinueButton()
    {
        menuAudio.Play();
        //Oyun(topun hareketi) aktif edildi ve pause menu kapat�ld�.

        ballRigidbody.velocity = storedBallVelocity;
        ballRigidbody.angularVelocity = storedBallAngularVelocity;
        ballRigidbody.isKinematic = false;

        ball.SetActive(true);
        //�izgi g�r�n�mleri aktif edildi.
        foreach (GameObject obj in DrawingLineScript.lines)
            obj.SetActive(true);
    }
    //After Death Menu
    public void MainMenuInAfterDeathButton()
    {
        //After Death menu kapat�ld� ve main menu a��ld�, topun aktifli�i kapat�ld�.
        //Sahne ba�tan y�klenir, m�rekkep s�f�rlan�r ve �izgiler silinir.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DrawingLineScript.incAmount = 100f;
        DrawingLineScript.lines.Clear();
        
        menuAudio.Play();
    }
    public void RestartButton()
    {
        menuAudio.Play();
        //Oyun i�inde yap�lm�� g�ncel de�i�iklikler s�f�rlan�r ve oyuna ba�tan ba�lan�r.
        ObjectCleaner();
        DrawingLineScript.incAmount = 100f;
        ballCollisions.s = 0;
        Time.timeScale = 1.0f;
        ballPositions.scoreText.text = ballPositions.score.ToString();
        ballCollisions.ballTrail.GetComponent<TrailRenderer>().enabled = true;
        PlayButton();
    }
    //-------------------Supporting methods-------------------------------------------------------------------
    private void LoadSoundSettings()//Oyun ba��nda PlayerPrefs'te bulunan ses verilerini y�kler
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
    private void SoundChanger()//Ses butonu i�in ses ayarlar�
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
    private void LoadBackgroundColor()//Oyun ba��nda PlayerPrefs'te bulunan arka plan verilerini y�kler
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
    private void BackgroundColorChanger()//Arka plan butonu i�in arka plan ayarlar�
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

    private void ObjectCleaner()//Oyunda bulunan t�m yararl� ve zararl� objeler ile �izgiler silinir ve skor s�f�rlan�r.
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
