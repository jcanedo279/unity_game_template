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

[CreateAssetMenu(fileName = "UIObjectContainer", menuName = "UI System/UI Objects/UI Containers/UI Object Container")]
public class UIObjectContainer : UIObject
{
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private List<UIObjectStringButton> items;
    [SerializeField] private List<UIObjectRuntimeProperties> uiObjectRuntimePropertiesList;
    [SerializeField] private UIObjectContainerLayoutDirection layoutDirection;


    public override void FillFromComponentManager(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                  UIComponent parentComponent,
                                                  Transform parentTransform,
                                                  UITheme uiTheme,
                                                  Vector2 uiObjectPosition)
    {
        if (containerPrefab == null)
        {
            return;
        }
        uiObjectRuntimeProperties.uiObjectRuntime = Instantiate(containerPrefab, parentTransform);
        FillContainer(uiObjectRuntimeProperties, parentComponent, uiTheme, uiObjectPosition);
        uiObjectRuntimeProperties.isFilled = true;
    }

    public void FillContainer(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                              UIComponent parentComponent,
                              UITheme uiTheme,
                              Vector2 uiObjectPosition)
    {
        if (items == null)
        {
            return;
        }
        // The Content is located in ScrollView->Viewport->Content which is a nested GetComponent,
        // store reference to Content here.
        Transform viewportTransform = uiObjectRuntimeProperties.uiObjectRuntime.transform.Find("Viewport");
        GameObject contentGameObject = viewportTransform.Find("Content").gameObject;

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
                => contentGameObject.AddComponent<HorizontalLayoutGroup>(),
            _ => contentGameObject.AddComponent<VerticalLayoutGroup>(),
        };
        FillLayoutGroup(layoutGroup, objectSpacing);
        FillScrollDirection(uiObjectRuntimeProperties);
        // Fill in item margins.
        RectTransform viewportRectTransform = viewportTransform.gameObject.GetComponent<RectTransform>();
        viewportRectTransform.offsetMin = new Vector2(objectMargin, objectMargin);
        viewportRectTransform.offsetMax = new Vector2(-objectMargin, -objectMargin);

        // Fill in each item.
        uiObjectRuntimePropertiesList = new List<UIObjectRuntimeProperties>();
        foreach (UIObjectStringButton itemObject in items)
        {
            UIObjectRuntimeProperties itemUIObjectRuntimeProperties = new UIObjectRuntimeProperties();
            itemObject.FillFromComponentManager(itemUIObjectRuntimeProperties,
                                                parentComponent, contentGameObject.transform,
                                                uiTheme, uiObjectPosition);
            uiObjectRuntimePropertiesList.Add(itemUIObjectRuntimeProperties);
        }
    }

    public void FillContainerSize(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                  float objectSpacing, int objectMargin) {
        switch (layoutDirection) {
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_HORIZONTAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(2 * objectMargin + 2 * objectSpacing + 2.5f * items[0].objectSize.x,
                                              2 * objectMargin + items[0].objectSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_VERTICAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(2 * objectMargin  + items[0].objectSize.x,
                                                      2 * objectMargin + 2 * objectSpacing + 2.5f * items[0].objectSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_HORIZONTAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(2 * objectMargin + (items.Count-1) * objectSpacing
                                                        + items.Count * items[0].objectSize.x,
                                                      2 * objectMargin  + items[0].objectSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_VERTICAL:
                uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(2 * objectMargin  + items[0].objectSize.x,
                                                      2 * objectMargin + (items.Count-1) * objectSpacing
                                                        + items.Count * items[0].objectSize.y);
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
