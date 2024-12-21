using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseScript : MonoBehaviour
{
    public static AudioClip ReverseAudioClip(AudioClip originalSound)
    {
        float[] samples = new float[originalSound.samples];//Ses verisi bir dizinin elemanlar� gibi ayr�l�r.
        originalSound.GetData(samples, 0);

        Array.Reverse(samples);//Dizinin elemanlar� ters �evrilir.

        //Ters �evrilmi� dizi ses verisine d�n��t�r�l�r ve kullan�ma sunulur.
        AudioClip reversedClip = AudioClip.Create("ReversedClip", samples.Length, 
                                                originalSound.channels, originalSound.frequency, false);
        reversedClip.SetData(samples, 0);


        return reversedClip;
    }

}
