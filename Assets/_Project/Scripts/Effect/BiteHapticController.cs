using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class BiteHapticController : MonoBehaviour
{
    [SerializeField] private MMF_Player _biteHaptic;

    void Start()
    {
        MessageBus.Subscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
    }

    void OnDestroy()
    {
        MessageBus.Unsubscribe<OnFoxBiteSquirrel>(Handler_OnFoxBiteSquirrel);
    }

    private void Handler_OnFoxBiteSquirrel(OnFoxBiteSquirrel squirrel)
    {
        _biteHaptic.PlayFeedbacks();
    }
}
