using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIComponent", menuName = "UI System/UI Component")]
public class UIComponent : ScriptableObject {
    [SerializeField] public string uiComponentName;
    // These UI Objects are ordered so it matters.
    [SerializeField] public List<UIObject> uiObjects;
    [SerializeField] private UIObjectResponseEventChannel onClickUIObjectEventChannel;
    [SerializeField] public UINestedComponentType uiNestedComponentType;
    public GameObject uiComponentRuntime;
    public float uiObjectSpacing = 0f;
    public Vector2 startingUIObjectPosition = new Vector2(0f,0f);

    public void onClickUIObject(string uiObjectName) {
        onClickUIObjectEventChannel.RaiseEvent(new UIObjectResponse(uiObjectName,uiComponentName));
    }
}
