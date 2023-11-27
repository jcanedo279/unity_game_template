using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


[CreateAssetMenu(fileName = "UIObjectStringButtonContainer", menuName = "UI System/UI Objects/UI Containers/UI Object StringButton Container")]
public class UIObjectStringButtonContainer : UIObjectContainer<string>, IUIObjectWithValue
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
        if (itemUIObject is UIObjectStringButton uiObjectStringButton) {
            runtimeProperties.itemRuntimePropertiesList = new List<UIObjectRuntimeProperties>();
            foreach (string itemTextData in containerItemData)
            {
                uiObjectStringButton.uiObjectValue = itemTextData;
                uiObjectStringButton.textContent = itemTextData;
                
                // Fill the items and add their (sub)contents to the childRuntimeProperties.
                uiObjectStringButton.FillFromComponentManager(parentComponent, runtimeProperties.contentGameObject.transform,
                                                    uiTheme, Vector3.zero)
                    .ToList().ForEach(propertyMap => childRuntimeProperties[propertyMap.Key] = propertyMap.Value);
                runtimeProperties.itemRuntimePropertiesList.Add(
                    childRuntimeProperties[new UIObjectRuntimePropertiesId { 
                        uiComponentName=parentComponent.uiComponentName,
                        uiObjectName=uiObjectStringButton.uiObjectName,
                        uiObjectValue=uiObjectStringButton.uiObjectValue}.Id
                    ]);
            }
        } else {
            throw new System.ArgumentException($"The UIObjectStringButtonContainer: {uiObjectName} must have a valid itemUIObject (UIObjectStringButton).");
        }
        return childRuntimeProperties;
    }
}
