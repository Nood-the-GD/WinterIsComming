using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button _playBtn, _settingBtn, _quitBtn;
    [SerializeField] private Transform _settingPanel;

    void Awake()
    {
        _playBtn.onClick.AddListener(Play);
        _settingBtn.onClick.AddListener(Setting);
        _quitBtn.onClick.AddListener(Quit);
    }

    private void Play()
    {
        SceneManager.LoadScene(0);
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
