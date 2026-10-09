using MoreMountains.Feedbacks;
using UnityEngine;

public class WarningHapticController : MonoBehaviour
{
    [SerializeField] private MMF_Player _warningHaptic;
    private bool _isHapticRunning;

    private void Start()
    {
        MessageBus.Subscribe<OnFoxSeeSquirrel>(Handler_OnFoxSeeSquirrel);
        MessageBus.Subscribe<OnFoxLooseSquirrel>(Handler_OnFoxLooseSquirrel);
        _warningHaptic.Events.OnComplete.AddListener(() => _isHapticRunning = false);
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxSeeSquirrel>(Handler_OnFoxSeeSquirrel);
        MessageBus.Unsubscribe<OnFoxLooseSquirrel>(Handler_OnFoxLooseSquirrel);
    }

    private async void Handler_OnFoxLooseSquirrel(OnFoxLooseSquirrel squirrel)
    {
        // _warningHaptic.PauseFeedbacks();
        var feedback = _warningHaptic.GetFeedbackOfType<MMF_Looper>();
        feedback.Active = false;
    }

    private void Handler_OnFoxSeeSquirrel(OnFoxSeeSquirrel squirrel)
    {
        if (_isHapticRunning) return;
        var feedback = _warningHaptic.GetFeedbackOfType<MMF_Looper>();
        feedback.Active = true;
        _warningHaptic.PlayFeedbacks();
        _isHapticRunning = true;
    }
}
