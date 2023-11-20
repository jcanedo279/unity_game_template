using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Objects/UI Object")]
public class UIObject : ScriptableObject {
    public string uiObjectName;
    public GameObject uiObjectPrefab;
    // Runtime properties.
    [System.NonSerialized] public RectTransform rectTransform;
    [System.NonSerialized] public GameObject uiObjectRuntime;
    [System.NonSerialized] public bool isFilled = false;
    // UI Theme inputs.
    public UIObjectTextColor uiObjectTextColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;
    public UIObjectImageColor uiObjectImageColor = UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL;
    public bool shouldFillImage = true;


    public virtual void FillFromComponentManager(UIComponent parentComponent,
                      // This may be different than parentComponent if we are filling from a UIObject Container.
                      Transform parentTransform,
                      UITheme uiTheme,
                      Vector2 uiObjectPosition) {
        isFilled = false;
        uiObjectRuntime = Instantiate(uiObjectPrefab, parentTransform);
        rectTransform = uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = uiObjectPosition;
        FillFromTheme(uiTheme);
        InterfaceBasedFilling(parentComponent);
        isFilled = true;
    }

    public virtual void FillFromTheme(UITheme uiTheme) {
        rectTransform = uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        if (shouldFillImage) {
            MaybeFillUIObjectImage(uiTheme);
        }
    }

    // Fill in UIObjects which implement an IUIObject interface.
    private void InterfaceBasedFilling(UIComponent parentComponent) {
        if (this is IUIObjectWithClick iUIObjectWithClick && 
                parentComponent.CanInterceptUIObjectWithClick()) {
            iUIObjectWithClick.OnClickUIObjectDelegate = parentComponent.OnClickUIObject;
        } else if (this is IUIObjectWithStringValueClick iUIObjectWithStringValueClick &&
                parentComponent.CanInterceptUIObjectWithClick()) {
            iUIObjectWithStringValueClick.OnClickUIObjectDelegate = parentComponent.OnClickUIObject;
        }
    }

    // FILLERS - BASE
    // ----------------------------------------------------------------------------------------------
    public void MaybeFillUIObjectImage(UITheme uiTheme) {
        Image uiObjectImage = uiObjectRuntime.gameObject.GetComponent<Image>();
        if (uiObjectImage == null) {
            // No image found on GameObject, don't fill.
            return;
        }
        uiObjectImage.color = UIThemeUtil.ColorFromUIObjectImageColor(uiObjectImageColor,uiTheme);
    }

    // FILLERS - EXTENSIONS
    // ----------------------------------------------------------------------------------------------
    protected void MaybeFillUIObjectText(UITheme uiTheme, string textContent) {
        if (uiTheme.font == null) {
            // No font detected in the theme, leave as the standard.
            return;
        }
        TMP_Text uiObjectText = uiObjectRuntime.gameObject.GetComponentInChildren<TMP_Text>();
        if (uiObjectText == null) {
            // No Text component found on GameObject, don't fill.
            return;
        }
        uiObjectText.font = uiTheme.font;
        uiObjectText.text = textContent;
        uiObjectText.color = UIThemeUtil.ColorFromUIObjectTextColor(uiObjectTextColor,uiTheme);
        uiObjectText.fontSize = 32f;
    }

    protected void MaybeFillUIObjectSize(Vector2 objectSize) {
        rectTransform.sizeDelta = objectSize;
    }
}
