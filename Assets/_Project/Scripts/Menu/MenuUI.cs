using Core.SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button _playBtn, _settingBtn, _quitBtn;
    [SerializeField] private Transform _settingPanel;
    [SerializeField] private GlobalSetting _globalSetting;

    void Awake()
    {
        _playBtn.onClick.AddListener(Play);
        _settingBtn.onClick.AddListener(Setting);
        _quitBtn.onClick.AddListener(Quit);
    }

    void Start()
    {
        ServiceManager.Get<SoundManager>().SetGlobalMusicVolume(_globalSetting.IsMusic ? 1 : 0);
        ServiceManager.Get<SoundManager>().SetGlobalSoundVolume(_globalSetting.IsSound ? 1 : 0);
    }

    private void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
    private void Setting()
    {
        _settingPanel.gameObject.SetActive(true);
    }
    private void Quit()
    {
        Application.Quit();
    }
}
