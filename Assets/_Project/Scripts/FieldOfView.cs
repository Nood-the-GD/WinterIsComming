using UnityEngine;
using UnityEngine.Rendering;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask _blockLayerMask;

    private float _fov = 90f;
    private Vector3 _origin = Vector3.zero;
    private float _angle = 0;
    private float _startingAngle = 0;
    private int _rayCount = 15;
    private float _angleIncrease;
    private float _viewDistance = 5f;
    private Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        _angleIncrease = _fov / _rayCount;
    }

    private void LateUpdate()
    {
        _angle = _startingAngle;
        vertices = new Vector3[_rayCount + 1 + 1];
        uv = new Vector2[vertices.Length];
        triangles = new int[_rayCount * 3];

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= _rayCount; i++)
        {
            var direction = VectorUtil.GetVectorFromAngle(_angle);
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, direction, _viewDistance, _blockLayerMask);
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = _origin + direction * _viewDistance;
            }
            else
            {
                // Hit
                vertex = raycastHit2D.point;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            _angle -= _angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        this.transform.position = Vector3.zero;
    }

    public void SetOrigin(Vector3 origin)
    {
        _origin = origin;
    }

    public void SetDirection(Vector3 direction)
    {
        _startingAngle = VectorUtil.GetAngleFromVectorFloat(direction) + _fov / 2f;
    }

}
