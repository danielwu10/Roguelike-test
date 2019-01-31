﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
    {
        AudioManager.instance.musicSource.Play();
        SceneManager.LoadScene(sceneIndex);
       
    }

    public void RestartGame()
    {
        AudioManager.instance.musicSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }
}