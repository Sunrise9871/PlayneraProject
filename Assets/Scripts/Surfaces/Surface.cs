using UnityEngine;

namespace Surfaces
{
    public class Surface : MonoBehaviour
    {
        private static readonly Color FrameColor = Color.blue;
        private static readonly Color CenterColor = new(0.5f, 0.5f, 1f, 0.25f);

        [SerializeField] private Vector2 _size = new(5f, 1f);

        private void OnDrawGizmos()
        {
            var center = transform.position;

            Gizmos.color = CenterColor;
            Gizmos.DrawCube(center, new Vector3(_size.x, _size.y, 0));

            Gizmos.color = FrameColor;
            Gizmos.DrawWireCube(center, new Vector3(_size.x, _size.y, 0));
        }

        public Vector3 GetClosestPoint(Vector3 itemPosition)
        {
            var halfSize = _size / 2f;
            var center = transform.position;

            var clampedX = Mathf.Clamp(itemPosition.x, center.x - halfSize.x, center.x + halfSize.x);
            var clampedY = Mathf.Clamp(itemPosition.y, center.y - halfSize.y, center.y + halfSize.y);

            return new Vector3(clampedX, clampedY, itemPosition.z);
        }
    }
}