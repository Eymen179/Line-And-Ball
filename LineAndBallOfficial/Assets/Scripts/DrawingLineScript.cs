using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawingLineScript : MonoBehaviour
{
    //line prefab and line manager
    public GameObject linePrefab;
    private GameObject currentLine;
    public static List<GameObject> lines = new List<GameObject>();

    //line variables
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPositions;
    
    //variables for ink
    public int lineCount = 0;
    
    public static float incAmount = 100f;
    public Slider drawingProgressBar;
    //For Testing
    public TextMeshProUGUI incAmountText;

    //line sfx
    private AudioSource lineAudio;
    public AudioClip drawingSound;
    public float drawingVolume;

    private void Start()
    {
        lineAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateDrawingProgressBar();//Mürekkep barýný ayarlayan metot

        incAmountText.text = "% " + Mathf.CeilToInt(incAmount).ToString();

        //Parmaðýmýzla bir kere bastýðýmýzda yapýlacak iþlemler
        if (Input.GetMouseButtonDown(0) && incAmount > 0f)
        {
            CreateLine();
            incAmount -= 0.2f;

            lineAudio.PlayOneShot(drawingSound, drawingVolume);
        }
        //Parmaðýmýzý basýlý tuttuðumuzda yapýlacak iþlemler
        if (Input.GetMouseButton(0) && incAmount > 0f)
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos);
                incAmount -= 0.2f;
            }
        }
    }
    void CreateLine()
    {
        //Çizgi oluþturulur ve listeye eklenir.
        lineCount++;
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lines.Add(currentLine);

        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();

        //Parmak pozisyonu ayarý
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);

        edgeCollider.points = fingerPositions.ToArray();
    }
    void UpdateLine(Vector2 newFingerPos)//Parmak ile ekrana basýlý hareket ettikçe yapýlacak iþlemler
    {
        newFingerPos.x = Mathf.Clamp(newFingerPos.x, -11f, 11f);
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);

        edgeCollider.points = fingerPositions.ToArray();
    }
    void UpdateDrawingProgressBar()
    {
        if (drawingProgressBar != null)
        {
            drawingProgressBar.value = incAmount / 100f;
        }
    }
}
