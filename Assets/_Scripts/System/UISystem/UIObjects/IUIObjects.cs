using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// PROPERTIES
// ---------------------------------------------------------------------------------------------------------------
public class UIObjectRuntimePropertiesId
{
    public string uiComponentName;
    public string uiObjectName;
    public string uiObjectValue;
    public string Id { get { return $"{uiComponentName}/{uiObjectName}/{uiObjectValue}"; } }
}

public class UIObjectRuntimeProperties
{
    public bool isFilled = false;
    public UIObjectRuntimePropertiesId propertyId;
    public GameObject uiObjectRuntime;
    public RectTransform rectTransform;
    public System.Action<UIObjectRuntimeProperties,UIComponent,UITheme> RenderFromComponentManager;

    // Interface proeprties.
    public Vector2 objectSize { get; set; }
    public UIObjectImageColor imageColor { get; set; }
    public Sprite imageSprite { get; set; }
    public Image image { get; set; }
    public Sprite childImageSprite { get; set; }
    public Image childImage { get; set; }
    public string textContent { get; set; }
    public UIObjectTextColor textColor { get; set; }
    public Button button { get; set; }
    public System.Action<string> OnClickUIObjectDelegate { get; set; }
    public string uiObjectValue { get; set; }
    public System.Action<string,string> OnValueClickUIObjectDelegate { get; set; }
    public void OnClickUIObject() {
        OnClickUIObjectDelegate?.Invoke(propertyId.uiObjectName);
    }

    // For later... INTERFACE - IUIObjectWithContainer
    public GameObject contentGameObject { get; set; }
    public Vector2 itemSize { get; set; }
    public List<UIObjectRuntimeProperties> itemRuntimePropertiesList { get; set; }
    public UIObjectContainerLayoutDirection layoutDirection { get; set; }
}

// INTERFACES
// ---------------------------------------------------------------------------------------------------------------
public interface IUIObjectWithSize
{
    public Vector2 objectSize { get; }
    public interface IUIObjectWithSizeProperties : IUIObjectWithSize {}
}

public interface IUIObjectWithImageColor
{
    public UIObjectImageColor imageColor { get; }
    public interface IUIObjectWithImageColorProperties : IUIObjectWithImageColor {}
}

public interface IUIObjectWithImage
{
    public Sprite imageSprite { get; set; }
    public interface IUIObjectWithImageProperties : IUIObjectWithImage {
        public Image image { get; set; }
    }
}

public interface IUIObjectWithImageChild
{
    public Sprite childImageSprite { get; set; }
    public interface IUIObjectWithImageChildProperties : IUIObjectWithImageChild {
        public Image childImage { get; set; }
    }
}

public interface IUIObjectWithTextChild
{
    public string textContent { get; set; }
    public interface IUIObjectWithTextChildProperties : IUIObjectWithTextChild {
        public UIObjectTextColor textColor { get; set; }
    }
}

public interface IUIObjectWithButton
{
    public interface IUIObjectWithButtonProperties : IUIObjectWithButton {
        public Button button { get; set; }
    }
}

public interface IUIObjectWithClick
{
    public interface IUIObjectWithClickProperties : IUIObjectWithClick  {
        public System.Action<string> OnClickUIObjectDelegate { get; set; }
    }
}

public interface IUIObjectWithValue
{
    public string uiObjectValue { get; set; }
    public interface IUIObjectWithValueProperties : IUIObjectWithValue {}
}

public interface IUIObjectWithStringValueClick : IUIObjectWithButton, IUIObjectWithValue
{
    public interface IUIObjectWithStringValueClickProperties : IUIObjectWithButtonProperties, IUIObjectWithValueProperties {
        public System.Action<string,string> OnValueClickUIObjectDelegate { get; set; }
    }
}

// FILLERS
// ---------------------------------------------------------------------------------------------------------------
public static class IUIObjectFillers
{
    public static void MaybeFillUIObjectSize(UIObjectRuntimeProperties runtimeProperties,
                                             UIObject uiObject)
    {
        if (uiObject is IUIObjectWithSize uiObjectWithSize)
        {
            runtimeProperties.objectSize=uiObjectWithSize.objectSize;
            runtimeProperties.rectTransform.sizeDelta = runtimeProperties.objectSize;
        }
    }

    public static void MaybeFillUIObjectImage(UIObjectRuntimeProperties runtimeProperties,
                                              UIObject uiObject)
    {
        if (uiObject is IUIObjectWithImage uiObjectWithImage)
        {
            if (uiObjectWithImage.imageSprite == null)
            {
                throw new System.ArgumentNullException($"UIObject: {runtimeProperties.propertyId.uiObjectName} does not have an imageSprite but implements IUIObjectWithImage.");
            }
            runtimeProperties.image = runtimeProperties.uiObjectRuntime.GetComponent<Image>();
            runtimeProperties.imageSprite = uiObjectWithImage.imageSprite;
            runtimeProperties.image.sprite = runtimeProperties.imageSprite;
        }
    }

