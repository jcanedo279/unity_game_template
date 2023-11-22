using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIObjectRuntimeProperties
{
    public bool isFilled = false;
    public string uiObjectValue;
    public GameObject uiObjectRuntime;
    public RectTransform rectTransform;
    public Image image { get; set; }
    public Image childImage { get; set; }
    public Button button { get; set; }
    public GameObject contentGameObject; // Used by UIObjectContainer.

    public void OnClickUIObjectRuntimeProperties(UIObject uiObject) {
        if (uiObject is IUIObjectWithStringValueClick uiObjectWithStringValueClick) {
            uiObjectWithStringValueClick.OnValueClickUIObjectDelegate(uiObject.uiObjectName, uiObjectValue);
        }
    }
}

public interface IUIObjectWithSize
{
    public Vector2 objectSize { get; }
}

public interface IUIObjectWithImageColor
{
    public UIObjectImageColor imageColor { get; }
}

public interface IUIObjectWithImage
{
    public Sprite imageSprite { get; }
}

public interface IUIObjectWithImageChild
{
    public Sprite childImageSprite { get; }
}

public interface IUIObjectWithTextChild
{
    public string textContent { get; }
    public UIObjectTextColor textColor { get; }
}

public interface IUIObjectWithButton
{
}

public interface IUIObjectWithClick : IUIObjectWithButton
{
    public System.Action<string> OnClickUIObjectDelegate { get; set; }
    public void OnClickUIObject();
}

public interface IUIObjectWithStringValueClick : IUIObjectWithButton
{
    public System.Action<string, string> OnValueClickUIObjectDelegate { get; set; }
    public void OnClickUIObject();
}

public static class IUIObjectFillers
{
    public static void MaybeFillUIObjectSize(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                             UIObject uiObject)
    {
        if (uiObject is IUIObjectWithSize uiObjectWithSize)
        {
            uiObjectRuntimeProperties.rectTransform.sizeDelta = uiObjectWithSize.objectSize;
        }
    }

    public static void MaybeFillUIObjectImage(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                              UIObject uiObject)
    {
        if (uiObject is IUIObjectWithImage uiObjectWithImage)
        {
            if (uiObjectWithImage.imageSprite == null)
            {
                throw new System.ArgumentNullException($"UIObject: {uiObject.uiObjectName} does not have an imageSprite but implements IUIObjectWithImage.");
            }
            uiObjectRuntimeProperties.image = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<Image>();
            uiObjectRuntimeProperties.image.sprite = uiObjectWithImage.imageSprite;
        }
    }

    public static void MaybeFillUIObjectImageColor(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                   UIObject uiObject,
                                                   UITheme uiTheme)
    {
        if (uiObject is IUIObjectWithImageColor uiObjectWithImageColor)
        {
            Image uiObjectImage;
            if (uiObject is IUIObjectWithImage)
            {
                uiObjectImage = uiObjectRuntimeProperties.image;
            }
            else
            {
                uiObjectImage = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponent<Image>();
            }
            if (uiObjectImage == null)
            {
                // No image found on GameObject, don't fill.
                return;
            }
            uiObjectImage.color = UIThemeUtil.ColorFromUIObjectImageColor(uiObjectWithImageColor.imageColor, uiTheme);
        }
    }

    public static void MaybeFillUIObjectImageChild(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                   UIObject uiObject)
    {
        if (uiObject is IUIObjectWithImageChild uiObjectWithImageChild)
        {
            if (uiObjectWithImageChild.childImageSprite == null)
            {
                throw new System.ArgumentNullException($"UIObject: {uiObject.uiObjectName} does not have a childImageSprite but implements IUIObjectWithImageChild.");
            }
            GameObject iconObjectRuntime = new GameObject(uiObjectWithImageChild.childImageSprite.name);
            iconObjectRuntime.transform.SetParent(uiObjectRuntimeProperties.uiObjectRuntime.transform, false);
            uiObjectRuntimeProperties.childImage = iconObjectRuntime.AddComponent<Image>();
            uiObjectRuntimeProperties.childImage.sprite = uiObjectWithImageChild.childImageSprite;
        }
    }

    public static void MaybeFillUIObjectTextChild(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                                  UIObject uiObject,
                                                  UITheme uiTheme)
    {
        if (uiObject is IUIObjectWithTextChild uiObjectWithTextChild)
        {
            if (uiTheme.font == null)
            {
                // No font detected in the theme, leave as the standard.
                return;
            }
            TMP_Text uiObjectText = uiObjectRuntimeProperties.uiObjectRuntime.gameObject.GetComponentInChildren<TMP_Text>();
            if (uiObjectText == null)
            {
                // No Text component found on GameObject, don't fill.
                return;
            }
            uiObjectText.font = uiTheme.font;
            uiObjectText.text = uiObjectWithTextChild.textContent;
            uiObjectText.color = UIThemeUtil.ColorFromUIObjectTextColor(uiObjectWithTextChild.textColor, uiTheme);
            uiObjectText.fontSize = 32f;
        }
    }

    public static void MaybeFillUIObjectButton(UIObjectRuntimeProperties uiObjectRuntimeProperties,
                                               UIComponent parentComponent,
                                               UIObject uiObject)
    {
        if (uiObject is IUIObjectWithClick uiObjectWithClick &&
                parentComponent.CanInterceptUIObjectWithClick())
        {
            uiObjectWithClick.OnClickUIObjectDelegate = parentComponent.OnClickUIObject;
            uiObjectRuntimeProperties.button = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<Button>();
            uiObjectRuntimeProperties.button.onClick.AddListener(
                delegate { uiObjectWithClick.OnClickUIObject(); });
        }
        else if (uiObject is IUIObjectWithStringValueClick uiObjectWithStringValueClick &&
                parentComponent.CanInterceptUIObjectWithClick())
        {
            uiObjectWithStringValueClick.OnValueClickUIObjectDelegate = parentComponent.OnClickUIObject;
            uiObjectRuntimeProperties.button = uiObjectRuntimeProperties.uiObjectRuntime.GetComponent<Button>();
            uiObjectRuntimeProperties.button.onClick.AddListener(
                delegate { uiObjectRuntimeProperties.OnClickUIObjectRuntimeProperties(uiObject); });
        }
    }
}
