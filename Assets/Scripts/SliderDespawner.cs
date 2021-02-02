using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDespawner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check for slider layer
        if(collision.gameObject.layer == 6) {
            Destroy(collision.gameObject);
        }
    }
}
