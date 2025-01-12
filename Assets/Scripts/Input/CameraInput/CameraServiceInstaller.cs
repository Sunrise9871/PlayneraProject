using UnityEngine;
using Zenject;

namespace Input.CameraInput
{
    public class CameraServiceInstaller : MonoInstaller
    {
        [SerializeField] private int _targetFramerate = 60;
        [SerializeField] private SpriteRenderer _backgroundSprite;
        
        public override void InstallBindings()
        {
            Application.targetFrameRate = _targetFramerate;
            
            Container
                .BindInterfacesAndSelfTo<CameraService>()
                .AsSingle()
                .WithArguments(_backgroundSprite);
        }
    }
}