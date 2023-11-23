using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputManager : MonoBehaviour {
    [SerializeField] UIComponentRequestEventChannel uiComponentRequestEventChannel;
    // public PlayerInputActions playerControls;
    private InputAction moveAction;

    public void OnOpenUIRequest() {
        Debug.Log("Opening UI Purchase Room.");
        if (uiComponentRequestEventChannel == null) {
            return;
        }
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("UIComponentPurchaseAquarium",
                            UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_FLIP_ENABLE));
    }

    // public void Awake() {
    //     playerControls = new PlayerInputActions();
    // }
}

public class PlayerInputResponse {
    public float[] wasd = new float[4];
}
