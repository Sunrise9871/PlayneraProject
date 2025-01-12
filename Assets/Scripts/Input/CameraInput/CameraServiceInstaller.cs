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
            Application.targetFrameRate = _targetFramerate; // 60 fps
            
            Container
                .BindInterfacesAndSelfTo<CameraService>() // Используя интерфейс ICameraService, использовать реализацию CameraService
                .AsSingle() // Как единственный экземпляр
                .WithArguments(_backgroundSprite); // Передавая аргументы в конструктор
        }
    }
}