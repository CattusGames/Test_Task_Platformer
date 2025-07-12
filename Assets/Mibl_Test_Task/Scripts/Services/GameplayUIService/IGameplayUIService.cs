using System;

namespace Mibl_Test_Task.Scripts.Services.GameplayUIService
{
    public interface IGameplayUIService
    {
        event Action<GameplayInputType> OnInputDown;
        event Action<GameplayInputType> OnInputUp;

        bool IsHeld(GameplayInputType input);
        void Press(GameplayInputType inputType);
        void Release(GameplayInputType inputType);
    }

}