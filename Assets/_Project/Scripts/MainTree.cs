using UnityEngine;

public class MainTree : MonoBehaviour
{
    [SerializeField] private Transform _offTrans;
    [SerializeField] private Transform _nestTrans;

    public Transform NestTrans => _nestTrans;
    public Transform OffTrans => _offTrans;
}
