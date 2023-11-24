using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputManager : MonoBehaviour {
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference openUIAction;
    private bool isMoving = false;

    void Awake() {
        moveAction.action.performed += context => isMoving=!isMoving;
        openUIAction.action.started += OnOpenUIRequest;
    }
    private void OnEnable()
    {
        moveAction.action.Enable();
        openUIAction.action.Enable();
    }
    private void OnDisable()
    {
        moveAction.action.Disable();
        openUIAction.action.Disable();
    }

    [SerializeField] UIComponentRequestEventChannel uiComponentRequestEventChannel;
    public void OnOpenUIRequest(InputAction.CallbackContext callbackContext) {
        Debug.Log("Opening UI Purchase Room.");
        if (uiComponentRequestEventChannel == null) {
            return;
        }
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumComponent",
                            UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_FLIP_ENABLE));
    }

    [SerializeField] PlayerInputResponseEventChannel playerInputResponseEventChannel;

    public void FixedUpdate() {
        PlayerInputResponse inputResponse = new PlayerInputResponse();
        if (isMoving) {
            inputResponse.moveVector = moveAction.action.ReadValue<Vector2>();
        }
        playerInputResponseEventChannel.RaiseEvent( inputResponse );
    }
}

public class PlayerInputResponse {
    public Vector2 moveVector;
}
