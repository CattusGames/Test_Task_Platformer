using System;
using UnityEngine;

namespace Services.InputHandler
{
    public interface ITouchInput
    {
        bool IsTouch { get; }
        Vector2 Delta { get; }
        event Action<Vector2> OnTouchInput;
    }
}