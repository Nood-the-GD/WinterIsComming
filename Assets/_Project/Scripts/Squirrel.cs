using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Squirrel : MonoBehaviour
{
    [SerializeField] private GameSetting _setting;
    [SerializeField] private InputActionReference _move;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5;

    public Action OnCollectAcorn;

    private Vector3 _moveDirection;
    private int _currentAcorn;
    private int _maxAcorn => _setting.MaxAcornCanCarry;

    public int CurrentAcorn => _currentAcorn;
    public int MaxAcorn => _maxAcorn;

    void Update()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();
        _moveDirection = Vector3.Normalize(_moveDirection);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Acorn")
        {
            if (_currentAcorn >= _maxAcorn) return;
            _currentAcorn++;
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
