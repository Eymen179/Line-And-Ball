using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsManagement : MonoBehaviour
{
    private int updateCounter = 0;
    void Awake()
    {
        // �lk ba�latma s�ras�nda FPS'yi ayarla
        SetFrameRate();
    }

    void Update()
    {
        if(updateCounter == 0)
        {
            //Hedeflenen FPS ekran FPS'i ile e�it de�ilse yap�lacak i�lemler
            if (Application.targetFrameRate != (int)Screen.currentResolution.refreshRateRatio.numerator)
            {
                SetFrameRate();
            }
            updateCounter++;
        }
        // FPS ayar�n�n do�ru �ekilde uygulan�p uygulanmad���n� s�rekli kontrol et

    }

    private void SetFrameRate()//Oyunun FPS'i ekran Hz'ine e�itlenir
    {
        int refreshRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
        Application.targetFrameRate = refreshRate;
        Debug.Log("FPS ayarland�: " + refreshRate + " Hz");
    }

}
