using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIObjectStringButtonContainer", menuName = "UI System/UI Objects/UI Containers/UI Object StringButton Container")]
public class UIObjectStringButtonContainer : UIObjectContainer<string>
{
    public override void FillFromComponentManager(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                  UIComponent parentComponent,
                                                  Transform parentTransform,
                                                  UITheme uiTheme,
                                                  Vector2 uiObjectPosition)
    {
        FillContainerBase(uiObjectRuntimeProperties, parentComponent, parentTransform, uiTheme, uiObjectPosition);
        if (itemUIObject is UIObjectStringButton uiObjectStringButton) {
            FillContainerStringButtonItems(uiObjectStringButton,uiObjectRuntimeProperties,parentComponent,uiTheme,uiObjectPosition);
        }
        uiObjectRuntimeProperties.isFilled = true;
    }
    public void FillContainerStringButtonItems(UIObjectStringButton itemObject,
                                             UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                             UIComponent parentComponent,
                                             UITheme uiTheme,
                                             Vector2 uiObjectPosition) {
        // Fill in each item.
        uiObjectRuntimePropertiesList = new List<UIObjectRuntimeProperties>();
        foreach (string itemTextData in containerItemData)
        {
            UIObjectRuntimeProperties itemUIObjectRuntimeProperties = new UIObjectRuntimeProperties
            {
                uiObjectValue = $"Container/{itemTextData}"
            };
            itemObject.textContent = itemTextData;
            itemObject.FillFromComponentManager(itemUIObjectRuntimeProperties,
                                                parentComponent, uiObjectRuntimeProperties.contentGameObject.transform,
                                                uiTheme, uiObjectPosition);
            uiObjectRuntimePropertiesList.Add(itemUIObjectRuntimeProperties);
        }
    }
}
