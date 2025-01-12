using UnityEngine;
using Zenject;

namespace Input.CameraInput
{
    public class CameraService : ICameraService, IInitializable
    {
        private SpriteRenderer _backgroundSprite;

        private Camera _mainCamera;
        private float _minX, _maxX;

        // Zenject
        public CameraService(SpriteRenderer backgroundSprite)
        {
            _backgroundSprite = backgroundSprite;
        }

        public void Initialize()
        {
            _mainCamera = Camera.main;

            SetCameraSize();
            SetCameraBounds();
        }

        //Перемещение камеры
        public void Drag(Vector2 drugDelta)
        {
            var worldDelta = ScreenToWorldDelta(drugDelta);

            var newPosition = _mainCamera.transform.position;
            newPosition.x = Mathf.Clamp(newPosition.x + worldDelta.x, _minX, _maxX);

            _mainCamera.transform.position = newPosition;
        }

        // Установка пределов движения камеры, чтобы не уйти за пределы background'а
        private void SetCameraBounds()
        {
            var cameraHeight = _mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            var roomBounds = _backgroundSprite.bounds;

            _minX = roomBounds.min.x + cameraWidth / 2f;
            _maxX = roomBounds.max.x - cameraWidth / 2f;
        }

        // Установка вертикального размера камеры в зависимости от размера background'а
        private void SetCameraSize()
        {
            var roomHeight = _backgroundSprite.bounds.size.y;
            _mainCamera.orthographicSize = roomHeight / 2f;
        }

        // Перевод координат экрана в координаты сцены
        private Vector3 ScreenToWorldDelta(Vector2 screenDelta)
        {
            var screenPoint =
                _mainCamera.ScreenToWorldPoint(new Vector3(screenDelta.x, screenDelta.y, _mainCamera.nearClipPlane));
            var zeroPoint = _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
            return screenPoint - zeroPoint;
        }
    }
}