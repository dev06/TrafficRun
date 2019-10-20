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
    public Section currentSection;
    public PurchaseableVehicle[] playerModels;
    public GameObject gameCamera;

    [Header("Fields")]
    public ProgressMeter furyMeter;

    private SectionController _sectionController;
    private bool _flicked, _furyAchieved, _inProtection, _isFinished;
    private float _flickTimer;
    private int _flickCount;
    private int _zone;
    private float _initialVelocity;

    private float _fury;

    private Vector3 _defaultPos;
    private Quaternion _defaultRot;

    void Awake()
    {
        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i].Init();
        }

        int activeVehicleId = PlayerPrefs.HasKey("ACTIVE_VEHICLE_ID") ? PlayerPrefs.GetInt("ACTIVE_VEHICLE_ID") : 0;
        if (!playerModels[activeVehicleId].IsPurchased)
        {
            activeVehicleId = 0;
        }
        PurchaseableVehicle.SetActiveVehicle(playerModels[activeVehicleId]);
    }
    void Start ()
    {
        FuryAchieved = false;
        _sectionController = SectionController.Instance;
        _zone = LevelController.Instance.Zone;
        _initialVelocity = _zone == 4 ? 175f : 100f;
        _defaultPos = transform.position;
        _defaultRot = transform.rotation;




        switchVehicleModel();
        playerModels[PurchaseableVehicle.active.GetIndex()].transform.GetChild(0).GetComponent<PlayerModel>().TranslateVehicle();

    }

    void OnEnable ()
    {
        FlickInput.OnDown += OnDown;
        EventManager.OnSectionTriggerHit += OnSectionTriggerHit;
        EventManager.OnGameEvent += OnGameEvent;
        FlickInput.OnUp += OnUp;
        EventManager.OnVehicleActive += OnVehicleActive;
        EventManager.OnStateChange += OnStateChange;
    }

    void OnDisable ()
    {

        FlickInput.OnDown -= OnDown;
        EventManager.OnSectionTriggerHit -= OnSectionTriggerHit;
        EventManager.OnGameEvent -= OnGameEvent;
        FlickInput.OnUp -= OnUp;
        EventManager.OnVehicleActive -= OnVehicleActive;
        EventManager.OnStateChange -= OnStateChange;
    }

    void OnUp()
    {
        if (_sectionController.Velocity >= maxVelocity && !FuryAchieved)
        {
            AudioController.Instance.Play(SFX.CAR_BRAKE);
            PurchaseableVehicle.active.FX_Trail.Activate(transform.GetChild(PurchaseableVehicle.active.GetIndex()).GetChild(0).transform.position);
        }
    }

    void OnDown ()
    {
        if (_isFinished) {
            return;
        }
        // if (GameController.Instance.state != State.Game)
        // {
        //     GameController.Instance.SetState(State.Game);
        // }

        if (!FuryAchieved)
        {
            CameraController.Instance.SetPosition (-Vector3.forward);
        }
    }

    void OnVehicleActive(PurchaseableVehicle v)
    {
        switchVehicleModel();
        switch (v.ID)
        {
            case ObjectID.Cop:
            {
                break;
            }

            case ObjectID.GarbageTruck:
            {
                break;
            }
            case ObjectID.Ambo:
            {

                break;
            }
        }

    }

    void OnStateChange(State s)
    {
        if (s == State.Store)
        {
            gameCamera.SetActive(false);
            return;
        }

        gameCamera.SetActive(true);
        switch (s)
        {
            case State.Menu:
            {
                switchVehicleModel();
                playerModels[PurchaseableVehicle.active.GetIndex()].transform.GetChild(0).GetComponent<PlayerModel>().TranslateVehicle();
                break;
            }
        }
    }

    private void switchVehicleModel()
    {
        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i].transform.gameObject.SetActive(false);
        }

        playerModels[PurchaseableVehicle.active.GetIndex()].transform.gameObject.SetActive(true);
    }

    void OnSectionTriggerHit ()
    {
        //StopCoroutine("IBrake");
        //StartCoroutine("IBrake");
    }

    void OnGameEvent (EventID id)
    {
        switch (id)
        {
            case EventID.FINISH:
            {
                _isFinished = true;
                CameraController.Instance.Detach ();
                _sectionController.Velocity = 0;
                GameController.Instance.CalculateScore();
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
                CameraController.Instance.SetPosition(-Vector3.forward * 3f);
                Haptic.VibrateHandheld();
                break;
            }

            case EventID.FURY_END:
            {
                CameraController.Instance.SetPosition(Vector3.forward * 3f);
                break;
            }

            case EventID.RESTART:
            {
                StopCoroutine("ITranslate");
                isAlive = true;
                fx_explosion.Stop();
                transform.position = _defaultPos;
                transform.rotation = _defaultRot;
                _furyAchieved = false;
                _inProtection = false;
                _isFinished = false;
                Fury = 0;
                break;
            }
        }
    }

    IEnumerator IRestart ()
    {
        yield return new WaitForSecondsRealtime (4f);
        // UnityEngine.SceneManagement.SceneManager.LoadScene (0);
        GameController.Instance.Restart(true);
        // if (EventManager.OnGameEvent != null)
        // {
        //     EventManager.OnGameEvent(EventID.RESTART);
        // }
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
    }

    void Update ()
    {
        if (!isAlive || GameController.Instance.state != State.Game) { return; }
        maxVelocity = _initialVelocity * PurchaseableVehicle.active.SpeedMult;

        if ( PurchaseableVehicle.active.FX_Trail.CanSetParent && _sectionController.Velocity > 0)
        {
            PurchaseableVehicle.active.FX_Trail.transform.SetParent(currentSection.transform);
        }

        if (!_furyAchieved)
        {
            if (!_isFinished)
            {
                if (FlickInput.IS_HOLDING)
                {
                    _sectionController.Velocity += Time.deltaTime * (FlickInput.IS_HOLDING ? onHoldBrakePower : frictionBrake);
                    Fury += Time.deltaTime * .2f;
                }
                else
                {
                    _sectionController.Velocity -= Time.deltaTime * (frictionBrake);
                }
            }
            _sectionController.Velocity = Mathf.Clamp (_sectionController.velocity, 0f, maxVelocity);
        } else
        {
            if (!_isFinished)
            {
                _sectionController.Velocity += Time.deltaTime *  onHoldBrakePower;
                _sectionController.Velocity = Mathf.Clamp (_sectionController.velocity, 0f, maxVelocity * 1.5f);
            }
        }

        float _mult = .1f;
        Fury -= Time.deltaTime * _mult;

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