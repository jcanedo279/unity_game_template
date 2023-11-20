using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIComponent", menuName = "UI System/UI Component")]
public class UIComponent : ScriptableObject {
    [SerializeField] public string uiComponentName;
    // These UI Objects are ordered so it matters.
    [SerializeField] public List<UIObject> uiObjects;
    [SerializeField] public UINestedComponentType uiNestedComponentType;
    [System.NonSerialized] public GameObject uiComponentRuntime;
    public float uiObjectSpacing = 16f;
    public Vector2 startingUIObjectPosition = new Vector2(0f,0f);


    // INTERFACE - IUIObjectWithClick
    // -----------------------------------------------------------------------------------
    [SerializeField] private UIObjectResponseEventChannel onClickUIObjectEventChannel;
    public bool CanInterceptUIObjectWithClick() {
        if (onClickUIObjectEventChannel == null) {
            return false;
        }
        return true;
    }
    public void OnClickUIObject(string uiObjectName) {
        if (!CanInterceptUIObjectWithClick()) {
            return;
        }
        onClickUIObjectEventChannel.RaiseEvent(new UIObjectResponse(uiObjectName,uiComponentName));
    }

    // INTERFACE - IUIObjectWithStringValueClick
    // -----------------------------------------------------------------------------------
    public void OnClickUIObject(string uiObjectName, string uiObjectValue) {
        if (!CanInterceptUIObjectWithClick()) {
            return;
        }
        onClickUIObjectEventChannel.RaiseEvent(
            new UIObjectResponse(uiObjectName,uiObjectValue,uiComponentName));
    }
}
