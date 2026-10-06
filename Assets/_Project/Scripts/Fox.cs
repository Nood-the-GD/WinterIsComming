using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Fox : MonoBehaviour
{
    private enum FoxStage
    {
        Walk,
        Run,
        SeeTarget,
        LooseTarget
    }

    [SerializeField] private FieldOfView _fov;
    [SerializeField] private LayerMask _blockLayerMask;
    [SerializeField] private Vector2 _limitX, _limitY;
    private Rigidbody2D _rb;
    private Vector3 _moveTargetPoint;
    private float _walkSpeed = 1f;
    private float _runSpeed = 2f;
    private float _currentSpeed;
    private Color _rayColor;
    private FoxStage _currentStage = FoxStage.Walk;

    #region LooseTarget
    private float _looseTargetCountTime = 3;
    private float _looseTargetTimer = 0;
    #endregion

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentSpeed = _walkSpeed;
    }

    void Start()
    {
        _fov.OnTargetDetected += Handler_OnTargetDetected;
    }

    void Update()
    {
        if (_looseTargetTimer > 0)
        {
            _looseTargetTimer -= Time.deltaTime;
            if (_looseTargetTimer <= 0)
            {
                _currentStage = FoxStage.Walk;
                FindNextTarget();
            }
            return;
        }

        _fov.SetOrigin(this.transform.position);
        switch (_currentStage)
        {
            case FoxStage.Walk:
                _currentSpeed = _walkSpeed;
                break;
            case FoxStage.Run:
            case FoxStage.SeeTarget:
                _currentSpeed = _runSpeed;
                break;
            case FoxStage.LooseTarget:
                // Stop a while then find another target
                _currentSpeed = 0;
                _rb.linearVelocity = Vector3.zero;
                LooseTargetCountDown();
                break;
        }
        if (IsMoveValid())
        {
            var direction = (_moveTargetPoint - this.transform.position).normalized;
            _rb.linearVelocity = direction * _currentSpeed;
            _fov.SetDirection(direction);
            Debug.DrawLine(this.transform.position, _moveTargetPoint, color: _rayColor);
            Debug.DrawRay(this.transform.position, direction, Color.red);
        }
        else
        {
            FindNextTarget();
        }
    }

    private void FindNextTarget()
    {
        _moveTargetPoint = VectorUtil.GetRandomPointAroundCircle(origin: this.transform.position, 5);
    }

    private bool IsMoveValid()
    {
        if (_moveTargetPoint.x < _limitX.x || _moveTargetPoint.x > _limitX.y || _moveTargetPoint.y < _limitY.x || _moveTargetPoint.y > _limitY.y)
        {
            // Out of zone
            return false;
        }

        if (Vector3.Distance(_moveTargetPoint, this.transform.position) < 0.2f)
        {
            return false;
        }
        var distance = Vector3.Distance(_moveTargetPoint, this.transform.position);
        if (Physics2D.Raycast(origin: this.transform.position, direction: (_moveTargetPoint - transform.position).normalized, distance, layerMask: _blockLayerMask))
        {
            _rayColor = Color.red;
            if (_currentStage == FoxStage.SeeTarget)
            {
                _currentStage = FoxStage.LooseTarget;
            }
            return false;
        }
        _rayColor = Color.green;
        return true;
    }

    private void LooseTargetCountDown()
    {
        _looseTargetTimer = _looseTargetCountTime;
    }

    private void Handler_OnTargetDetected(Transform transform)
    {
        _moveTargetPoint = transform.position;
        _currentSpeed = _runSpeed;
        _currentStage = FoxStage.SeeTarget;
    }
}
