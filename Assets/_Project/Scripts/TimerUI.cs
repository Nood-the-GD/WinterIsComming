using System;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private TextMeshProUGUI _text;
    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;
    private float _time;

    void Start()
    {
        _time = _setting.MaxTimeInSecond;
    }

    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0) return;

        _text.text = TimeSpan.FromSeconds(_time).ToString("mm':'ss");
    }
}
