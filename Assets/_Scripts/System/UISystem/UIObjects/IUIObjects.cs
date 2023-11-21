using UnityEngine;
using UnityEngine.UI;
using TMPro;


public interface IUIObjectWithSize {
    public Vector2 objectSize { get; }
}

public interface IUIObjectWithImageColor {
    public UIObjectImageColor imageColor { get; }
}

public interface IUIObjectWithImage {
    public Sprite imageSprite { get; }
    public Image image { get; set; }
}

public interface IUIObjectWithImageChild {
    public Sprite childImageSprite { get; }
    public Image childImage { get; set; }
}

public interface IUIObjectWithTextChild {
    public string textContent { get; }
    public UIObjectTextColor textColor { get; }
}

public interface IUIObjectWithButton {
    public Button button { get; set; }
}

public interface IUIObjectWithClick : IUIObjectWithButton {
    public System.Action<string> OnClickUIObjectDelegate { get; set; }
    public void OnClickUIObject();
}

public interface IUIObjectWithStringValueClick : IUIObjectWithButton {
    public System.Action<string,string> OnValueClickUIObjectDelegate { get; set; }
    public void OnClickUIObject();
}

public static class IUIObjectFillers {
    public static void MaybeFillUIObjectSize(UIObject uiObject) {
        if (uiObject is IUIObjectWithSize uiObjectWithSize) {
            uiObject.rectTransform.sizeDelta = uiObjectWithSize.objectSize;
        }
    }
    public static void MaybeFillUIObjectImageColor(UIObject uiObject, UITheme uiTheme) {
        if (uiObject is IUIObjectWithImageColor uiObjectWithImageColor) {
            Image uiObjectImage = uiObject.uiObjectRuntime.gameObject.GetComponent<Image>();
            if (uiObjectImage == null) {
                // No image found on GameObject, don't fill.
                return;
            }
            uiObjectImage.color = UIThemeUtil.ColorFromUIObjectImageColor(uiObjectWithImageColor.imageColor,uiTheme);
        }
    }

    public static void MaybeFillUIObjectImage(UIObject uiObject) {
        if (uiObject is IUIObjectWithImage uiObjectWithImage) {
            if (uiObjectWithImage.imageSprite == null) {
                throw new System.ArgumentNullException($"UIObject: {uiObject.uiObjectName} does not have an imageSprite but implements IUIObjectWithImage.");
            }
            uiObjectWithImage.image = uiObject.uiObjectRuntime.GetComponent<Image>();
            uiObjectWithImage.image.sprite = uiObjectWithImage.imageSprite;
        }
    }

    public static void MaybeFillUIObjectImageChild(UIObject uiObject) {
        if (uiObject is IUIObjectWithImageChild uiObjectWithImageChild) {
            if (uiObjectWithImageChild.childImageSprite == null) {
                throw new System.ArgumentNullException($"UIObject: {uiObject.uiObjectName} does not have a childImageSprite but implements IUIObjectWithImageChild.");
            }
            GameObject iconObjectRuntime = new GameObject(uiObjectWithImageChild.childImageSprite.name);
            iconObjectRuntime.transform.SetParent(uiObject.uiObjectRuntime.transform, false);
            uiObjectWithImageChild.childImage = iconObjectRuntime.AddComponent<Image>();
            uiObjectWithImageChild.childImage.sprite = uiObjectWithImageChild.childImageSprite;
        }
    }

    public static void MaybeFillUIObjectTextChild(UIObject uiObject, UITheme uiTheme) {
        if (uiObject is IUIObjectWithTextChild uiObjectWithTextChild) {
            if (uiTheme.font == null) {
                // No font detected in the theme, leave as the standard.
                return;
            }
            TMP_Text uiObjectText = uiObject.uiObjectRuntime.gameObject.GetComponentInChildren<TMP_Text>();
            if (uiObjectText == null) {
                // No Text component found on GameObject, don't fill.
                return;
            }
            uiObjectText.font = uiTheme.font;
            uiObjectText.text = uiObjectWithTextChild.textContent;
            uiObjectText.color = UIThemeUtil.ColorFromUIObjectTextColor(uiObjectWithTextChild.textColor,uiTheme);
            uiObjectText.fontSize = 32f;
        }
    }

    public static void MaybeFillUIObjectButton(UIComponent parentComponent, UIObject uiObject) {
        if (uiObject is IUIObjectWithClick uiObjectWithClick && 
                parentComponent.CanInterceptUIObjectWithClick()) {
            uiObjectWithClick.OnClickUIObjectDelegate = parentComponent.OnClickUIObject;
            uiObjectWithClick.button = uiObject.uiObjectRuntime.GetComponent<Button>();
            uiObjectWithClick.button.onClick.AddListener(
                delegate {uiObjectWithClick.OnClickUIObject();} );
        } else if (uiObject is IUIObjectWithStringValueClick uiObjectWithStringValueClick &&
                parentComponent.CanInterceptUIObjectWithClick()) {
            uiObjectWithStringValueClick.OnValueClickUIObjectDelegate = parentComponent.OnClickUIObject;
            uiObjectWithStringValueClick.button = uiObject.uiObjectRuntime.GetComponent<Button>();
            uiObjectWithStringValueClick.button.onClick.AddListener(
                delegate {uiObjectWithStringValueClick.OnClickUIObject();} );
        }
    }
}
