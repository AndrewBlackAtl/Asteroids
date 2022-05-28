using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputSystemPlayerInput : IPlayerInput
{
    private class InputActionNames
    {
        public const string MoveAction = "Move";
        public const string FireAction = "Fire";
        public const string TurnAction = "Turn";
        public const string FirstWeaponAction = "FirstWeapon";
        public const string SecondWeaponAction = "SecondWeapon";
    }

    public event Action<Vector2> OnStartTurning;
    public event Action OnStopTurning;
    public event Action OnStartMoving;
    public event Action OnStopMoving;
    public event Action OnFire;
    public event Action<int> OnSwitchWeapon;


    public InputSystemPlayerInput(PlayerInput input)
    {
        input.onActionTriggered += OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case InputActionNames.MoveAction:
                if (context.started)
                {
                    OnStartMoving?.Invoke();
                }
                else if (context.canceled)
                {
                    OnStopMoving?.Invoke();
                }
                break;

            case InputActionNames.FireAction:
                if (context.started)
                {
                    OnFire?.Invoke();
                }
                break;

            case InputActionNames.TurnAction:
                if (context.started)
                {
                    OnStartTurning?.Invoke(context.action.ReadValue<Vector2>());
                }
                else if (context.canceled)
                {
                    OnStopTurning?.Invoke();
                }
                break;

            case InputActionNames.FirstWeaponAction:
                if (context.started)
                {
                    OnSwitchWeapon?.Invoke(0);
                }
                break;

            case InputActionNames.SecondWeaponAction:
                if (context.started)
                {
                    OnSwitchWeapon?.Invoke(1);
                }
                break;

            default:
                throw new NotImplementedException($"Input action with name {context.action.name} is not implemented");
        }
    }
}