    public static void MaybeFillUIObjectImageColor(UIObjectRuntimeProperties runtimeProperties,
                                                   UIObject uiObject,
                                                   UITheme uiTheme)
    {
        if (uiObject is IUIObjectWithImageColor uiObjectWithImageColor)
        {
            if (runtimeProperties.image == null) {
                runtimeProperties.image = runtimeProperties.uiObjectRuntime.gameObject.GetComponent<Image>();
            }
            if (runtimeProperties.image == null)
            {
                // No image found on GameObject, don't fill.
                return;
            }
            runtimeProperties.imageColor = uiObjectWithImageColor.imageColor;
            runtimeProperties.image.color = UIThemeUtil.ColorFromUIObjectImageColor(runtimeProperties.imageColor, uiTheme);
        }
    }

    public static void MaybeFillUIObjectImageChild(UIObjectRuntimeProperties runtimeProperties,
                                                   UIObject uiObject)
    {
        if (uiObject is IUIObjectWithImageChild uiObjectWithImageChild)
        {
            if (runtimeProperties.childImageSprite == null) {
                if (uiObjectWithImageChild.childImageSprite == null)
                {
                    throw new System.ArgumentNullException($"UIObject: {runtimeProperties.propertyId.uiObjectName} does not have a childImageSprite but implements IUIObjectWithImageChild.");
                }
                runtimeProperties.childImageSprite = uiObjectWithImageChild.childImageSprite;
            }
            GameObject iconObjectRuntime = new GameObject(runtimeProperties.childImageSprite.name);
            iconObjectRuntime.transform.SetParent(runtimeProperties.uiObjectRuntime.transform, false);
            runtimeProperties.childImage = iconObjectRuntime.AddComponent<Image>();
            runtimeProperties.childImage.sprite = runtimeProperties.childImageSprite;
        }
    }

    public static void MaybeFillUIObjectTextChild(UIObjectRuntimeProperties runtimeProperties,
                                                  UIObject uiObject,
                                                  UITheme uiTheme)
    {
        if (uiObject is IUIObjectWithTextChild uiObjectWithTextChild)
        {
            if (runtimeProperties.textContent == null) {
                runtimeProperties.textContent = uiObjectWithTextChild.textContent;
            }
            TMP_Text uiObjectText = runtimeProperties.uiObjectRuntime.gameObject.GetComponentInChildren<TMP_Text>();
            if (uiObjectText == null)
            {
                // No Text component found on GameObject, don't fill.
                return;
            }
            uiObjectText.font = uiTheme.font;
            uiObjectText.text = runtimeProperties.textContent;
            uiObjectText.color = UIThemeUtil.ColorFromUIObjectTextColor(runtimeProperties.textColor, uiTheme);
            uiObjectText.fontSize = 32f;
        }
    }

    public static void MaybeFillUIObjectButton(UIObjectRuntimeProperties runtimeProperties,
                                               UIComponent parentComponent,
                                               UIObject uiObject)
    {
        if (uiObject is IUIObjectWithClick &&
                parentComponent.CanInterceptUIObjectWithClick())
        {
            runtimeProperties.button = runtimeProperties.uiObjectRuntime.GetComponent<Button>();
            runtimeProperties.OnClickUIObjectDelegate = parentComponent.OnClickUIObject;
            runtimeProperties.button.onClick.AddListener(
                delegate { 
                    runtimeProperties.OnClickUIObjectDelegate?.Invoke(runtimeProperties.propertyId.uiObjectName);
                });
        }
        else if (uiObject is IUIObjectWithStringValueClick uiObjectWithStringValueClick &&
                    parentComponent.CanInterceptUIObjectWithClick())
        {
            if (runtimeProperties.uiObjectValue == null) {
                runtimeProperties.uiObjectValue = uiObjectWithStringValueClick.uiObjectValue;
            }
            runtimeProperties.button = runtimeProperties.uiObjectRuntime.GetComponent<Button>();
            runtimeProperties.OnValueClickUIObjectDelegate = parentComponent.OnClickUIObject;
            runtimeProperties.button.onClick.AddListener(
                delegate { 
                    runtimeProperties.OnValueClickUIObjectDelegate(runtimeProperties.propertyId.uiObjectName,
                        runtimeProperties.uiObjectValue);
                });
        }
    }
}
