using System;
using TMPro;
using UnityEngine;

public class MainTreeUI : MonoBehaviour
{
    [SerializeField] private GameSetting _setting;
    [SerializeField] private TextMeshPro _text;
    private int _currentAcorn;

    void Start()
    {
        MessageBus.Subscribe<OnAcornUnload>(Handler_OnAcornUnload);
        _currentAcorn = 0;
        _text.text = $"{_currentAcorn}/{_setting.RequireAcorn}";
    }

    private void Handler_OnAcornUnload(OnAcornUnload unload)
    {
        _currentAcorn += unload.AcornNumber;
        _text.text = $"{_currentAcorn}/{_setting.RequireAcorn}";
    }
}
