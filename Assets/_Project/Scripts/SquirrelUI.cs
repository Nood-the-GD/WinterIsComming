using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class SquirrelUI : MonoBehaviour
{

    [SerializeField] private TextMeshPro _acornText;

    [SerializeField] private Squirrel _squirrel;

    void Start()
    {
        _squirrel.OnCollectAcorn += Handle_OnAcornCollect;        
        _acornText.text = $"{_squirrel.CurrentAcorn}/{_squirrel.MaxAcorn}";
    }

    private void Handle_OnAcornCollect()
    {
        _acornText.text = $"{_squirrel.CurrentAcorn}/{_squirrel.MaxAcorn}";
    }
}
