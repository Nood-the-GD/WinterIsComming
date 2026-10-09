using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Fox : MonoBehaviour
{
    private enum FoxStage
    {
        Walk,
        Run,
        ChaseTarget,
        LooseTarget
    }

    [SerializeField] private FieldOfView _fov;
    [SerializeField] private LayerMask _blockLayerMask;
    [SerializeField] private Vector2 _limitX, _limitY;
    [SerializeField] private Transform _questionMark;
    private Rigidbody2D _rb;
    private Vector3 _moveTargetPoint;
    private Transform _targetTransform;
    private float _walkSpeed = 1f;
    private float _runSpeed = 2f;
    private float _currentSpeed;
    private Color _rayColor;
    private FoxStage _currentStage = FoxStage.Walk;
    private bool _canMove = true;

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
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnOutOfTime>(Handler_OnOutOfTime);
        MessageBus.Subscribe<OnGameWin>(Handler_OnGameWin);
        _fov.OnTargetDetected += Handler_OnTargetDetected;
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Unsubscribe<OnOutOfTime>(Handler_OnOutOfTime);
        MessageBus.Unsubscribe<OnGameWin>(Handler_OnGameWin);
    }

    private void Handler_OnGameWin(OnGameWin win)
    {
        _canMove = false;
    }

    private void Handler_OnOutOfTime(OnOutOfTime time)
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
            _rb.linearVelocity = Vector3.zero;
            return;
        }

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
                _questionMark.gameObject.SetActive(false);
                break;
            case FoxStage.Run:
            case FoxStage.ChaseTarget:
                _currentSpeed = _runSpeed;
                _questionMark.gameObject.SetActive(false);
                break;
            case FoxStage.LooseTarget:
                // Stop a while then find another target
                _currentSpeed = 0;
                _rb.linearVelocity = Vector3.zero;
                _questionMark.gameObject.SetActive(true);
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

        if (_targetTransform != null && _currentStage == FoxStage.ChaseTarget)
        {
            if (Vector3.Distance(_targetTransform.position, this.transform.position) <= 0.2f)
            {
                MessageBus.Publish<OnFoxBiteSquirrel>(new());
            }
        }

        if (Vector3.Distance(_moveTargetPoint, this.transform.position) < 0.2f)
        {
            return false;
        }

        var distance = Vector3.Distance(_moveTargetPoint, this.transform.position);
        if (Physics2D.Raycast(origin: this.transform.position, direction: (_moveTargetPoint - transform.position).normalized, distance, layerMask: _blockLayerMask))
        {
            _rayColor = Color.red;
            if (_currentStage == FoxStage.ChaseTarget)
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
        if (transform == null)
        {
            if (_currentStage == FoxStage.ChaseTarget)
            {
                _currentStage = FoxStage.LooseTarget;
                MessageBus.Publish<OnFoxLooseSquirrel>(new());
            }
            return;
        }

        _targetTransform = transform;
        _moveTargetPoint = transform.position;
        _currentSpeed = _runSpeed;
        _currentStage = FoxStage.ChaseTarget;
        MessageBus.Publish<OnFoxSeeSquirrel>(new());
    }
}
