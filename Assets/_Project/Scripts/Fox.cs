using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fox : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private LayerMask _blockLayerMask;
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
        if(IsMoveValid())
        {
            var direction = (_moveTargetPoint - this.transform.position).normalized;
            _rb.linearVelocity = direction * _walkSpeed;
            _fov.SetDirection(direction);
            if(IsSeeSquirrel())
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
        if(Vector3.Distance(_moveTargetPoint, this.transform.position) < 0.2f)
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
