using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickInput : MonoBehaviour
{
    public delegate void Flick ();
    public static event Flick OnFlick;

    [Tooltip("Time in seconds Down and Up has to get called")]
    public float swipeCounterThreshold = .5f; 
    [Tooltip("Magnitude of the flick")]
    public float swipeThreshold = .1f;
    private Vector3 _pointerDown;
    private Vector3 _pointerCurrent;
    private Vector3 _pointerUp;
    private float _flickTimer;

    void Update ()
    {
        if (Input.GetMouseButtonDown (0))
        {
            _pointerDown = Camera.main.ScreenToViewportPoint (Input.mousePosition);
        }

        if (Input.GetMouseButton (0))
        {
            _pointerCurrent = Camera.main.ScreenToViewportPoint (Input.mousePosition);
            _flickTimer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp (0))
        {
            _pointerUp = Camera.main.ScreenToViewportPoint (Input.mousePosition);

            Vector3 _delta = _pointerUp - _pointerDown;
            if (_delta.magnitude > swipeThreshold)
            {
                if (_delta.y > 0f && Mathf.Abs (_delta.y) > Mathf.Abs (_delta.x) && _flickTimer < swipeCounterThreshold)
                {
                    if (OnFlick != null)
                    {
                        OnFlick ();
                    }
                }
            }
            _flickTimer = 0f;
        }
    }
}