using System;
using Core.SoundManager;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Squirrel : MonoBehaviour
{
    [SerializeField] private MMF_Player _collectAcornFeedback;
    [SerializeField] private MMF_Player _fullAcornFeedback;

    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private InputActionReference _move;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5;
    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;

    public Action OnCollectAcorn;

    private Vector3 _moveDirection;
    private int _currentAcorn;
    private int _maxAcorn => _setting.MaxAcornCanCarry;
    private bool _canMove = true;

    public int CurrentAcorn => _currentAcorn;
    public int MaxAcorn => _maxAcorn;

    void Start()
    {
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnGameWin>(Handler_OnGameWin);
        MessageBus.Subscribe<OnOutOfTime>(Handler_OnOutOfTime);
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Unsubscribe<OnGameWin>(Handler_OnGameWin);
        MessageBus.Unsubscribe<OnOutOfTime>(Handler_OnOutOfTime);
    }

    private void Handler_OnOutOfTime(OnOutOfTime time)
    {
        _canMove = false;
    }

    private void Handler_OnGameWin(OnGameWin win)
    {
        _canMove = false;
    }

    private void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        _canMove = false;
    }

    void Update()
    {
        if (_canMove == false)
        {
            _moveDirection = Vector2.zero;
            return;
        }
        _moveDirection = _move.action.ReadValue<Vector2>();
        _moveDirection = Vector3.Normalize(_moveDirection);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Acorn")
        {
            if (_currentAcorn >= _maxAcorn)
            {
                _fullAcornFeedback.PlayFeedbacks();
                ServiceManager.Get<SoundManager>().PlaySound(SoundListEnum.Collect_acorn_error);
                return;
            }
            _collectAcornFeedback.PlayFeedbacks();
            _currentAcorn++;
            ServiceManager.Get<SoundManager>().PlaySound(SoundListEnum.Collect_acorn);
            if (collision.TryGetComponent<Acorn>(out Acorn acorn))
            {
                acorn.OnAcornCollect?.Invoke();
            }
            Destroy(collision.gameObject);
            OnCollectAcorn?.Invoke();
        }

        if (collision.gameObject.tag == "MainTree")
        {
            MessageBus.Publish<OnAcornUnload>(new OnAcornUnload { AcornNumber = CurrentAcorn });
            _currentAcorn = 0;
            OnCollectAcorn?.Invoke();
        }
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _moveDirection * 5;
    }
}
