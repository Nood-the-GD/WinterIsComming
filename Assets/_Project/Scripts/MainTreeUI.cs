using System;
using Core.SoundManager;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class MainTreeUI : MonoBehaviour
{
    [SerializeField] private MMF_Player _unloadAcornFeedback;
    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private TextMeshPro _text;
    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;
    private int _currentAcorn;

    void Start()
    {
        MessageBus.Subscribe<OnAcornUnload>(Handler_OnAcornUnload);
        _currentAcorn = 0;
        _text.text = $"{_currentAcorn}/{_setting.RequireAcorn}";
    }

    private void Handler_OnAcornUnload(OnAcornUnload unload)
    {
        if (unload.AcornNumber <= 0) return;
        ServiceManager.Get<SoundManager>().PlaySound(SoundEnum.Unload_acorn);
        _unloadAcornFeedback.PlayFeedbacks();
        _currentAcorn += unload.AcornNumber;
        _text.text = $"{_currentAcorn}/{_setting.RequireAcorn}";
    }
}
