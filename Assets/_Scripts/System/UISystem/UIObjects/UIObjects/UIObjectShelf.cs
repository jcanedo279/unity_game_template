using TMPro;
using UnityEngine;


[CreateAssetMenu(fileName = "UIObjectShelf", menuName = "UI System/UI Objects/UI Containers/UI Object Shelf")]
public class UIObjectShelf : UIObject {
    [SerializeField] public UIObjectImage uiObjectShelfTab;
    // [SerializeField] public UIObjectImage uiObjectDescriptionTab;
    [SerializeField] public UIObjectContainer uiObjectContainer;
    [SerializeField] private string descriptionContent = "Some description content.";
    [SerializeField] private UIObjectTextColor tabFontColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;

    public override void FillFromComponentManager(UIComponent parentComponent,
                                                  Transform parentTransform,
                                                  UITheme uiTheme,
                                                  Vector2 uiObjectPosition)
    {
        uiObjectRuntimeProperties = new UIObjectRuntimeProperties
        {
            uiObjectRuntime = new GameObject(uiObjectName)
        };
        uiObjectRuntimeProperties.uiObjectRuntime.transform.SetParent(parentTransform, false);
        uiObjectRuntimeProperties.rectTransform = uiObjectRuntimeProperties.uiObjectRuntime.AddComponent<RectTransform>();
        uiObjectRuntimeProperties.rectTransform.localPosition = uiObjectPosition;

        uiObjectShelfTab.FillFromComponentManager(parentComponent,parentTransform,
                                                  uiTheme,uiObjectPosition);
        // uiObjectDescriptionTab.FillFromComponentManager(parentComponent,parentTransform,
        //                                                 uiTheme,uiObjectPosition);

        uiObjectContainer.FillFromComponentManager(parentComponent,parentTransform,
                                                   uiTheme,uiObjectPosition);
        uiObjectShelfTab.uiObjectRuntimeProperties.rectTransform.sizeDelta = new Vector2(200f,70f);
        // uiObjectDescriptionTab.rectTransform.sizeDelta = new Vector2(300f,70f);
        uiObjectShelfTab.uiObjectRuntimeProperties.rectTransform.localPosition =
            new Vector3(uiObjectContainer.uiObjectRuntimeProperties.rectTransform.localPosition.x,
                        uiObjectContainer.uiObjectRuntimeProperties.rectTransform.localPosition.y)
            + new Vector3(-uiObjectContainer.uiObjectRuntimeProperties.rectTransform.sizeDelta.x/2f
                            +uiObjectShelfTab.uiObjectRuntimeProperties.rectTransform.sizeDelta.x/2f+(6*3f),
                          uiObjectContainer.uiObjectRuntimeProperties.rectTransform.sizeDelta.y/2f
                            +uiObjectShelfTab.uiObjectRuntimeProperties.rectTransform.sizeDelta.y/2f-(1*3f));
        // uiObjectDescriptionTab.rectTransform.localPosition =
        //     new Vector3(uiObjectShelfTab.rectTransform.localPosition.x,
        //                 uiObjectShelfTab.rectTransform.localPosition.y)
        //     + new Vector3(uiObjectShelfTab.rectTransform.sizeDelta.x/2f+uiObjectDescriptionTab.rectTransform.sizeDelta.x/2f,0f);

        GameObject tabTextObject = new GameObject($"{uiObjectShelfTab.uiObjectName}/{uiObjectName}");
        tabTextObject.transform.SetParent(uiObjectShelfTab.uiObjectRuntimeProperties.uiObjectRuntime.transform,false);

        // Fill in tab text.
        TMP_Text tabTextComponent = tabTextObject.AddComponent<TextMeshProUGUI>();
        tabTextComponent.text = uiObjectName;
        tabTextComponent.font = uiTheme.font;
        tabTextComponent.alignment = TextAlignmentOptions.Center;
        tabTextComponent.color = UIThemeUtil.ColorFromUIObjectTextColor(tabFontColor,uiTheme);
        tabTextComponent.fontSize = 32f;

        uiObjectRuntimeProperties.isFilled = true;
    }
}
