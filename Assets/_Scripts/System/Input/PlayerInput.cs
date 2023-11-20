using UnityEngine;


public class PlayerInputManager : MonoBehaviour {
    [SerializeField] UIComponentRequestEventChannel uiComponentRequestEventChannel;

    public void OnOpenUIRequest() {
        Debug.Log("Opening UI Purchase Room.");
        if (uiComponentRequestEventChannel == null) {
            return;
        }
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("UIComponentPurchaseRoom",
                            UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_FLIP_ENABLE));
    }
}
