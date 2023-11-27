using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Objects/UI Object")]
public class UIObject : ScriptableObject
{
    [SerializeField] string _uiObjectName;
    public string uiObjectName { get { return _uiObjectName; } }
    public virtual string uiObjectValue { get; set; }
    [SerializeField] GameObject _uiObjectPrefab;
    public GameObject uiObjectPrefab { get { return _uiObjectPrefab; } }


    public Dictionary<string,UIObjectRuntimeProperties> FillFromComponentManager(
        UIComponent parentComponent,
        Transform parentTransform, // This may be different than parentComponent if we are filling from a UIObject Container.
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        UIObjectRuntimeProperties runtimeProperties
            = FillUIObjectRuntimeProperties(parentComponent, parentTransform, uiTheme, uiObjectPosition);
        Dictionary<string, UIObjectRuntimeProperties> filledRuntimeProperties
            = FillChildUIObjectRuntimeProperties(runtimeProperties, parentComponent, parentTransform, uiTheme);
        filledRuntimeProperties.Add(runtimeProperties.propertyId.Id, runtimeProperties);
        runtimeProperties.isFilled = true;
        return filledRuntimeProperties;
    }

    public virtual UIObjectRuntimeProperties FillUIObjectRuntimeProperties(
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        GameObject runtimeObject = Instantiate(uiObjectPrefab, parentTransform);
        UIObjectRuntimeProperties runtimeProperties = new UIObjectRuntimeProperties {
            propertyId = new UIObjectRuntimePropertiesId {
                uiComponentName=parentComponent.uiComponentName,uiObjectName=uiObjectName,uiObjectValue=uiObjectValue
            },
            uiObjectRuntime = runtimeObject,
            rectTransform = runtimeObject.gameObject.GetComponent<RectTransform>(),
            RenderFromComponentManager = RenderFromComponentManager
        };
        runtimeProperties.rectTransform.localPosition = uiObjectPosition;
        FillUIObjectByInterface(runtimeProperties, parentComponent, uiTheme);
        return runtimeProperties;
    }

    // Fill in UIObjects which implement an IUIObject interface.
    private void FillUIObjectByInterface(UIObjectRuntimeProperties runtimeProperties,
                                         UIComponent parentComponent,
                                         UITheme uiTheme)
    {
        IUIObjectFillers.MaybeFillUIObjectSize(runtimeProperties, this);
        IUIObjectFillers.MaybeFillUIObjectImage(runtimeProperties, this);
        IUIObjectFillers.MaybeFillUIObjectImageColor(runtimeProperties, this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectTextChild(runtimeProperties, this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectButton(runtimeProperties, parentComponent, this);
        IUIObjectFillers.MaybeFillUIObjectImageChild(runtimeProperties, this);
    }

    public virtual Dictionary<string,UIObjectRuntimeProperties> FillChildUIObjectRuntimeProperties(
        UIObjectRuntimeProperties runtimeProperties,
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme)
    {
        return new Dictionary<string, UIObjectRuntimeProperties>();
    }

    public virtual void RenderFromComponentManager(
        UIObjectRuntimeProperties runtimeProperties,
        UIComponent parentComponent,
        UITheme uiTheme)
    {
        // No-op... for now :>
    }
}
