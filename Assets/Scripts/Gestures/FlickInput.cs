using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickInput : MonoBehaviour
{
    public delegate void GameInput ();
    public static event GameInput OnFlick;
    public static event GameInput OnDown;
    public static event GameInput OnUp;

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


    void OnEnable ()
    {
        EventManager.OnGameEvent += OnGameEvent;
    }

    void OnDisable ()
    {
        EventManager.OnGameEvent -= OnGameEvent;
    }


    void OnGameEvent(EventID id)
    {
        switch (id)
        {
            case EventID.RESTART:
            {
                IS_HOLDING = false;
                break;
            }
        }
    }
    void Start()
    {
        IS_HOLDING = false;
    }

    void Update ()
    {
        if (!GameController.Instance.Player.isAlive || GameController.Instance.state != State.Game) { return; }
        if (Input.GetMouseButtonDown (0))
        {
            _pointerDown = Camera.main.ScreenToViewportPoint (Input.mousePosition);
            IS_HOLDING = true;
            if (OnDown != null)
            {
                OnDown();
            }
        }

        if (Input.GetMouseButtonUp (0))
        {
            _pointerUp = Camera.main.ScreenToViewportPoint (Input.mousePosition);
            IS_HOLDING = false;
            _holdTimer = 0f;

            if (OnUp != null)
            {
                OnUp();
            }
        }
    }
}