using UnityEngine;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Objects/UI Object")]
public class UIObject : ScriptableObject {
    public string uiObjectName;
    public GameObject uiObjectPrefab;
    // Runtime properties.
    public UIObjectRuntimeProperties uiObjectRuntimeProperties;


    public virtual void FillFromComponentManager(UIComponent parentComponent,
                      // This may be different than parentComponent if we are filling from a UIObject Container.
                      Transform parentTransform,
                      UITheme uiTheme,
                      Vector2 uiObjectPosition) {
        uiObjectRuntimeProperties = new UIObjectRuntimeProperties
        {
            uiObjectRuntime = Instantiate(uiObjectPrefab, parentTransform)
        };
        uiObjectRuntimeProperties.rectTransform
            = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        uiObjectRuntimeProperties.rectTransform.localPosition = uiObjectPosition;
        FillUIObjectByInterface(parentComponent, uiTheme);
        uiObjectRuntimeProperties.isFilled = true;
    }

    // Fill in UIObjects which implement an IUIObject interface.
    private void FillUIObjectByInterface(UIComponent parentComponent, UITheme uiTheme) {
        IUIObjectFillers.MaybeFillUIObjectSize(this);
        IUIObjectFillers.MaybeFillUIObjectImage(this);
        IUIObjectFillers.MaybeFillUIObjectImageColor(this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectTextChild(this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectButton(parentComponent, this);
        IUIObjectFillers.MaybeFillUIObjectImageChild(this);
    }
}

public class UIObjectRuntimeProperties {
    public bool isFilled = false;
    public GameObject uiObjectRuntime;
    public RectTransform rectTransform;
}
