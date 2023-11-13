using UnityEngine;
using System.Collections.Generic;


// This class just listens to different channels and uses them to update the GameState multiCriteria.
public class GameStateDriver : MonoBehaviour {
    private UIObjectResponseEventChannelListener onClickUIObjectEventChannelListener;
    [SerializeField] StringBoolMapEventChannel gameStateTransitionMultiCriteriaEventChannel;

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
            gameStateTransitionMultiCriteriaEventChannel.RaiseEvent(new Dictionary<string,bool>{
                {"isCriteriaMetAquariumRequest",true},
            });
        }
    }
}
