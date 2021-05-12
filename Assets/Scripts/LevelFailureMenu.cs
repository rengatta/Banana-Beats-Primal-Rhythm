using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

//script for the level failure scene. mostly just changed the gamestate to failed and adds scene loads to the buttons
public class LevelFailureMenu : MonoBehaviour
{

    public SceneField levelSelectScene;
    public SceneField songScene;
    public SceneLoader sceneLoader;

    private void Start()
    {
        GameState.failed = false;
    }


    public void LevelSelectButtonPressed() {
        sceneLoader.LoadScene(levelSelectScene);
    }

    public void RetryButtonPressed() {
        sceneLoader.LoadScene(songScene);

    }

}
