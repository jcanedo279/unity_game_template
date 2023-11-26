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

    // INTERFACE - IUIObjectWithSize
    public IUIObjectWithSize.IUIObjectWithSizeProperties sizeProperties;

    // INTERFACE - IUIObjectWithImage
    public IUIObjectWithImage.IUIObjectWithImageProperties imageProperties;

    // INTERFACE - IUIObjectWithImageColor
    public IUIObjectWithImageColor.IUIObjectWithImageColorProperties imageColorProperties;

    // INTERFACE - IUIObjectWithImageChild
    public IUIObjectWithImageChild.IUIObjectWithImageChildProperties imageChildProperties;

    // INTERFACE - IUIObjectWithTextChild
    public IUIObjectWithTextChild.IUIObjectWithTextChildProperties textChildProperties;

    // INTERFACE - IUIObjectWithClick
    public IUIObjectWithClick.IUIObjectWithClickProperties clickProperties;
    public void OnClickUIObject() {
        clickProperties.OnClickUIObjectDelegate?.Invoke(propertyId.uiObjectName);
    }

    // INTERFACE - IUIObjectWithStringValueClick
    public IUIObjectWithStringValueClick.IUIObjectWithStringValueClickProperties stringValueClickProperties;

    // INTERFACE - IUIObjectWithContainer
    public class IUIObjectWithContainerProperties {
        public GameObject contentGameObject { get; set; }
        public Vector2 itemSize { get; set; }
        public List<UIObjectRuntimeProperties> itemRuntimePropertiesList { get; set; }
        public UIObjectContainerLayoutDirection layoutDirection { get; set; }
    }
    public IUIObjectWithContainerProperties containerProperties;
}

// INTERFACES
// ---------------------------------------------------------------------------------------------------------------
public interface IUIObjectWithSize
{
    public class IUIObjectWithSizeProperties {
        public Vector2 objectSize { get; set; }
    }
    public Vector2 objectSize { get; }
}

public interface IUIObjectWithImageColor
{
    public class IUIObjectWithImageColorProperties {
        public UIObjectImageColor imageColor { get; set; }
    }
    public UIObjectImageColor imageColor { get; }
}

public interface IUIObjectWithImage
{
    public class IUIObjectWithImageProperties {
        public Sprite imageSprite { get; set; }
        public Image image { get; set; }
    }
    public Sprite imageSprite { get; set; }
}

public interface IUIObjectWithImageChild
{
    public class IUIObjectWithImageChildProperties {
        public Sprite childImageSprite { get; set; }
        public Image childImage { get; set; }
    }
    public Sprite childImageSprite { get; set; }
}

public interface IUIObjectWithTextChild
{
    public class IUIObjectWithTextChildProperties {
        public string textContent { get; set; }
        public UIObjectTextColor textColor { get; set; }
    }
    public string textContent { get; set; }
    public UIObjectTextColor textColor { get; }
}

public interface IUIObjectWithButton
{
    public class IUIObjectWithButtonProperties {
        public Button button { get; set; }
    }
}

public interface IUIObjectWithClick : IUIObjectWithButton
{
    public class IUIObjectWithClickProperties : IUIObjectWithButtonProperties {
        public System.Action<string> OnClickUIObjectDelegate { get; set; }
    }
}

public interface IUIObjectWithStringValueClick : IUIObjectWithButton
{
    public class IUIObjectWithStringValueClickProperties : IUIObjectWithButtonProperties {
        public System.Action<string,string> OnValueClickUIObjectDelegate { get; set; }
        public string uiObjectValue { get; set; }
    }
    public string uiObjectValue { get; }
}

// FILLERS
// ---------------------------------------------------------------------------------------------------------------
public static class IUIObjectFillers
{
    public static void MaybeFillUIObjectSize(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                             UIObject uiObject)
    {
        if (uiObject is IUIObjectWithSize uiObjectWithSize)
        {
            uiObjectRuntimeProperties.sizeProperties = new IUIObjectWithSize.IUIObjectWithSizeProperties {
                objectSize=uiObjectWithSize.objectSize
            };
            uiObjectRuntimeProperties.rectTransform.sizeDelta = uiObjectRuntimeProperties.sizeProperties.objectSize;
        }
    }

    public static void MaybeFillUIObjectImage(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                              UIObject uiObject)
    {
        if (uiObject is IUIObjectWithImage uiObjectWithImage)
        {
            if (uiObjectWithImage.imageSprite == null)
            {
                throw new System.ArgumentNullException($"UIObject: {uiObjectRuntimeProperties.propertyId.uiObjectName} does not have an imageSprite but implements IUIObjectWithImage.");
            }
            uiObjectRuntimeProperties.imageProperties = new IUIObjectWithImage.IUIObjectWithImageProperties {
                image = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<Image>(),
                imageSprite = uiObjectWithImage.imageSprite
            };
            uiObjectRuntimeProperties.imageProperties.image.sprite = uiObjectRuntimeProperties.imageProperties.imageSprite;
        }
    }

