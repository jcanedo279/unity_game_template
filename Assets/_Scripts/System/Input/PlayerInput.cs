using UnityEngine;


public class PlayerInputManager : MonoBehaviour {
    [SerializeField] UIComponentRequestEventChannel uiComponentRequestEventChannel;

    public void OnOpenUIRequest() {
        Debug.Log("heng");
        if (uiComponentRequestEventChannel == null) {
            return;
        }
        print("hello");
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("UIComponentPurchaseRoom",
                            UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE));
    }
}
