using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input.InputService
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private PlayerInputActions _playerInput;
        private Camera _camera;

        public event Action OnStartTouch; 
        public event Action OnEndTouch;

        // Инициализация управления (New Input System) и подписка на его события
        public void Initialize()
        {
            _playerInput = new PlayerInputActions();
            _camera = Camera.main;
            
            _playerInput.Enable();
            _playerInput.Touch.TouchContact.started += StartTouchPrimary;
            _playerInput.Touch.TouchContact.canceled += EndTouchPrimary;
        }
        
        // Освобождение ресурсов
        public void Dispose()
        {
            _playerInput?.Disable();
            _playerInput?.Dispose();
        }
        
        // Возвращает координаты экрана, где нажал палец
        public Vector2 PrimaryScreenPosition()
        {
            return _playerInput.Touch.TouchPosition.ReadValue<Vector2>();
        }

        // Возвращает координаты сцены, где нажал палец
        public Vector2 PrimaryWorldPosition()
        {
            return ScreenToWorld(_camera, PrimaryScreenPosition());
        }

        // Вызов события нажатия пальца
        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            OnStartTouch?.Invoke();
        }
        
        // Вызов события отпускания пальца
        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            OnEndTouch?.Invoke();
        }

        // Перевод координат экрана в координаты сцены
        private Vector3 ScreenToWorld(Camera sceneCamera, Vector3 position)
        {
            position.z = sceneCamera.nearClipPlane;
            return sceneCamera.ScreenToWorldPoint(position);
        }
    }
}