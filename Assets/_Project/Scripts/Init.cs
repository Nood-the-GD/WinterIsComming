using Core.SoundManager;
using UnityEngine;

public class Init : MonoBehaviour
{
    void Awake()
    {
        ServiceManager.Init();
        MessageBus.Init();
        ServiceManager.Register<SoundManager>();
    }

    void Start()
    {
        ServiceManager.Get<SoundManager>().PlayMusic(MusicEnum.BGM);
    }
}
