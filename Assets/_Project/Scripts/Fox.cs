using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fox : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private LayerMask _blockLayerMask;
    [SerializeField] private Vector2 _limitX, _limitY;
    private Rigidbody2D _rb;
    private Vector3 _moveTargetPoint;
    private float _walkSpeed = 2f;
    private float _runSpeed = 6f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    void Update()
    {
        _fov.SetOrigin(this.transform.position);
        if (IsMoveValid())
        {
            var direction = (_moveTargetPoint - this.transform.position).normalized;
            _rb.linearVelocity = direction * _walkSpeed;
            _fov.SetDirection(direction);
            Debug.DrawLine(this.transform.position, _moveTargetPoint);
            Debug.DrawRay(this.transform.position, direction, Color.red);
            if (IsSeeSquirrel())
            {
                _rb.linearVelocity = direction * _runSpeed;
            }
        }
        else
        {
            _moveTargetPoint = VectorUtil.GetRandomPointAroundCircle(origin: this.transform.position, 5);
        }
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
        if(Physics2D.Raycast(origin: this.transform.position, direction: (_moveTargetPoint - transform.position).normalized, distance: 1, layerMask: _blockLayerMask))
        {
            return false;
        }
        return true;
    }

    private bool IsSeeSquirrel()
    {
        return false;
    }
}
