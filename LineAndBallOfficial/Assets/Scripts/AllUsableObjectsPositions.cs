using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUsableObjectsPositions : MonoBehaviour
{
    //Yararlý objelerin kullanýlacaðý yere göre listeleri
    public GameObject[] usableObjects;
    public static GameObject[] createdUsableObjects;

    private float randomX, randomY, randomZ;
    Quaternion quaternion;

    bool[] positionChangeControl;//Her bir obje için konum deðiþikliðini kontrol eden bool deðiþkenleri
    private bool canUpdate = false;

    void Start()
    {
        UsableObjectSpawnerOnStart();
    }

    void Update()
    {
        UsableObjectSpawnerInUpdate();
    }
    public void UsableObjectSpawnerOnStart()
    {
        //Dizi ayarlarý
        createdUsableObjects = new GameObject[usableObjects.Length];
        positionChangeControl = new bool[usableObjects.Length];
        System.Array.Fill(positionChangeControl, false);


        if (usableObjects.Length == 0)//Hata kontrol
            Debug.Log("Error, empty array");

        Shuffle(usableObjects);//Baþlangýçta objelerin bulunduðu diziyi rastgele karmaya yarayan algoritma

        //Objeler -y ekseninde sabit aralýklarla sýrayla spawnlanýr.
        for (int i = 0; i < usableObjects.Length; i++)
        {
            Vector2 objPos = usableObjects[i].transform.position;
            objPos.y = 0;
            usableObjects[i].transform.position = objPos;
        }
        for (int i = 0; i < usableObjects.Length; i++)
        {
            randomX = Random.Range(-1.6f, 1.6f);
            randomZ = Random.Range(0f, 180f);
            if (usableObjects[i].tag == "Platform" || usableObjects[i].tag == "Trampoline")
                createdUsableObjects[i] = Instantiate(usableObjects[i], new Vector2(randomX, (i + 1) * (-12)), Quaternion.Euler(0, 0, randomZ));
            else
                createdUsableObjects[i] = Instantiate(usableObjects[i], new Vector2(randomX, (i + 1) * (-12)), Quaternion.identity);

        }

        StartCoroutine(StartDelay());//Spawn iþinin düzgün iþlemesi için zorunlu geciktirme uygulanýr.
    }
    public void UsableObjectSpawnerInUpdate()
    {
        if (!canUpdate) return;

        //Objeler kamera kadrajýndan çýktýktan sonra y ekseninde aþaðýya doðru belirtildiði konuma ýþýnlanýr
        for (int i = 0; i < createdUsableObjects.Length; i++)
        {
            randomX = Random.Range(-1.6f, 1.6f);
            randomZ = Random.Range(0f, 180f);

            GameObject obj = createdUsableObjects[i];
            Renderer objRenderer = obj.GetComponent<Renderer>();

            if (objRenderer != null)
            {
                if (!positionChangeControl[i] && !objRenderer.isVisible)
                {
                    switch (obj.tag)//Her objenin kendine özel ýþýnlanma mesafesi vardýr.
                    {
                        case "Platform":
                            randomY = Random.Range(15f, 20f);
                            quaternion = Quaternion.Euler(0, 0, randomZ);
                            break;
                        case "IncBall":
                            randomY = Random.Range(12f, 20f);
                            quaternion = Quaternion.identity;
                            break;
                        case var v when (v == "Minimiser" || v == "Magnifyer"):
                            randomY = Random.Range(15f, 40f);
                            quaternion = Quaternion.identity;
                            break;
                        case "Trampoline":
                            randomY = Random.Range(50f, 70f);
                            quaternion = Quaternion.Euler(0,0,randomZ);
                            break;
                        case "MovingPlatform":
                            randomY = Random.Range(50f, 100f);
                            quaternion = Quaternion.identity;
                            break;
                        case "ThreeSecSlower":
                            randomY = Random.Range(60f, 120f);
                            quaternion = Quaternion.identity;
                            break;
                        default:
                            break;
                    }
                    positionChangeControl[i] = true;
                    obj.transform.position = new Vector2(randomX, obj.transform.position.y - randomY);
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
