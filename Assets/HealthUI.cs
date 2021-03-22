using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class HealthUI : MonoBehaviour
{
    

    public int maxHealth = 40;
    public int currentHealth = 20;
    public GameObject healthPartPrefab;
    public GameObject healthUIRoot;
    public GameObject healthUIHolder;

    public float healthSpacing = 3f;


    void ValidateFunction() {


        if (currentHealth < 0) currentHealth = 0;
        foreach (Transform child in healthUIHolder.transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(child.gameObject);
            };
        }


        for(int i=0; i < currentHealth; i++) {
            GameObject healthPartInstance = Instantiate(healthPartPrefab);
            healthPartInstance.transform.SetParent(healthUIHolder.transform, false);
      
        }

      
    }



    IEnumerator DestroyNew(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    private void OnValidate()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode) {
            ValidateFunction();
        }
       
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
