using UnityEngine;
using System.Collections.Generic;


// This class just listens to different channels and uses them to update the GameState multiCriteria.
public class GameStateDriver : MonoBehaviour {
    private UIObjectResponseEventChannelListener onClickUIObjectEventChannelListener;
    [SerializeField] GameStateEventChannel gameStateEventChannel;

    public void Awake() {
        onClickUIObjectEventChannelListener = GetComponent<UIObjectResponseEventChannelListener>();
        if (onClickUIObjectEventChannelListener != null) {
            onClickUIObjectEventChannelListener.UnityEventResponse += OnClickUIObject;
        }
    }

    public void OnClickUIObject(UIObjectResponse uiObjectResponse) {
        string uiObjectName = uiObjectResponse.uiObjectName;
        string uiComponentName = uiObjectResponse.uiComponentName;

        if (uiObjectName=="ButtonPlay" && uiComponentName=="MainMenuComponent") {
            gameStateEventChannel.RaiseEvent(GameState.GAME_STATE_AQUARIUM);
        }
    }
}
