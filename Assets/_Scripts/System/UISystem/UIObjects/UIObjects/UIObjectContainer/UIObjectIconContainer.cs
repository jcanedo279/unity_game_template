using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


// Can we move this into the interface somehow?
[System.Serializable]
public class ItemWithImageChildData {
    public Sprite sprite;
    public string uiObjectValue;
}

[CreateAssetMenu(fileName = "UIObjectIconContainer", menuName = "UI System/UI Objects/UI Containers/UI Object Icon Container")]
public class UIObjectIconContainer : UIObjectContainer<ItemWithImageChildData>
{
    public override void FillContainerUIObjectRuntimeProperties(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        UITheme uiTheme)
    {
        base.FillContainerUIObjectRuntimeProperties(uiObjectRuntimeProperties, parentComponent, uiTheme);
    }

    public override Dictionary<string,UIObjectRuntimeProperties> FillChildUIObjectRuntimeProperties(
        UIObjectRuntimeProperties runtimeProperties,
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme)
    {
        Dictionary<string, UIObjectRuntimeProperties> childRuntimeProperties
            = base.FillChildUIObjectRuntimeProperties(runtimeProperties, parentComponent, parentTransform, uiTheme);
        if (itemUIObject is UIObjectIconButton uiObjectIconButton) {
            runtimeProperties.itemRuntimePropertiesList = new List<UIObjectRuntimeProperties>();
            foreach (ItemWithImageChildData itemData in containerItemData)
            {
                uiObjectIconButton.uiObjectValue = itemData.uiObjectValue;
                uiObjectIconButton.childImageSprite = itemData.sprite;

                // Fill the items and add their (sub)contents to the childRuntimeProperties (returned) and itemPropertiesList (container property).
                uiObjectIconButton.FillFromComponentManager(parentComponent, runtimeProperties.contentGameObject.transform,
                                                    uiTheme, Vector3.zero)
                    .ToList().ForEach(propertyMap => childRuntimeProperties[propertyMap.Key] = propertyMap.Value);
                runtimeProperties.itemRuntimePropertiesList.Add(
                    childRuntimeProperties[new UIObjectRuntimePropertiesId { 
                        uiComponentName=parentComponent.uiComponentName,
                        uiObjectName=uiObjectIconButton.uiObjectName,
                        uiObjectValue=uiObjectIconButton.uiObjectValue}.Id
                    ]);
            }
        } else {
            throw new System.ArgumentException($"The UIObjectIconContainer: {uiObjectName} must have a valid itemUIObject (UIObjectIconButton).");
        }
        return childRuntimeProperties;
    }
}
