using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "UIObjectShelf", menuName = "UI System/UI Objects/UI Containers/UI Object Shelf")]
public class UIObjectShelf : UIObject {
    [SerializeField] public UIObjectButton uiObjectTab;
    [SerializeField] public float tabLength = 200f;
    [SerializeField] public UIObjectStringButtonContainer uiObjectContainer;
    [System.NonSerialized] private GameObject descriptionTextObject;
     [SerializeField] private List<string> itemContentList;
    [SerializeField] private string descriptionContent = "Some description content.";
    [SerializeField] private UIObjectTextColor tabFontColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;
    [SerializeField] private UIObjectTextColor descriptionFontColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;

    public override UIObjectRuntimeProperties FillUIObjectRuntimeProperties(
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        UIObjectRuntimeProperties runtimeProperties
            = base.FillUIObjectRuntimeProperties(parentComponent, parentTransform, uiTheme, uiObjectPosition);
        return runtimeProperties;
    }

    public override Dictionary<string,UIObjectRuntimeProperties> FillChildUIObjectRuntimeProperties(
        UIObjectRuntimeProperties runtimeProperties,
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme)
    {
        Dictionary<string, UIObjectRuntimeProperties> childRuntimeProperties
            = base.FillChildUIObjectRuntimeProperties(runtimeProperties, parentComponent, parentTransform, uiTheme);

        // Fill container item.
        uiObjectContainer.uiObjectValue = "Container";
        uiObjectContainer.containerItemData = itemContentList;
        uiObjectContainer.FillFromComponentManager(parentComponent,parentTransform,
                                                   uiTheme,Vector3.zero)
                         .ToList().ForEach(propertyMap => childRuntimeProperties[propertyMap.Key] = propertyMap.Value);
        foreach (string props in childRuntimeProperties.Keys) {
            Debug.Log(props);
        }
        UIObjectRuntimeProperties containerRuntimeProperties = childRuntimeProperties[
            new UIObjectRuntimePropertiesId {uiComponentName=parentComponent.uiComponentName,
                                             uiObjectName=uiObjectContainer.uiObjectName,
                                             uiObjectValue=uiObjectContainer.uiObjectValue}.Id
        ];

        // Fill shelf tab.
        uiObjectTab.uiObjectValue = "ShelfTab";
        uiObjectTab.FillFromComponentManager(parentComponent,parentTransform,uiTheme,Vector3.zero)
                   .ToList().ForEach(propertyMap => childRuntimeProperties[propertyMap.Key] = propertyMap.Value);
        UIObjectRuntimeProperties shelfTabRuntimeProperties = childRuntimeProperties[
            new UIObjectRuntimePropertiesId {uiComponentName=parentComponent.uiComponentName,
                                             uiObjectName=uiObjectTab.uiObjectName,
                                             uiObjectValue=uiObjectTab.uiObjectValue}.Id
        ];
        shelfTabRuntimeProperties.button.onClick.AddListener(
            delegate { OnClickShelfTab(containerRuntimeProperties.contentGameObject); });
        shelfTabRuntimeProperties.rectTransform.sizeDelta = new Vector2(tabLength,50f);
        shelfTabRuntimeProperties.rectTransform.localPosition =
            new Vector3(containerRuntimeProperties.rectTransform.localPosition.x,
                        containerRuntimeProperties.rectTransform.localPosition.y)
            + new Vector3(-containerRuntimeProperties.rectTransform.sizeDelta.x/2f
                            +shelfTabRuntimeProperties.rectTransform.sizeDelta.x/2f+(6*3f),
                          containerRuntimeProperties.rectTransform.sizeDelta.y/2f
                            +shelfTabRuntimeProperties.rectTransform.sizeDelta.y/2f-(1*3f));

        // Fill description tab.
        uiObjectTab.uiObjectValue = "DescriptionTab";
        uiObjectTab.FillFromComponentManager(parentComponent,parentTransform,uiTheme,Vector3.zero)
                   .ToList().ForEach(propertyMap => childRuntimeProperties[propertyMap.Key] = propertyMap.Value);
        UIObjectRuntimeProperties descriptionTabRuntimeProperties = childRuntimeProperties[
            new UIObjectRuntimePropertiesId {uiComponentName=parentComponent.uiComponentName,
                                             uiObjectName=uiObjectTab.uiObjectName,
                                             uiObjectValue=uiObjectTab.uiObjectValue}.Id
        ];
        descriptionTabRuntimeProperties.button.onClick.AddListener(
            delegate { OnClickDescriptionTab(containerRuntimeProperties.contentGameObject); });
        descriptionTabRuntimeProperties.rectTransform.sizeDelta = new Vector2(275f,50f);
        descriptionTabRuntimeProperties.rectTransform.localPosition =
            shelfTabRuntimeProperties.rectTransform.localPosition
            + new Vector3(shelfTabRuntimeProperties.rectTransform.sizeDelta.x/2f
                            +descriptionTabRuntimeProperties.rectTransform.sizeDelta.x/2f,0f);

        // Fill in Shelf tab text.
        GameObject shelfTabTextObject = new GameObject($"{uiObjectTab.uiObjectName}/{uiObjectName}");
        shelfTabTextObject.transform.SetParent(shelfTabRuntimeProperties.uiObjectRuntime.transform,false);
        FillInText(shelfTabTextObject, uiTheme, uiObjectName, 32f, tabFontColor, enableTextWrapping:false);
    
        // Fill in Description tab text.
        GameObject descriptionTabTextObject = new GameObject($"{uiObjectTab.uiObjectName}/DescriptionTab");
        descriptionTabTextObject.transform.SetParent(descriptionTabRuntimeProperties.uiObjectRuntime.transform,false);
        FillInText(descriptionTabTextObject, uiTheme, "Description", 32f, tabFontColor);

        // Fill in Description text.
        descriptionTextObject = new GameObject($"{uiObjectTab.uiObjectName}/Description");
        descriptionTextObject.transform.SetParent(containerRuntimeProperties.uiObjectRuntime.transform,false);
        descriptionTextObject.SetActive(false);
        FillInText(descriptionTextObject, uiTheme, $"{uiObjectName}: {descriptionContent}", 24f, descriptionFontColor);

        return childRuntimeProperties;
    }

    void FillInText(GameObject objectWithText, UITheme uiTheme, string textContent, float fontSize, UIObjectTextColor fontColor,
                    bool enableTextWrapping=true) {
        TMP_Text textComponent = objectWithText.AddComponent<TextMeshProUGUI>();
        textComponent.text = textContent;
        textComponent.font = uiTheme.font;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = UIThemeUtil.ColorFromUIObjectTextColor(fontColor,uiTheme);
        textComponent.fontSize = fontSize;
        if (enableTextWrapping == false) {
            textComponent.enableWordWrapping = false;
        }
    }

    void OnClickShelfTab(GameObject contentGameObject) {
        descriptionTextObject.SetActive(false);
        contentGameObject.SetActive(true);
    }
    void OnClickDescriptionTab(GameObject contentGameObject) {
        contentGameObject.SetActive(false);
        descriptionTextObject.SetActive(true);
    }
}
