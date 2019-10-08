using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickInput : MonoBehaviour
{
    public delegate void GameInput ();
    public static event GameInput OnFlick;

    public static bool IS_HOLDING;

    [Tooltip("Time in seconds Down and Up has to get called")]
    public float swipeCounterThreshold = .5f;
    [Tooltip("Magnitude of the flick")]
    public float swipeThreshold = .1f;
    private Vector3 _pointerDown;
    private Vector3 _pointerCurrent;
    private Vector3 _pointerUp;
    private float _flickTimer;
    private float _holdTimer;

    void Start()
    {
        IS_HOLDING = false;
    }

    void Update ()
    {
        if (GameController.Instance.Player.isAlive == false) { return; }
        if (Input.GetMouseButtonDown (0))
        {
            _pointerDown = Camera.main.ScreenToViewportPoint (Input.mousePosition);
        }

        if (Input.GetMouseButton (0))
        {
            _pointerCurrent = Camera.main.ScreenToViewportPoint (Input.mousePosition);
            _flickTimer += Time.deltaTime;
            _holdTimer += Time.deltaTime;
            if (_holdTimer > .05f)
            {
                IS_HOLDING = true;
            }
        }

        if (Input.GetMouseButtonUp (0))
        {
            _pointerUp = Camera.main.ScreenToViewportPoint (Input.mousePosition);
            IS_HOLDING = false;
            _holdTimer = 0f;
            Vector3 _delta = _pointerUp - _pointerDown;
            if (_delta.magnitude > swipeThreshold)
            {

                if (_delta.y > 0f && Mathf.Abs (_delta.y) > Mathf.Abs (_delta.x * .35f))
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