using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUsableObjectsPositions : MonoBehaviour
{
    //Yararl� objelerin kullan�laca�� yere g�re listeleri
    public GameObject[] usableObjects;
    public static GameObject[] createdUsableObjects;

    private float randomX, randomY, randomZ;
    Quaternion quaternion;

    bool[] positionChangeControl;//Her bir obje i�in konum de�i�ikli�ini kontrol eden bool de�i�kenleri
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
        //Dizi ayarlar�
        createdUsableObjects = new GameObject[usableObjects.Length];
        positionChangeControl = new bool[usableObjects.Length];
        System.Array.Fill(positionChangeControl, false);


        if (usableObjects.Length == 0)//Hata kontrol
            Debug.Log("Error, empty array");

        Shuffle(usableObjects);//Ba�lang��ta objelerin bulundu�u diziyi rastgele karmaya yarayan algoritma

        //Objeler -y ekseninde sabit aral�klarla s�rayla spawnlan�r.
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

        StartCoroutine(StartDelay());//Spawn i�inin d�zg�n i�lemesi i�in zorunlu geciktirme uygulan�r.
    }
    public void UsableObjectSpawnerInUpdate()
    {
        if (!canUpdate) return;

        //Objeler kamera kadraj�ndan ��kt�ktan sonra y ekseninde a�a��ya do�ru belirtildi�i konuma ���nlan�r
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
                    switch (obj.tag)//Her objenin kendine �zel ���nlanma mesafesi vard�r.
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

    private static void Shuffle<T>(T[] array)//Dizi elemanlar�n� rastgele karmaya yarayan algoritma metodu
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
