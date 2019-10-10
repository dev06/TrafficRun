using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxVelocity = 100;
    public float onHoldBrakePower = 800;
    public float frictionBrake = 100;
    public ParticleSystem fx_explosion;
    public bool isAlive = true;

    [Header("Fields")]
    public ProgressMeter furyMeter;

    private SectionController _sectionController;
    private bool _flicked, _furyAchieved, _inProtection;
    private float _flickTimer;
    private int _flickCount;

    private float _fury;
    private float _furyDepletionRate;
    void Start ()
    {
        FuryAchieved = false;
        _sectionController = SectionController.Instance;
    }

    void OnEnable ()
    {
        FlickInput.OnFlick += OnFlick;
        FlickInput.OnDown += OnDown;
        EventManager.OnSectionTriggerHit += OnSectionTriggerHit;
        EventManager.OnGameEvent += OnGameEvent;
    }

    void OnDisable ()
    {
        FlickInput.OnFlick -= OnFlick;
        FlickInput.OnDown -= OnDown;
        EventManager.OnSectionTriggerHit -= OnSectionTriggerHit;
        EventManager.OnGameEvent -= OnGameEvent;
    }

    void OnFlick ()
    {
        _sectionController.Velocity += maxVelocity;
        _flicked = true;
        _flickCount++;
    }

    void OnDown ()
    {
        if (GameController.Instance.state != State.Game)
        {
            GameController.Instance.SetState(State.Game);
        }

        if (!FuryAchieved)
        {
            CameraController.Instance.SetPosition (-Vector3.forward);
        }
    }

    void OnSectionTriggerHit ()
    {
        //StopCoroutine("IBrake");
        //StartCoroutine("IBrake");
        if (!_furyAchieved)
        {
            Fury += .08f;
        }
    }

    void OnGameEvent (EventID id)
    {
        switch (id)
        {
            case EventID.FINISH:
            {
                CameraController.Instance.Detach ();
                _sectionController.Velocity = 0;

                UIController.Instance.ShowPage (PageType.Complete);
                StartCoroutine ("ITranslate");
                break;
            }
            case EventID.VEHICLE_HIT:
            {
                isAlive = false;
                _sectionController.Velocity = 0;
                Fury = 0;
                fx_explosion.Play ();
                StartCoroutine ("IRestart");
                break;
            }

            case EventID.FURY_START:
            {
                CameraController.Instance.SetPosition(-Vector3.forward);
                break;
            }

            case EventID.FURY_END:
            {
                break;
            }
        }
    }

    IEnumerator IRestart ()
    {
        yield return new WaitForSecondsRealtime (4f);
        UnityEngine.SceneManagement.SceneManager.LoadScene (0);

    }

    IEnumerator ITranslate ()
    {
        if (EventManager.OnComplete != null)
        {
            EventManager.OnComplete ("level", LevelController.Instance.Level, LevelController.Instance.Zone);
        }
        float timer = 0f;
        while (timer < 3f)
        {
            transform.Translate (Vector3.forward * Time.deltaTime * maxVelocity, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }

        // UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }

    void Update ()
    {
        if (!isAlive) { return; }
        if (_flicked)
        {
            _flickTimer += Time.deltaTime;
            if (_flickTimer > 2f)
            {
                _flickTimer = 0;
                _flicked = false;
                _flickCount = 0;
            }
        }

        float _mult = .15f;
        Fury -= Time.deltaTime * _mult;


        if (!_furyAchieved)
        {
            if (FlickInput.IS_HOLDING)
            {
                _sectionController.Velocity += Time.deltaTime * (FlickInput.IS_HOLDING ? onHoldBrakePower : frictionBrake);
            }
            else
            {
                _sectionController.Velocity -= Time.deltaTime * (frictionBrake);
            }
            _sectionController.Velocity = Mathf.Clamp (_sectionController.velocity, 0f, maxVelocity);
        } else
        {
            _sectionController.Velocity += Time.deltaTime *  onHoldBrakePower;
            _sectionController.Velocity = Mathf.Clamp (_sectionController.velocity, 0f, maxVelocity * 1.5f);
        }



        if (Input.GetKeyDown(KeyCode.F))
        {
            Fury = 1f;
        }

        furyMeter.fill = Fury;
    }

    IEnumerator IBrake ()
    {
        while (_sectionController.Velocity > 0)
        {
            _sectionController.Velocity -= Time.deltaTime * onHoldBrakePower;
            yield return null;
        }
    }

    public float Fury
    {
        get
        {
            return _fury;
        }
        set {
            _fury = value;
            _fury = Mathf.Clamp(_fury, 0f, 1f);
            furyMeter.fill = _fury;
            if (_fury <= 0f && FuryAchieved)
            {
                if (EventManager.OnGameEvent != null)
                {
                    EventManager.OnGameEvent(EventID.FURY_END);
                }
                StartCoroutine("IEnterProtection");
                FuryAchieved = false;
            }

            if (_fury >= .99f && !FuryAchieved)
            {
                if (EventManager.OnGameEvent != null)
                {
                    EventManager.OnGameEvent(EventID.FURY_START);
                }
                EnableProtection = true;
                FuryAchieved = true;
            }
        }
    }

    IEnumerator IEnterProtection()
    {
        yield return new WaitForSecondsRealtime(2f);
        EnableProtection = false;
        if (EventManager.OnGameEvent != null)
        {
            EventManager.OnGameEvent(EventID.PROTECTION_END);
        }
        StopCoroutine("IEnterProtection");
    }

    public bool EnableProtection
    {
        get {return _inProtection;}
        set {_inProtection = value; }
    }

    public bool FuryAchieved
    {
        get {return _furyAchieved; }
        set {_furyAchieved = value; }
    }
}