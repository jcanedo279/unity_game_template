using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Objects/UI Object")]
public class UIObject : ScriptableObject
{
    [SerializeField] string _uiObjectName;
    public string uiObjectName { get { return _uiObjectName; } }
    [SerializeField] GameObject _uiObjectPrefab;
    GameObject uiObjectPrefab { get { return _uiObjectPrefab; } }


    public virtual Dictionary<string,UIObjectRuntimeProperties> FillFromComponentManager(
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
        return new Dictionary<string, UIObjectRuntimeProperties>();
    }

    public virtual void RenderFromComponentManager(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        UITheme uiTheme)
    {
        // No-op... for now :>
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
