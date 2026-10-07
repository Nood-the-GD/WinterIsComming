using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Extension;
using DG.Tweening;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject _acornPref;
    [SerializeField] private TreeDataSo _data;
    [SerializeField] private Transform[] _spawnPosition;
    private List<int> _occupiedPosition = new();

    private float _timer = 0;
    private int _currentAcorn = 0;

    void Start()
    {
        _timer = _data.SpawnAcornInterval;
    }

    void Update()
    {
        if (_currentAcorn >= _data.MaxAcorn) return;

        _timer += Time.deltaTime;

        if (_timer >= _data.SpawnAcornInterval)
        {
            AcornSpawnAnim();
            _timer = 0;
        }
    }
    
    private void AcornSpawnAnim()
    {
        _currentAcorn++;

        int index = -1;
        var spawnPos = _spawnPosition.GetRandom(exceptIndexes: _occupiedPosition.ToArray(), out index);
        _occupiedPosition.Add(index);
        var newAcorn = Instantiate(_acornPref, this.transform);
        newAcorn.transform.position = spawnPos.position;
        newAcorn.transform.localScale = Vector3.zero;
        newAcorn.GetComponent<Acorn>().OnAcornCollect = () =>
        {
            _currentAcorn--;
        };

        Sequence acornSequence = DOTween.Sequence();
        acornSequence.Append(newAcorn.transform.DOScale(1, 1));
        acornSequence.AppendInterval(1);
        acornSequence.Append(newAcorn.transform.DOLocalMoveY(endValue: 0, duration: 1f).SetEase(Ease.OutBounce));
        acornSequence.OnComplete(() =>
        {
            _occupiedPosition.Remove(index);
        });

        acornSequence.Play();
    }
}











