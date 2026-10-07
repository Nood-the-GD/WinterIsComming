using System;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private TextMeshProUGUI _text;
    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;
    private float _time;
    private bool _timeStop;

    void Start()
    {
        _time = _setting.MaxTimeInSecond;
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnGameWin>(Handler_OnGameWin);
    }

    private void Handler_OnGameWin(OnGameWin win)
    {
        _timeStop = true;
    }

    private void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        _timeStop = true;
    }

    void Update()
    {
        if (_timeStop) return;

        _time -= Time.deltaTime;
        if (_time <= 0) return;

        _text.text = TimeSpan.FromSeconds(_time).ToString("mm':'ss");
    }
}
