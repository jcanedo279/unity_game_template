using UnityEngine;
using UnityEngine.UI;
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
    // INTERFACE - IUIObjectWithSize
    public Vector2 objectSize { get; }

    // INTERFACE - IUIObjectWithImage
    public Sprite imageSprite { get; }
    public Image image { get; set; }

    // INTERFACE - IUIObjectWithImageColor
    public UIObjectImageColor imageColor { get; }

    // INTERFACE - IUIObjectWithStringValueClick
    public Button button { get; set; }
    public string uiObjectValue { get; }
    public System.Action<string,string> OnValueClickUIObjectDelegate { get; set; }
}


public abstract class UIObjectContainer<TContainerItem> : UIObject
{
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] protected UIObject itemUIObject;
    [SerializeField] public List<TContainerItem> containerItemData = new List<TContainerItem>();
    [SerializeField] protected List<UIObjectRuntimeProperties> uiObjectRuntimePropertiesList;
    [SerializeField] private UIObjectContainerLayoutDirection layoutDirection;


    public override void FillFromComponentManager(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                  UIComponent parentComponent,
                                                  Transform parentTransform,
                                                  UITheme uiTheme,
                                                  Vector2 uiObjectPosition)
    {
        FillContainerBase(uiObjectRuntimeProperties, parentComponent, parentTransform, uiTheme, uiObjectPosition);
        uiObjectRuntimeProperties.isFilled = true;
    }

    public void FillContainerBase(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                  UIComponent parentComponent,
                                  Transform parentTransform,
                                  UITheme uiTheme,
                                  Vector2 uiObjectPosition) {
        if (containerPrefab == null)
        {
            throw new System.ArgumentException($"Container UIObject: {uiObjectName} does not have a valid containerPrefab.");
        }
        uiObjectRuntimeProperties.uiObjectRuntime = Instantiate(containerPrefab, parentTransform);
        if (itemUIObject == null || !itemUIObject is IUIObjectWithContainerItem) {
            throw new System.ArgumentException($"Item UIObject: {itemUIObject.uiObjectName} does not implement IUIObjectWithContainerItem.");
        }
        // The Content is located in ScrollView->Viewport->Content which is a nested GetComponent,
        // store reference to Content here.
        Transform viewportTransform = uiObjectRuntimeProperties.uiObjectRuntime.transform.Find("Viewport");
        uiObjectRuntimeProperties.contentGameObject = viewportTransform.Find("Content").gameObject;

        float objectSpacing = uiTheme.UISpacingValueFromEnum(parentComponent.uiObjectSpacing);
        int objectMargin = (int)uiTheme.UIMarginValueFromEnum(parentComponent.uiObjectMargin);

        // Fill in container transform (including size).
        uiObjectRuntimeProperties.rectTransform = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<RectTransform>();
        uiObjectRuntimeProperties.rectTransform.localPosition = uiObjectPosition;
        FillContainerSize(uiObjectRuntimeProperties, objectSpacing,objectMargin);
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
                                  float objectSpacing, int objectMargin) {
        Vector2 itemSize = Vector2.zero;
        if (itemUIObject is IUIObjectWithContainerItem uiObjectWithContainerData) {
            itemSize = uiObjectWithContainerData.objectSize;
        } else {
            return;
        }
        int numberItems = containerItemData.Count;
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
