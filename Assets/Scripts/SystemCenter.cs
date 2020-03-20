using Audio;
using Cam;
using UI;
using UnityEngine;
using Utils;

public class SystemCenter : Singleton<SystemCenter>
{
    public Transform ui2D, ui3D;

    private void Awake()
    {
        AudioManager.Instance.Init();
        UIManager.Instance.Init(ui2D, ui3D);
        CameraManager.Instance.Init();

    }


}


