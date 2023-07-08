using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    void Start()
    {
        AudioSource[] SoundsOnObject = GetComponents<AudioSource>();

        for (int i = 0; i < SoundsOnObject.Length; i++)
        {
            SoundsOnObject[i].volume = PlayerPrefs.GetFloat("SoundVol");
        }
    }
}