    public static void MaybeFillUIObjectImageColor(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                   UIObject uiObject,
                                                   UITheme uiTheme)
    {
        if (uiObject is IUIObjectWithImageColor uiObjectWithImageColor)
        {
            Image uiObjectImage;
            if (uiObjectRuntimeProperties.imageProperties != null) {
                uiObjectImage = uiObjectRuntimeProperties.imageProperties.image;
            } else {
                uiObjectImage = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponent<Image>();
            }
            if (uiObjectImage == null)
            {
                // No image found on GameObject, don't fill.
                return;
            }
            uiObjectRuntimeProperties.imageColorProperties = new IUIObjectWithImageColor.IUIObjectWithImageColorProperties {
                imageColor = uiObjectWithImageColor.imageColor
            };
            uiObjectImage.color = UIThemeUtil.ColorFromUIObjectImageColor(uiObjectRuntimeProperties.imageColorProperties.imageColor, uiTheme);
        }
    }

    public static void MaybeFillUIObjectImageChild(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                   UIObject uiObject)
    {
        if (uiObject is IUIObjectWithImageChild uiObjectWithImageChild)
        {
            if (uiObjectRuntimeProperties.imageChildProperties == null) {
                if (uiObjectWithImageChild.childImageSprite == null)
                {
                    throw new System.ArgumentNullException($"UIObject: {uiObjectRuntimeProperties.propertyId.uiObjectName} does not have a childImageSprite but implements IUIObjectWithImageChild.");
                }
                uiObjectRuntimeProperties.imageChildProperties = new IUIObjectWithImageChild.IUIObjectWithImageChildProperties {
                    childImageSprite = uiObjectWithImageChild.childImageSprite
                };
            }
            GameObject iconObjectRuntime = new GameObject(uiObjectRuntimeProperties.imageChildProperties.childImageSprite.name);
            iconObjectRuntime.transform.SetParent(uiObjectRuntimeProperties.uiObjectRuntime.transform, false);
            uiObjectRuntimeProperties.imageChildProperties.childImage = iconObjectRuntime.AddComponent<Image>();
            uiObjectRuntimeProperties.imageChildProperties.childImage.sprite = uiObjectRuntimeProperties.imageChildProperties.childImageSprite;
        }
    }

    public static void MaybeFillUIObjectTextChild(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                  UIObject uiObject,
                                                  UITheme uiTheme)
    {
        if (uiObject is IUIObjectWithTextChild uiObjectWithTextChild)
        {
            if (uiObjectRuntimeProperties.textChildProperties == null) {
                uiObjectRuntimeProperties.textChildProperties = new IUIObjectWithTextChild.IUIObjectWithTextChildProperties {
                    textContent = uiObjectWithTextChild.textContent,
                    textColor = uiObjectWithTextChild.textColor
                };
            }
            TMP_Text uiObjectText = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponentInChildren<TMP_Text>();
            if (uiObjectText == null)
            {
                // No Text component found on GameObject, don't fill.
                return;
            }
            uiObjectText.font = uiTheme.font;
            uiObjectText.text = uiObjectRuntimeProperties.textChildProperties.textContent;
            uiObjectText.color = UIThemeUtil.ColorFromUIObjectTextColor(uiObjectRuntimeProperties.textChildProperties.textColor, uiTheme);
            uiObjectText.fontSize = 32f;
        }
    }

    public static void MaybeFillUIObjectButton(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                               UIComponent parentComponent,
                                               UIObject uiObject)
    {
        if (uiObject is IUIObjectWithClick &&
                parentComponent.CanInterceptUIObjectWithClick())
        {
            uiObjectRuntimeProperties.clickProperties = new IUIObjectWithClick.IUIObjectWithClickProperties {
                button = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<Button>(),
                OnClickUIObjectDelegate = parentComponent.OnClickUIObject
            };
            uiObjectRuntimeProperties.clickProperties.button.onClick.AddListener(
                delegate { 
                    uiObjectRuntimeProperties.clickProperties.OnClickUIObjectDelegate?.Invoke(uiObjectRuntimeProperties.propertyId.uiObjectName);
                });
        }
        else if (uiObject is IUIObjectWithStringValueClick uiObjectWithStringValueClick &&
                    parentComponent.CanInterceptUIObjectWithClick())
        {
            if (uiObjectRuntimeProperties.stringValueClickProperties == null) {
                uiObjectRuntimeProperties.stringValueClickProperties = new IUIObjectWithStringValueClick.IUIObjectWithStringValueClickProperties {
                    uiObjectValue = uiObjectWithStringValueClick.uiObjectValue,
                };
            }
            uiObjectRuntimeProperties.stringValueClickProperties.button = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<Button>();
            uiObjectRuntimeProperties.stringValueClickProperties.OnValueClickUIObjectDelegate = parentComponent.OnClickUIObject;
            uiObjectRuntimeProperties.stringValueClickProperties.button.onClick.AddListener(
                delegate { 
                    uiObjectRuntimeProperties.stringValueClickProperties.OnValueClickUIObjectDelegate(uiObjectRuntimeProperties.propertyId.uiObjectName,
                        uiObjectRuntimeProperties.stringValueClickProperties.uiObjectValue);
                });
        }
    }
}
