using System;
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
    }

    private void Handler_OnGameWin(OnGameWin win)
    {
        ShowWin();
    }

    private void Handler_OnOutOfTime(OnOutOfTime time)
    {
        ShowLoose(false);
    }

    private void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        ShowLoose(true);
    }

    private void Handler_OnMenuClick()
    {
        SceneManager.LoadScene(1);
    }

    private void Handler_OnRetryClick()
    {
        SceneManager.LoadScene(0);
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
