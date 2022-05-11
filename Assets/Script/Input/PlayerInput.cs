using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : 
    ScriptableObject, 
    InputActions.IGamePlayActions,
    InputActions.IPauseMenuActions
{
    public event UnityAction<Vector2> onMove = delegate {};
    public event UnityAction onStopMove = delegate {};
    public event UnityAction onAttack = delegate {};
    public event UnityAction onDodge = delegate {};
    public event UnityAction onPause = delegate{};
    public event UnityAction onUnpause = delegate{};
    public event UnityAction<Vector2> getMousePos = delegate {};
    
    InputActions inputActions;

    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.GamePlay.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    void SwitchActionMap(InputActionMap actionMap)
    {
        inputActions.Disable();
        actionMap.Enable();
    }

    public void SwitchToDynanicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInputs() => inputActions.Disable();
    public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay);
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu);
    //public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);

    public void OnMove(InputAction.CallbackContext context)
    {
        //if(context.phase == InputActionPhase.Performed)       //原式
        if(context.performed)       //簡式
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onAttack.Invoke();
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            getMousePos.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onDodge.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }
    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }
}
