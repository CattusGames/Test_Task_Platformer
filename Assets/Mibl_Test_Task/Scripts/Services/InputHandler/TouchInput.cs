using System;
using UnityEngine;
using Zenject;

namespace Services.InputHandler
{
    public class TouchInput : ITouchInput, ITickable
    {
        private Vector3 _lastPosition;
        
        public bool IsTouch { get; private set; }
        public Vector2 Delta { get; private set; }
        public event Action<Vector2> OnTouchInput;

        public void Tick()
        {
            IsTouch = Input.GetMouseButton(0);

            Delta = Input.mousePosition - _lastPosition;
            if(Input.GetMouseButtonDown(0) || !IsTouch)
                Delta = Vector2.zero;

            _lastPosition = Input.mousePosition;
            
            OnTouchInput?.Invoke(Delta);
        }
    }
}