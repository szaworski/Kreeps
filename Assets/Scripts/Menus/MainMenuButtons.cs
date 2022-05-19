﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject newGameButton;
    public GameObject optionsButton;
    public GameObject exitGameButton;
    public GameObject backButton;
    public Animator transition;
    public bool newGameButtonTriggered;
    public bool optionsButtonTriggered;
    public bool exitGameButtonTriggered;

    void Awake()
    {
        newGameButtonTriggered = false;
        optionsButtonTriggered = false;
        exitGameButtonTriggered = false;
    }

    public void StartNewGame()
    {
        if (!newGameButtonTriggered)
        {
            newGameButtonTriggered = true;
            //Load the main game scene
            StartCoroutine(LoadScene(1));
        }
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        //Trigger the "ScreenWipe_Start" animation
        transition.SetTrigger("Start");
        //Wait for desired num of seconds (Async Operation was too quick)
        yield return new WaitForSeconds(1.5f);
        //Load the next scene based on the value of "sceneIndex"
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Wait until the end of the frame after all cameras and GUI are rendered, just before displaying the frame on screen. (Quoted from Unity docs)
        yield return new WaitForEndOfFrame(); 
    }
}
