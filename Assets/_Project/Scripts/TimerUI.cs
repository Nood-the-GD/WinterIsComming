using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private MMF_Player _outOfTimeFeedback;
    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private TextMeshProUGUI _text;
    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;
    private float _time;
    private bool _timeStop;
    private bool _isFeedbackTriggered;

    void Start()
    {
        _time = _setting.MaxTimeInSecond;
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnGameWin>(Handler_OnGameWin);
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Unsubscribe<OnGameWin>(Handler_OnGameWin);
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
        if (_timeStop)
        {
            _outOfTimeFeedback.StopFeedbacks();
            return;
        }

        if (_time <= 10 && _isFeedbackTriggered == false)
        {
            _outOfTimeFeedback.PlayFeedbacks();
            _isFeedbackTriggered = true;
        }
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _outOfTimeFeedback.StopFeedbacks();
        }

        _text.text = TimeSpan.FromSeconds(_time).ToString("mm':'ss");
    }
}
