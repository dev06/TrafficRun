using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 40; 
    void OnEnable ()
    {
        FlickInput.OnFlick += OnFlick;
    }

    void OnDisable ()
    {
        FlickInput.OnFlick -= OnFlick;
    }
    void OnFlick ()
    {
        Debug.Log ("Flick");
    }

    // Update is called once per frame
    void Update ()
    {
      // transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World); 
    }
}