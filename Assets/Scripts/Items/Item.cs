using System.Collections;
using Surfaces;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Collider2D))]
    public class Item : MonoBehaviour
    {
        private const float MinimalDistance = 0.01f;

        [SerializeField] private float _fallSpeed = 5f;
        [SerializeField] private LayerMask _layerMask;

        private Collider2D _itemCollider;

        private void Awake()
        {
            _itemCollider = GetComponent<Collider2D>();
        }

        public void CancelDragging()
        {
            var rayStart = _itemCollider.bounds.min;
            var hit = Physics2D.Raycast(rayStart, Vector2.down, Mathf.Infinity, _layerMask);

            // Получает компонент Surface под объектом
            var surface = hit.collider?.GetComponent<Surface>();
            if (surface is not null)
            {
                var targetPosition = surface.GetClosestPoint(transform.position);
                MoveToSurface(targetPosition);

                return;
            }

            MoveToSurface(Vector3.zero);
        }

        // Вычисляет начальную и конечную точки для перемещения предмета
        private void MoveToSurface(Vector3 targetPosition)
        {
            var startPosition = transform.position;
            var endPosition = targetPosition;

            var itemBottomY = _itemCollider.bounds.min.y;

            if (itemBottomY > targetPosition.y)
            {
                var offset = itemBottomY - startPosition.y;
                endPosition.y -= offset;
            }
            else
            {
                endPosition.y = startPosition.y;
            }

            StartCoroutine(MoveToSurfaceCoroutine(startPosition, endPosition));
        }

        // Плавно перемещает предмет от начальной к конечной точке
        private IEnumerator MoveToSurfaceCoroutine(Vector3 startPosition, Vector3 endPosition)
        {
            var elapsedTime = 0f;
            while (Vector3.Distance(transform.position, endPosition) > MinimalDistance)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime * _fallSpeed);
                yield return null;
            }

            transform.position = endPosition;
        }
    }
}