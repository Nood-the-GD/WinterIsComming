using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private Transform _winTextPanel, _looseTextPanel;
    [SerializeField] private TextMeshProUGUI _winText, _looseText;
    [SerializeField] private Button _retryBtn, _menuBtn;

    void Awake()
    {
        _retryBtn.onClick.AddListener(Handler_OnRetryClick);
        _menuBtn.onClick.AddListener(Handler_OnMenuClick);
    }

    void Start()
    {
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Subscribe<OnOutOfTime>(Handler_OnOutOfTime);
        MessageBus.Subscribe<OnGameWin>(Handler_OnGameWin);
        MessageBus.Subscribe<OnOutOfWarmth>(Handler_OnOutOfWarmth);
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
        MessageBus.Unsubscribe<OnOutOfTime>(Handler_OnOutOfTime);
        MessageBus.Unsubscribe<OnGameWin>(Handler_OnGameWin);
        MessageBus.Unsubscribe<OnOutOfWarmth>(Handler_OnOutOfWarmth);
    }

    private void Handler_OnOutOfWarmth(OnOutOfWarmth warmth)
    {
        ShowLoose(false);
    }

    private void Handler_OnGameWin(OnGameWin win)
    {
        ShowWin();
    }

    private void Handler_OnOutOfTime(OnOutOfTime time)
    {
        ShowLoose(false);
    }

    private async void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        await UniTask.WaitForSeconds(0.2f);
        ShowLoose(true);
    }

    private void Handler_OnMenuClick()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void Handler_OnRetryClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ShowWin()
    {
        _winTextPanel.gameObject.SetActive(true);
        _looseTextPanel.gameObject.SetActive(false);
        _retryBtn.gameObject.SetActive(true);
        _menuBtn.gameObject.SetActive(true);
    }
    private void ShowLoose(bool isBite)
    {
        if (isBite)
        {
            _looseText.text = "Squirrel is delicious !!!";
        }
        else
        {
            _looseText.text = "Squirrel die cold !!!";
        }

        _winTextPanel.gameObject.SetActive(false);
        _looseTextPanel.gameObject.SetActive(true);

        _retryBtn.gameObject.SetActive(true);
        _menuBtn.gameObject.SetActive(true);
    }
}
