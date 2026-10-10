using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GlobalSetting _globalSetting;
    private float _limitTimeInSecond => _setting.MaxTimeInSecond;
    private int _requireAcorn => _setting.RequireAcorn;
    private float _timer;
    private int _currentAcorn;
    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;
    private bool _isGameComplete;


    void Start()
    {
        _timer = _limitTimeInSecond;
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnAcornUnload>(Handler_OnAcornUnload);
        MessageBus.Subscribe<OnOutOfWarmth>(Handler_OutOfWarm);
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Unsubscribe<OnAcornUnload>(Handler_OnAcornUnload);
        MessageBus.Unsubscribe<OnOutOfWarmth>(Handler_OutOfWarm);
    }

    private void Handler_OutOfWarm(OnOutOfWarmth warmth)
    {
        Debug.Log("Game Loose");
        _isGameComplete = true;
    }

    private void Handler_OnAcornUnload(OnAcornUnload unload)
    {
        _currentAcorn += unload.AcornNumber;
        if(_currentAcorn >= _setting.RequireAcorn)
        {
            // Game Win
            Debug.Log("Game Win");
            MessageBus.Publish<OnGameWin>(new());
            _isGameComplete = true;
        }
    }

    void Update()
    {
        if (_isGameComplete) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            // Out of time
            _isGameComplete = true;
            MessageBus.Publish<OnOutOfTime>(new());
            // Game Loose
            Debug.Log("Game Loose");
        }
    }

    private void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        // Game loose
        _isGameComplete = true;
        Debug.Log("Game Loose");
    }
}
