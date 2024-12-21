using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHarmfulObjects : MonoBehaviour
{
    //Zararlý objelerin kullanýlacaðý yere göre listeleri
    public GameObject[] harmfulObjects;
    public static GameObject[] createdHarmfulObjects;

    private float randomX, randomY, randomZ;
    private Quaternion quaternion;

    private bool[] positionChangeControl;//Her bir obje için konum deðiþikliðini kontrol eden bool deðiþkenleri
    private bool canUpdate = false;

    void Start()
    {
        HarmfulObjectSpawnerOnStart();
    }

    void Update()
    {
        HarmfulObjectSpawnerInUpdate();
    }
    public void HarmfulObjectSpawnerOnStart()
    {
        //Dizi ayarlarý
        createdHarmfulObjects = new GameObject[harmfulObjects.Length];
        positionChangeControl = new bool[harmfulObjects.Length];
        System.Array.Fill(positionChangeControl, false);

        if (harmfulObjects.Length == 0)//Hata kontrol
            Debug.Log("Error, empty array");

        Shuffle(harmfulObjects);//Baþlangýçta objelerin bulunduðu diziyi rastgele karmaya yarayan algoritma

        //Objeler -y ekseninde sabit aralýklarla sýrayla spawnlanýr.
        for (int i = 0; i < harmfulObjects.Length; i++)
        {
            randomX = Random.Range(-1.6f, 1.6f);
            randomZ = Random.Range(0f, 180f);
            
            createdHarmfulObjects[i] = Instantiate(harmfulObjects[i], 
                                        new Vector2(randomX, (i + 1) * (-15)), Quaternion.Euler(0, 0, randomZ));
        }

        StartCoroutine(StartDelay());//Spawn iþinin düzgün iþlemesi için zorunlu geciktirme uygulanýr.
    }
    public void HarmfulObjectSpawnerInUpdate()
    {
        if (!canUpdate) return;

        //Objeler kamera kadrajýndan çýktýktan sonra y ekseninde aþaðýya doðru belirtildiði konuma ýþýnlanýr
        for (int i = 0; i < createdHarmfulObjects.Length; i++)
        {
            randomX = Random.Range(-1.6f, 1.6f);
            randomZ = Random.Range(0f, 180f);

            GameObject obj = createdHarmfulObjects[i];
            Renderer objRenderer = obj.GetComponent<Renderer>();

            if (objRenderer != null)
            {
                if (!positionChangeControl[i] && !objRenderer.isVisible)
                {
                    switch (obj.tag)//Her objenin kendine özel ýþýnlanma mesafesi vardýr.
                    {
                        case var v when (v == "SpikedPlatform" || v == "TwoSidedSpikedPlatform"):
                            randomY = Random.Range(15f, 25f);
                            quaternion = Quaternion.Euler(0, 0, randomZ);
                            obj.transform.position = new Vector2(randomX, obj.transform.position.y - randomY);
                            break;
                        case "LeftWallSpike":
                            randomY = Random.Range(15f, 25f);
                            quaternion = Quaternion.Euler(0, 0, -90);
                            obj.transform.position = new Vector2(-2.24f, obj.transform.position.y - randomY);
                            break;
                        case "RightWallSpike":
                            randomY = Random.Range(15f, 25f);
                            quaternion = Quaternion.Euler(0, 0, -90);
                            obj.transform.position = new Vector2(2.24f, obj.transform.position.y - randomY);
                            break;
                        default:
                            break;
                    }
                    positionChangeControl[i] = true;

                    obj.transform.rotation = quaternion;
                }
                else if (objRenderer.isVisible)
                {
                    positionChangeControl[i] = false;
                }
            }
        }
    }
    private static void Shuffle<T>(T[] array)//Dizi elemanlarýný rastgele karmaya yarayan algoritma metodu
    {
        int rng = (int)Random.Range(0f, array.Length);
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng;
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
    IEnumerator StartDelay()//Geciktirme metodu
    {
        yield return new WaitForSeconds(2f);
        canUpdate = true;
    }
}
