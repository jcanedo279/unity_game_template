using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public enum UIObjectContainerLayoutDirection
{
    LAYOUT_DIRECTION_SCROLL_HORIZONTAL,
    LAYOUT_DIRECTION_SCROLL_VERTICAL,
    LAYOUT_DIRECTION_HORIZONTAL,
    LAYOUT_DIRECTION_VERTICAL,
}

public interface IUIObjectWithContainerItem : IUIObjectWithSize,
                                              IUIObjectWithImage, IUIObjectWithImageColor,
                                              IUIObjectWithStringValueClick {
}

public abstract class UIObjectContainer<TContainerItem> : UIObject
{
    [SerializeField] protected UIObject itemUIObject;
    [SerializeField] public List<TContainerItem> containerItemData = new List<TContainerItem>();
    [SerializeField] private UIObjectContainerLayoutDirection layoutDirection;


    // DERIVED - UIOBJECT
    // ------------------------------------------------------------------------------------------------------------------
    public override UIObjectRuntimeProperties FillUIObjectRuntimeProperties(
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        if (uiObjectPrefab == null)
        {
            throw new ArgumentException($"Container UIObject: {uiObjectName} does not have a valid uiObjectPrefab.");
        }
        UIObjectRuntimeProperties runtimeProperties
            = base.FillUIObjectRuntimeProperties(parentComponent, parentTransform, uiTheme, uiObjectPosition);
        FillContainerUIObjectRuntimeProperties(runtimeProperties, parentComponent, uiTheme);
        FillContainerSize(runtimeProperties, parentComponent, uiTheme);
        return runtimeProperties;
    }

    public override void RenderFromComponentManager(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        UITheme uiTheme)
    {
        FillContainerSize(uiObjectRuntimeProperties,parentComponent,uiTheme);
        base.RenderFromComponentManager(uiObjectRuntimeProperties,parentComponent,uiTheme);
    }

    // UI OBJECT CONTAINER
    // ------------------------------------------------------------------------------------------------------------------
    public virtual void FillContainerUIObjectRuntimeProperties(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        UITheme uiTheme)
    {
        // The Content is located in ScrollView->Viewport->Content which is a nested GetComponent,
        // store reference to Content here.
        Transform viewportTransform = uiObjectRuntimeProperties.uiObjectRuntime.transform.Find("Viewport");
        Debug.Log(uiObjectRuntimeProperties.uiObjectRuntime);
        uiObjectRuntimeProperties.contentGameObject = viewportTransform.Find("Content").gameObject;
        uiObjectRuntimeProperties.layoutDirection = layoutDirection;
        uiObjectRuntimeProperties.itemRuntimePropertiesList = new List<UIObjectRuntimeProperties>();

        if (itemUIObject is IUIObjectWithContainerItem uiObjectWithContainerData) {
            uiObjectRuntimeProperties.itemSize = uiObjectWithContainerData.objectSize;
        } else {
            throw new ArgumentException($"Item UIObject: {itemUIObject.uiObjectName} does not implement IUIObjectWithContainerItem.");
        }
        float objectSpacing = uiTheme.UISpacingValueFromEnum(parentComponent.uiObjectSpacing);
        int objectMargin = (int)uiTheme.UIMarginValueFromEnum(parentComponent.uiObjectMargin);

        // Fill in item spacing.
        HorizontalOrVerticalLayoutGroup layoutGroup = layoutDirection switch
        {
            UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_HORIZONTAL or
            UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_HORIZONTAL
                => uiObjectRuntimeProperties.contentGameObject.AddComponent<HorizontalLayoutGroup>(),
            _ => uiObjectRuntimeProperties.contentGameObject.AddComponent<VerticalLayoutGroup>(),
        };
        FillLayoutGroup(layoutGroup, objectSpacing);
        FillScrollDirection(uiObjectRuntimeProperties);
        // Fill in item margins.
        RectTransform viewportRectTransform = viewportTransform.gameObject.GetComponent<RectTransform>();
        viewportRectTransform.offsetMin = new Vector2(objectMargin, objectMargin);
        viewportRectTransform.offsetMax = new Vector2(-objectMargin, -objectMargin);
    }
    
    public void FillContainerSize(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                  UIComponent parentComponent, UITheme uiTheme) {
        float objectSpacing = uiTheme.UISpacingValueFromEnum(parentComponent.uiObjectSpacing);
        int objectMargin = (int)uiTheme.UIMarginValueFromEnum(parentComponent.uiObjectMargin);
        Vector2 itemSize = uiObjectRuntimeProperties.itemSize;
        UIObjectContainerLayoutDirection layoutDirection = uiObjectRuntimeProperties.layoutDirection;
        int numberItems = 0;
        foreach (UIObjectRuntimeProperties itemProperties in uiObjectRuntimeProperties.itemRuntimePropertiesList) {
            numberItems += Convert.ToInt32(itemProperties.uiObjectRuntime.gameObject.activeSelf);
        }
        switch (layoutDirection) {
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_HORIZONTAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(
                    2 * objectMargin + 2 * objectSpacing + 2.5f * itemSize.x,
                    2 * objectMargin + itemSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_VERTICAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(
                    2 * objectMargin  + itemSize.x,
                    2 * objectMargin + 2 * objectSpacing + 2.5f * itemSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_HORIZONTAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(
                    2 * objectMargin + (numberItems-1) * objectSpacing
                        + numberItems * itemSize.x,
                    2 * objectMargin  + itemSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_VERTICAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(
                    2 * objectMargin  + itemSize.x,
                    2 * objectMargin + (numberItems-1) * objectSpacing
                        + numberItems * itemSize.y);
                break;
        }
    }

    public void FillLayoutGroup(HorizontalOrVerticalLayoutGroup layoutGroup, float objectSpacing)
    {
        layoutGroup.spacing = objectSpacing;
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        // Scale container by children.
        layoutGroup.childScaleWidth = true;
        layoutGroup.childScaleHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
    }

    public void FillScrollDirection(UIObjectRuntimeProperties uiObjectRuntimeProperties)
    {
        ScrollRect scrollRect = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponent<ScrollRect>();
        switch (layoutDirection)
        {
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_HORIZONTAL:
                scrollRect.horizontal = true;
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_VERTICAL:
                scrollRect.vertical = true;
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_HORIZONTAL:
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_VERTICAL:
                scrollRect.enabled = false;
                break;
        }
    }
}
