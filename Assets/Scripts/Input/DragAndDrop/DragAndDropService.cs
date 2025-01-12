using System.Collections;
using Input.CameraInput;
using Input.InputService;
using Items;
using UnityEngine;
using Zenject;

namespace Input.DragAndDrop
{
    public class DragAndDropService : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        
        private IInputService _inputService;
        private ICameraService _cameraService;

        private bool _isSwiping;

        [Inject]
        private void Construct(IInputService inputService, ICameraService cameraService)
        {
            _inputService = inputService;
            _cameraService = cameraService;
        }

        // Подписывается на события нажатия и отпускания пальца
        private void OnEnable()
        {
            _inputService.OnStartTouch += SwipeStart;
            _inputService.OnEndTouch += SwipeEnd;
        }

        // Отписывается от событий нажатия и отпускания пальца
        private void OnDisable()
        {
            _inputService.OnStartTouch -= SwipeStart;
            _inputService.OnEndTouch -= SwipeEnd;
        }

        private void SwipeStart()
        {
            var worldPosition = _inputService.PrimaryWorldPosition();
            var hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _layerMask);
            var item = hit.collider?.GetComponent<Item>();
            
            // Если нажали на предмет, то перетаскиваем его
            if (item is not null)
            {
                // Отступ используется для перемещения предмета за ту точку, где нажат палец, а не за центр предмета
                var offset = (Vector2)hit.transform.position - worldPosition;
                    
                StartCoroutine(DragItem(item, offset));
            }
            else // Если нажали на экран, а там ничего нет, значит можно перемещать камеру
            {
                StartCoroutine(DragCamera());
            }
        }

        private void SwipeEnd()
        {
            _isSwiping = false;
        }
        
        // Перемещение камеры, используя координаты экрана
        private IEnumerator DragCamera()
        {
            _isSwiping = true;
            var startPosition = _inputService.PrimaryScreenPosition();

            // Пока палец на экране, вычисляется дельта перемещения пальца
            while (_isSwiping)
            {
                var currentPosition = _inputService.PrimaryScreenPosition();
                var drugDelta = startPosition - currentPosition;
                _cameraService.Drag(drugDelta);
                
                startPosition = currentPosition;
                yield return null;
            }
        }

        // Перемещение с помощью перевода координат в координаты сцены
        private IEnumerator DragItem(Item item, Vector2 offset)
        {
            _isSwiping = true;
            
            var itemTransform = item.transform;
            
            while (_isSwiping)
            {
                itemTransform.position = _inputService.PrimaryWorldPosition() + offset;
                yield return null;
            }
            
            item.CancelDragging();
        }
    }
}