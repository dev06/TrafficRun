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

    private SectionController _sectionController;
    private bool _flicked;
    private float _flickTimer;
    private int _flickCount;
    void Start ()
    {
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
        FlickInput.OnDown += OnDown;
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
        if(GameController.Instance.state != State.Game)
        {
            GameController.Instance.SetState(State.Game); 
        }
        CameraController.Instance.SetPosition (-Vector3.forward);
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
                    fx_explosion.Play ();
                    StartCoroutine ("IRestart");
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

        if (FlickInput.IS_HOLDING)
        {
            _sectionController.Velocity += Time.deltaTime * (FlickInput.IS_HOLDING ? onHoldBrakePower : frictionBrake);
        }
        else
        {
            _sectionController.Velocity -= Time.deltaTime * (frictionBrake);
        }

        // if (FlickInput.IS_HOLDING || !_flicked)
        // {
        //     _sectionController.Velocity -= Time.deltaTime * (FlickInput.IS_HOLDING ? onHoldBrakePower : frictionBrake);
        // }
        _sectionController.Velocity = Mathf.Clamp (_sectionController.velocity, 0f, maxVelocity);
    }

    IEnumerator IBrake ()
    {
        while (_sectionController.Velocity > 0)
        {
            _sectionController.Velocity -= Time.deltaTime * onHoldBrakePower;
            yield return null;
        }
    }
}