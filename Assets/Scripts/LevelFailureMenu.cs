using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
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
