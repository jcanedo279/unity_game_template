using UnityEngine;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Objects/UI Object")]
public class UIObject : ScriptableObject
{
    public string uiObjectName;
    public GameObject uiObjectPrefab;
    // Runtime properties.
    // public UIObjectRuntimeProperties uiObjectRuntimeProperties;


    public virtual void FillFromComponentManager(
                      UIObjectRuntimeProperties uiObjectRuntimeProperties,
                      UIComponent parentComponent,
                      // This may be different than parentComponent if we are filling from a UIObject Container.
                      Transform parentTransform,
                      UITheme uiTheme,
                      Vector2 uiObjectPosition)
    {
        uiObjectRuntimeProperties.uiObjectRuntime = Instantiate(uiObjectPrefab, parentTransform);
        uiObjectRuntimeProperties.rectTransform
            = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        uiObjectRuntimeProperties.rectTransform.localPosition = uiObjectPosition;
        FillUIObjectByInterface(uiObjectRuntimeProperties, parentComponent, uiTheme);
        uiObjectRuntimeProperties.isFilled = true;
    }

    // Fill in UIObjects which implement an IUIObject interface.
    private void FillUIObjectByInterface(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                         UIComponent parentComponent,
                                         UITheme uiTheme)
    {
        IUIObjectFillers.MaybeFillUIObjectSize(uiObjectRuntimeProperties, this);
        IUIObjectFillers.MaybeFillUIObjectImage(uiObjectRuntimeProperties, this);
        IUIObjectFillers.MaybeFillUIObjectImageColor(uiObjectRuntimeProperties, this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectTextChild(uiObjectRuntimeProperties, this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectButton(uiObjectRuntimeProperties, parentComponent, this);
        IUIObjectFillers.MaybeFillUIObjectImageChild(uiObjectRuntimeProperties, this);
    }
}
