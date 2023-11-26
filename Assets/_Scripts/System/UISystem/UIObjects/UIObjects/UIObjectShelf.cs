using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "UIObjectShelf", menuName = "UI System/UI Objects/UI Containers/UI Object Shelf")]
public class UIObjectShelf : UIObject {
    [SerializeField] public UIObjectButton uiObjectTab;
    [SerializeField] public float tabLength = 200f;
    [SerializeField] public UIObjectStringButtonContainer uiObjectContainer;
    [System.NonSerialized] private UIObjectRuntimeProperties uiObjectShelfRuntimeProperties;
    [System.NonSerialized] private UIObjectRuntimeProperties uiObjectDescriptionRuntimeProperties;
    [System.NonSerialized] private UIObjectRuntimeProperties uiObjectContainerRuntimeProperties;
    [System.NonSerialized] private GameObject descriptionTextObject;
     [SerializeField] private List<string> itemContentList;
    [SerializeField] private string descriptionContent = "Some description content.";
    [SerializeField] private UIObjectTextColor tabFontColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;
    [SerializeField] private UIObjectTextColor descriptionFontColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;

    public override Dictionary<string,UIObjectRuntimeProperties> FillFromComponentManager(
        UIObjectRuntimeProperties uiObjectRuntimeProperties,
        UIComponent parentComponent,
        Transform parentTransform,
        UITheme uiTheme,
        Vector2 uiObjectPosition)
    {
        Dictionary<string,UIObjectRuntimeProperties> objectPropertyIdToProperty
            = new Dictionary<string, UIObjectRuntimeProperties>();
        uiObjectRuntimeProperties.uiObjectRuntime = new GameObject(uiObjectName);
        uiObjectRuntimeProperties.uiObjectRuntime.transform.SetParent(parentTransform, false);
        uiObjectRuntimeProperties.rectTransform = uiObjectRuntimeProperties.uiObjectRuntime.AddComponent<RectTransform>();
        uiObjectRuntimeProperties.rectTransform.localPosition = uiObjectPosition;

        // Fill in Container.
        uiObjectContainerRuntimeProperties = new UIObjectRuntimeProperties
        {
            propertyId = new UIObjectRuntimePropertiesId
            {
                uiComponentName = parentComponent.uiComponentName, uiObjectName = uiObjectName, uiObjectValue = "Container"
            }
        };
        uiObjectContainer.containerItemData = itemContentList;
        uiObjectContainer.FillFromComponentManager(uiObjectContainerRuntimeProperties,
                                                   parentComponent,parentTransform,
                                                   uiTheme,uiObjectPosition)
                         .ToList().ForEach(propertyMap => objectPropertyIdToProperty[propertyMap.Key] = propertyMap.Value);
        Vector2 containerSize = uiObjectContainerRuntimeProperties.rectTransform.sizeDelta;

        // Fill in Shelf tab.
        uiObjectShelfRuntimeProperties = new UIObjectRuntimeProperties
        {
            propertyId = new UIObjectRuntimePropertiesId
            {
                uiComponentName = parentComponent.uiComponentName, uiObjectName = uiObjectName, uiObjectValue = "ShelfTab"
            }
        };
        uiObjectTab.FillFromComponentManager(uiObjectShelfRuntimeProperties,
                                                  parentComponent,parentTransform,
                                                  uiTheme,uiObjectPosition);
        uiObjectShelfRuntimeProperties.clickProperties.button.onClick.AddListener(OnClickShelfTab);
        uiObjectShelfRuntimeProperties.rectTransform.sizeDelta = new Vector2(tabLength,50f);
        uiObjectShelfRuntimeProperties.rectTransform.localPosition =
            new Vector3(uiObjectContainerRuntimeProperties.rectTransform.localPosition.x,
                        uiObjectContainerRuntimeProperties.rectTransform.localPosition.y)
            + new Vector3(-uiObjectContainerRuntimeProperties.rectTransform.sizeDelta.x/2f
                            +uiObjectShelfRuntimeProperties.rectTransform.sizeDelta.x/2f+(6*3f),
                          uiObjectContainerRuntimeProperties.rectTransform.sizeDelta.y/2f
                            +uiObjectShelfRuntimeProperties.rectTransform.sizeDelta.y/2f-(1*3f));

        // Fill in Description tab.
        uiObjectDescriptionRuntimeProperties = new UIObjectRuntimeProperties
        {
            propertyId = new UIObjectRuntimePropertiesId
            {
                uiComponentName = parentComponent.uiComponentName, uiObjectName = uiObjectName, uiObjectValue = "DescriptionTab"
            }
        };
        uiObjectTab.FillFromComponentManager(uiObjectDescriptionRuntimeProperties,
                                                        parentComponent,parentTransform,
                                                        uiTheme,uiObjectPosition);
        uiObjectDescriptionRuntimeProperties.clickProperties.button.onClick.AddListener(OnClickDescriptionTab);
        uiObjectDescriptionRuntimeProperties.rectTransform.sizeDelta = new Vector2(275f,50f);
        uiObjectDescriptionRuntimeProperties.rectTransform.localPosition =
            uiObjectShelfRuntimeProperties.rectTransform.localPosition
            + new Vector3(uiObjectShelfRuntimeProperties.rectTransform.sizeDelta.x/2f
                            +uiObjectDescriptionRuntimeProperties.rectTransform.sizeDelta.x/2f,0f);


        // Fill in Shelf tab text.
        GameObject shelfTabTextObject = new GameObject($"{uiObjectTab.uiObjectName}/{uiObjectName}");
        shelfTabTextObject.transform.SetParent(uiObjectShelfRuntimeProperties.uiObjectRuntime.transform,false);
        FillInText(shelfTabTextObject, uiTheme, uiObjectName, 32f, tabFontColor, enableTextWrapping:false);
    
        // Fill in Description tab text.
        GameObject descriptionTabTextObject = new GameObject($"{uiObjectTab.uiObjectName}/DescriptionTab");
        descriptionTabTextObject.transform.SetParent(uiObjectDescriptionRuntimeProperties.uiObjectRuntime.transform,false);
        FillInText(descriptionTabTextObject, uiTheme, "Description", 32f, tabFontColor);

        // Fill in Description text.
        descriptionTextObject = new GameObject($"{uiObjectTab.uiObjectName}/Description");
        descriptionTextObject.transform.SetParent(uiObjectContainerRuntimeProperties.uiObjectRuntime.transform,false);
        descriptionTextObject.SetActive(false);
        FillInText(descriptionTextObject, uiTheme, $"{uiObjectName}: {descriptionContent}", 24f, descriptionFontColor);

        uiObjectRuntimeProperties.isFilled = true;
        objectPropertyIdToProperty.Add(uiObjectShelfRuntimeProperties.propertyId.Id,uiObjectShelfRuntimeProperties);
        objectPropertyIdToProperty.Add(uiObjectDescriptionRuntimeProperties.propertyId.Id,uiObjectDescriptionRuntimeProperties);
        return objectPropertyIdToProperty;
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

    void OnClickShelfTab() {
        descriptionTextObject.SetActive(false);
        uiObjectContainerRuntimeProperties.containerProperties.contentGameObject.SetActive(true);
    }
    void OnClickDescriptionTab() {
        uiObjectContainerRuntimeProperties.containerProperties.contentGameObject.SetActive(false);
        descriptionTextObject.SetActive(true);
    }
}
