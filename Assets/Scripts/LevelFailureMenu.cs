using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
public class LevelFailureMenu : MonoBehaviour
{

    public SceneField levelSelectScene;
    public SceneField songScene;

    public void LevelSelectButtonPressed() {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void RetryButtonPressed() {
        SceneManager.LoadScene(songScene);

    }

}
