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

[CreateAssetMenu(fileName = "UIObjectContainer", menuName = "UI System/UI Objects/UI Object Container")]
public class UIObjectContainer : UIObject
{
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private List<UIObjectStringButton> items;
    [SerializeField] private UIObjectContainerLayoutDirection layoutDirection;


    public override void FillFromComponentManager(UIComponent parentComponent,
                      Transform parentTransform,
                      UITheme uiTheme,
                      Vector2 uiObjectPosition)
    {
        if (containerPrefab == null)
        {
            return;
        }
        isFilled = false;
        uiObjectRuntime = Instantiate(containerPrefab, parentTransform);
        // Container-specific logic.
        FillContainer(parentComponent, uiTheme, uiObjectPosition);
        isFilled = true;
    }

    public void FillContainer(UIComponent parentComponent,
                              UITheme uiTheme,
                              Vector2 uiObjectPosition)
    {
        if (items == null)
        {
            return;
        }
        // The Content is located in ScrollView->Viewport->Content which is a nested GetComponent,
        // store reference to Content here.
        Transform viewportTransform = uiObjectRuntime.transform.Find("Viewport");
        GameObject contentGameObject = viewportTransform.Find("Content").gameObject;

        float objectSpacing = uiTheme.UISpacingValueFromEnum(parentComponent.uiObjectSpacing);
        int objectMargin = (int)uiTheme.UIMarginValueFromEnum(parentComponent.uiObjectMargin);
        FillLayoutDirection();

        // Fill in container transform (including size).
        rectTransform = uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = uiObjectPosition;
        switch (layoutDirection) {
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_HORIZONTAL:
                rectTransform.sizeDelta = new Vector2(2 * objectMargin + 2 * objectSpacing + 2.5f * items[0].buttonSize.x,
                                              2 * objectMargin + items[0].buttonSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_VERTICAL:
                rectTransform.sizeDelta = new Vector2(2 * objectMargin  + items[0].buttonSize.x,
                                                      2 * objectMargin + 2 * objectSpacing + 2.5f * items[0].buttonSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_HORIZONTAL:
                rectTransform.sizeDelta = new Vector2(2 * objectMargin + (items.Count-1) * objectSpacing
                                                        + items.Count * items[0].buttonSize.x,
                                                      2 * objectMargin  + items[0].buttonSize.y);
                break;
            case UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_VERTICAL:
                rectTransform.sizeDelta = new Vector2(2 * objectMargin  + items[0].buttonSize.x,
                                                      2 * objectMargin + (items.Count-1) * objectSpacing
                                                        + items.Count * items[0].buttonSize.y);
                break;
        }
        // Fill in item spacing.
        HorizontalOrVerticalLayoutGroup layoutGroup = layoutDirection switch
        {
            UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_SCROLL_HORIZONTAL or
            UIObjectContainerLayoutDirection.LAYOUT_DIRECTION_HORIZONTAL
                => contentGameObject.AddComponent<HorizontalLayoutGroup>(),
            _ => contentGameObject.AddComponent<VerticalLayoutGroup>(),
        };
        FillLayoutGroup(layoutGroup, objectSpacing);
        // Fill in item margins.
        RectTransform viewportRectTransform = viewportTransform.gameObject.GetComponent<RectTransform>();
        viewportRectTransform.offsetMin = new Vector2(objectMargin, objectMargin);
        viewportRectTransform.offsetMax = new Vector2(-objectMargin, -objectMargin);

        // Fill in each item.
        foreach (UIObjectStringButton itemObject in items)
        {
            itemObject.FillFromComponentManager(parentComponent, contentGameObject.transform, uiTheme, uiObjectPosition);
        }
    }

    public void FillLayoutDirection()
    {
        ScrollRect scrollRect = uiObjectRuntime.gameObject.GetComponent<ScrollRect>();
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

    public void FillLayoutGroup(HorizontalOrVerticalLayoutGroup layoutGroup, float objectSpacing)
    {
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childScaleWidth = true;
        layoutGroup.childScaleHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.spacing = objectSpacing;
    }
}

// TODO: Create extension which contains description and title.

