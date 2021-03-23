using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

[CustomEditor(typeof(HealthUI))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthUI myScript = (HealthUI)target;
        if (GUILayout.Button("Apply Changes"))
        {
            myScript.ValidateFunction();
        }
    }

}


public class HealthUI : MonoBehaviour
{
    

    public int maxHealth = 20;
    public int currentHealth = 20;
    public GameObject healthPartPrefab;
    public GameObject healthPartHalfPrefab;
    public GameObject healthPartNonePrefab;
    public GameObject healthUIRoot;
    public GameObject healthUIHolder;

    public float healthSpacing = 3f;


    public SceneField levelFailScene;

    public void ValidateFunction() {


        if (currentHealth < 0) currentHealth = 0;
        foreach (Transform child in healthUIHolder.transform)
        {
            
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(child.gameObject);
            };
        }

        int numHit = maxHealth - currentHealth;
        int fullHealthPart = 0;
        int halfHealthPart = 0;
        int noneHealthPart = 0;
        if (currentHealth > maxHealth / 2) {
            fullHealthPart = (maxHealth / 2) - numHit;
            halfHealthPart = (maxHealth / 2) - fullHealthPart;
            for (int i = 0; i < fullHealthPart; i++)
            {
                Instantiate(healthPartPrefab, healthUIHolder.transform);
            }

            for (int i = 0; i < halfHealthPart; i++)
            {
                Instantiate(healthPartHalfPrefab, healthUIHolder.transform);
            }
        }
        else {
            noneHealthPart = (maxHealth / 2) - currentHealth;
            halfHealthPart = currentHealth;
            for (int i = 0; i < halfHealthPart; i++)
            {
                Instantiate(healthPartHalfPrefab, healthUIHolder.transform);
            }
            for (int i = 0; i < noneHealthPart; i++)
            {
                Instantiate(healthPartNonePrefab, healthUIHolder.transform);
            }
        }




    }
    public void GameOver() {
        SceneManager.LoadScene(levelFailScene);
    }

    public void ChangeHP(int change) {

        
        currentHealth += change;
        if (currentHealth <= 0) {
            GameOver();
        }


        foreach (Transform child in healthUIHolder.transform)
        {
              Destroy(child.gameObject);
        }


        int numHit = maxHealth - currentHealth;
        int fullHealthPart = 0;
        int halfHealthPart = 0;
        int noneHealthPart = 0;
        if (currentHealth > maxHealth / 2)
        {
            fullHealthPart = (maxHealth / 2) - numHit;
            halfHealthPart = (maxHealth / 2) - fullHealthPart;
            for (int i = 0; i < fullHealthPart; i++)
            {
                Instantiate(healthPartPrefab, healthUIHolder.transform);
            }

            for (int i = 0; i < halfHealthPart; i++)
            {
                Instantiate(healthPartHalfPrefab, healthUIHolder.transform);
            }
        }
        else
        {
            noneHealthPart = (maxHealth / 2) - currentHealth;
            halfHealthPart = currentHealth;
            for (int i = 0; i < halfHealthPart; i++)
            {
                Instantiate(healthPartHalfPrefab, healthUIHolder.transform);
            }
            for (int i = 0; i < noneHealthPart; i++)
            {
                Instantiate(healthPartNonePrefab, healthUIHolder.transform);
            }
        }

    }



    IEnumerator DestroyNew(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }



    void Start()
    {
        ChangeHP(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
