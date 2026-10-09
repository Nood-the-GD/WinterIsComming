using Core.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private TMP_Dropdown _difficultyDropDown;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Button _backBtn;

    void Awake()
    {
        _backBtn.onClick.AddListener(Back);
        _musicToggle.onValueChanged.AddListener(Handler_OnMusicToggleChange);
        _soundToggle.onValueChanged.AddListener(Handler_OnSoundToggleChange);
        _difficultyDropDown.onValueChanged.AddListener(Handler_OnDifficultyChange);
    }

    private void Handler_OnDifficultyChange(int arg0)
    {
        switch(arg0)
        {
            case 0:
                _globalSetting.ChangeDifficulty(Difficulty.Easy);
                break;
            case 1:
                _globalSetting.ChangeDifficulty(Difficulty.Normal);
                break;
            case 2:
                _globalSetting.ChangeDifficulty(Difficulty.Hard);
                break;
            default:
                _globalSetting.ChangeDifficulty(Difficulty.Easy);
                break;
        }
    }

    private void Handler_OnSoundToggleChange(bool arg0)
    {
        if(arg0)
        {
            _globalSetting.IsSound = true;
        }
        else
        {
            _globalSetting.IsSound = false;
        }
        ServiceManager.Get<SoundManager>().SetGlobalSoundVolume(arg0 ? 1 : 0);
    }

    private void Handler_OnMusicToggleChange(bool arg0)
    {
        if (arg0)
        {
            _globalSetting.IsMusic = true;
        }
        else
        {
            _globalSetting.IsMusic = false;
        }

        ServiceManager.Get<SoundManager>().SetGlobalMusicVolume(arg0 ? 1 : 0);
    }

    void OnEnable()
    {
        _musicToggle.isOn = _globalSetting.IsMusic;
        _soundToggle.isOn = _globalSetting.IsSound;
        _difficultyDropDown.value = GetDifficultyValue(_globalSetting.CurrentDifficulty);
    }

    private void Back()
    {
        this.gameObject.SetActive(false);
    }

    private int GetDifficultyValue(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 0;
            case Difficulty.Normal:
                return 1;
            case Difficulty.Hard:
                return 2;
            default: 
                return 0;
        }
    }
}
