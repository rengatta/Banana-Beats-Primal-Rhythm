using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneLoader : MonoBehaviour
{
    public SceneField level1;
   

    public void Loadgame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(level1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!"); // Used for quitting in the debug since you can't actually quit in unity editor.
    }
}
