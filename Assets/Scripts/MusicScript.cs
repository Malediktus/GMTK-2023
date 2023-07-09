using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private void Update()
    {
        AudioSource[] MusicOnObject = GetComponents<AudioSource>();

        for (int i = 0; i < MusicOnObject.Length; i++)
        {
            MusicOnObject[i].volume = PlayerPrefs.GetFloat("MusicVol");
        }
    }
}
