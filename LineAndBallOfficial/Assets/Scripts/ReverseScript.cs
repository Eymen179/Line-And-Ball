using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseScript : MonoBehaviour
{
    public static AudioClip ReverseAudioClip(AudioClip originalSound)
    {
        float[] samples = new float[originalSound.samples];//Ses verisi bir dizinin elemanlarý gibi ayrýlýr.
        originalSound.GetData(samples, 0);

        Array.Reverse(samples);//Dizinin elemanlarý ters çevrilir.

        //Ters çevrilmiþ dizi ses verisine dönüþtürülür ve kullanýma sunulur.
        AudioClip reversedClip = AudioClip.Create("ReversedClip", samples.Length, 
                                                originalSound.channels, originalSound.frequency, false);
        reversedClip.SetData(samples, 0);


        return reversedClip;
    }

}
