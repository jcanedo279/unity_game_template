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
    public override Dictionary<string,UIObjectRuntimeProperties> FillFromComponentManager(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        FillContainerBase(uiObjectRuntimeProperties, parentComponent, parentTransform, uiTheme, uiObjectPosition);
        if (itemUIObject is UIObjectIconButton uiObjectIconButton) {
            FillContainerIconButtonItems(uiObjectIconButton,uiObjectRuntimeProperties,parentComponent,uiTheme,uiObjectPosition);
        }
        FillContainerSize(uiObjectRuntimeProperties, parentComponent, uiTheme);
        uiObjectRuntimeProperties.isFilled = true;
        return uiObjectRuntimePropertiesList.ToDictionary(runtimeProperties => runtimeProperties.propertyId.Id, runtimeProperties => runtimeProperties);
    }
    public void FillContainerIconButtonItems(UIObjectIconButton itemObject,
                                             UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                             UIComponent parentComponent,
                                             UITheme uiTheme,
                                             Vector2 uiObjectPosition) {
        // Fill in each item.
        uiObjectRuntimePropertiesList = new List<UIObjectRuntimeProperties>();
        foreach (ItemWithImageChildData itemData in containerItemData)
        {
            UIObjectRuntimeProperties itemUIObjectRuntimeProperties = new UIObjectRuntimeProperties
            {
                propertyId = new UIObjectRuntimePropertiesId {
                    uiComponentName=parentComponent.uiComponentName, uiObjectName=itemObject.uiObjectName, uiObjectValue=itemData.uiObjectValue
                },
                stringValueClickProperties = new IUIObjectWithStringValueClick.IUIObjectWithStringValueClickProperties {
                    uiObjectValue = $"Container/{itemData.uiObjectValue}"
                },
                imageChildProperties = new IUIObjectWithImageChild.IUIObjectWithImageChildProperties {
                    childImageSprite = itemData.sprite
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
