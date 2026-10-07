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


    void Start()
    {
        _timer = _limitTimeInSecond;
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnAcornUnload>(Handler_OnAcornUnload);
    }

    private void Handler_OnAcornUnload(OnAcornUnload unload)
    {
        _currentAcorn += unload.AcornNumber;
        if(_currentAcorn >= _setting.RequireAcorn)
        {
            // Game Win
            Debug.Log("Game Win");
            MessageBus.Publish<OnGameWin>(new());
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            // Out of time
            MessageBus.Publish<OnOutOfTime>(new());
            // Game Loose
            Debug.Log("Game Loose");
        }
    }

    private void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        // Game loose
        Debug.Log("Game Loose");
    }
}
