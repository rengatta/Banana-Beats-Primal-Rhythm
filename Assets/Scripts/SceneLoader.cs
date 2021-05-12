using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

//used to load scenes
//sort of redundant and not used properly, since scene loading can be done in one line
//should be removed if necessary
public class SceneLoader : MonoBehaviour
{
    public SceneField level1;

    public FadeInOut fadeInOut;

    IEnumerator LoadNextScene() {
        SceneManager.LoadScene(level1);
        yield return null;
    }


    IEnumerator LoadThisScene(SceneField scene) {
        SceneManager.LoadScene(scene);
        yield return null;
    }

    public void LoadScene(SceneField scene) {
        fadeInOut.FadeOut(LoadThisScene(scene));
    }



    public void Loadgame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        fadeInOut.FadeOut(LoadNextScene());
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!"); // Used for quitting in the debug since you can't actually quit in unity editor.
    }
}
