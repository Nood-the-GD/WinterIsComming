using Core.SoundManager;
using UnityEngine;

public class Init : MonoBehaviour
{
    void Awake()
    {
        ServiceManager.Init();
        MessageBus.Init();

    }

    void Start()
    {
        ServiceManager.Register<SoundManager>();
        ServiceManager.Get<SoundManager>().PlayMusic(MusicEnum.BGM);
    }
}
