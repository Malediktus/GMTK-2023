using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetMusicValue(Single Value)
    {
        PlayerPrefs.SetFloat("MusicVol", Value);
    }

    public void SetSoundValue(Single Value)
    {
        PlayerPrefs.SetFloat("SoundVol", Value);
    }
}
