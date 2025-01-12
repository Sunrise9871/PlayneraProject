using System;
using UnityEngine;

namespace Input.InputService
{
    public interface IInputService
    {
        public event Action OnStartTouch; 
        public event Action OnEndTouch;
        
        public Vector2 PrimaryScreenPosition();

        public Vector2 PrimaryWorldPosition();
    }
}