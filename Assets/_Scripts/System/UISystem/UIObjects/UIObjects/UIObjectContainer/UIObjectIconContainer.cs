using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


// Can we move this into the interface somehow?
[System.Serializable]
public class ItemWithImageChildData {
    public Sprite sprite;
    public string uiObjectValue;
}

[CreateAssetMenu(fileName = "UIObjectIconContainer", menuName = "UI System/UI Objects/UI Containers/UI Object Icon Container")]
public class UIObjectIconContainer : UIObjectContainer<ItemWithImageChildData>
{
    public override void FillFromComponentManager(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                  UIComponent parentComponent,
                                                  Transform parentTransform,
                                                  UITheme uiTheme,
                                                  Vector2 uiObjectPosition)
    {
        FillContainerBase(uiObjectRuntimeProperties, parentComponent, parentTransform, uiTheme, uiObjectPosition);
        if (itemUIObject is UIObjectIconButton uiObjectIconButton) {
            FillContainerIconButtonItems(uiObjectIconButton,uiObjectRuntimeProperties,parentComponent,uiTheme,uiObjectPosition);
        }
        uiObjectRuntimeProperties.isFilled = true;
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
                uiObjectValue = $"Container/{itemData.uiObjectValue}"
            };
            itemObject.childImageSprite = itemData.sprite;
            itemObject.FillFromComponentManager(itemUIObjectRuntimeProperties,
                                                parentComponent, uiObjectRuntimeProperties.contentGameObject.transform,
                                                uiTheme, uiObjectPosition);
            uiObjectRuntimePropertiesList.Add(itemUIObjectRuntimeProperties);
        }
    }
}
