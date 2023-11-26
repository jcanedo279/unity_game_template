using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


[CreateAssetMenu(fileName = "UIObjectStringButtonContainer", menuName = "UI System/UI Objects/UI Containers/UI Object StringButton Container")]
public class UIObjectStringButtonContainer : UIObjectContainer<string>
{
    public override Dictionary<string,UIObjectRuntimeProperties> FillFromComponentManager(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        FillContainerBase(uiObjectRuntimeProperties, parentComponent, parentTransform, uiTheme, uiObjectPosition);
        if (itemUIObject is UIObjectStringButton uiObjectStringButton) {
            FillContainerStringButtonItems(uiObjectStringButton,uiObjectRuntimeProperties,parentComponent,uiTheme,uiObjectPosition);
        }
        FillContainerSize(uiObjectRuntimeProperties, parentComponent, uiTheme);
        uiObjectRuntimeProperties.isFilled = true;
        return uiObjectRuntimePropertiesList.ToDictionary(runtimeProperties => runtimeProperties.propertyId.Id, runtimeProperties => runtimeProperties);
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
                propertyId = new UIObjectRuntimePropertiesId {
                    uiComponentName=parentComponent.uiComponentName, uiObjectName=itemObject.uiObjectName, uiObjectValue=itemTextData
                },
                stringValueClickProperties = new IUIObjectWithStringValueClick.IUIObjectWithStringValueClickProperties {
                    uiObjectValue = $"Container/{itemTextData}"
                },
                textChildProperties = new IUIObjectWithTextChild.IUIObjectWithTextChildProperties {
                    textContent = itemTextData
                }
            };
            uiObjectRuntimeProperties.containerProperties.itemRuntimePropertiesList = uiObjectRuntimePropertiesList;
            itemObject.FillFromComponentManager(itemUIObjectRuntimeProperties,
                                                parentComponent, uiObjectRuntimeProperties.containerProperties.contentGameObject.transform,
                                                uiTheme, uiObjectPosition);
            uiObjectRuntimePropertiesList.Add(itemUIObjectRuntimeProperties);
        }
    }
}
