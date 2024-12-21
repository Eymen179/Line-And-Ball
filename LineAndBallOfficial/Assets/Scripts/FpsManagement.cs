using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsManagement : MonoBehaviour
{
    private int updateCounter = 0;
    void Awake()
    {
        // Ýlk baþlatma sýrasýnda FPS'yi ayarla
        SetFrameRate();
    }

    void Update()
    {
        if(updateCounter == 0)
        {
            //Hedeflenen FPS ekran FPS'i ile eþit deðilse yapýlacak iþlemler
            if (Application.targetFrameRate != (int)Screen.currentResolution.refreshRateRatio.numerator)
            {
                SetFrameRate();
            }
            updateCounter++;
        }
        // FPS ayarýnýn doðru þekilde uygulanýp uygulanmadýðýný sürekli kontrol et

    }

    private void SetFrameRate()//Oyunun FPS'i ekran Hz'ine eþitlenir
    {
        int refreshRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
        Application.targetFrameRate = refreshRate;
        Debug.Log("FPS ayarlandý: " + refreshRate + " Hz");
    }

}
