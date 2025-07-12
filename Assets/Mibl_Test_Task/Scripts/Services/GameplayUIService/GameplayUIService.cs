using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Services.GameplayUIService
{
    using System;
    using System.Collections.Generic;
    using Zenject;

    public class GameplayUIService : IGameplayUIService, ITickable
    {
        public event Action<GameplayInputType> OnInputDown;
        public event Action<GameplayInputType> OnInputUp;

        private readonly HashSet<GameplayInputType> _heldInputs = new();

        public bool IsHeld(GameplayInputType input) => _heldInputs.Contains(input);

        public void Press(GameplayInputType input)
        {
            if (_heldInputs.Add(input))
            {
                OnInputDown?.Invoke(input);
            }
        }

        public void Release(GameplayInputType input)
        {
            if (_heldInputs.Remove(input))
            {
                OnInputUp?.Invoke(input);
            }

            if (input is GameplayInputType.MoveLeft or GameplayInputType.MoveRight)
            {
                if (!_heldInputs.Contains(GameplayInputType.MoveLeft) &&
                    !_heldInputs.Contains(GameplayInputType.MoveRight))
                {
                    OnInputDown?.Invoke(GameplayInputType.MoveReleased);
                }
            }
        }

        public void Tick()
        {
            if (_heldInputs.Contains(GameplayInputType.MoveLeft))
                OnInputDown?.Invoke(GameplayInputType.MoveLeft);

            if (_heldInputs.Contains(GameplayInputType.MoveRight))
                OnInputDown?.Invoke(GameplayInputType.MoveRight);
            
            if (_heldInputs.Contains(GameplayInputType.Attack))
                OnInputDown?.Invoke(GameplayInputType.Attack);
        }
    }
}