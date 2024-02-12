using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class PB_InputReaderSO : ScriptableObject, PB_Input.IGameplayActions
{
    public event UnityAction shootEvent = delegate { };
    public event UnityAction<float> turnEvent = delegate { };

    private PB_Input PBInputs = null;

    private void OnEnable()
    {
        if (PBInputs == null)
        {
            PBInputs = new PB_Input();
            PBInputs.Gameplay.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    public void EnableGameplayInput()
    {
        if(PBInputs != null)
        {
            PBInputs.Gameplay.Enable();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            shootEvent.Invoke();
    }
    
    public void OnTurn(InputAction.CallbackContext context)
    {
        turnEvent.Invoke(context.ReadValue<float>());
    }

}
