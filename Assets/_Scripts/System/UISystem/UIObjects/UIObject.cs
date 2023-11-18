using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Object")]
public class UIObject : ScriptableObject {
    [SerializeField] public string uiObjectName;
    [SerializeField] public GameObject uiObjectPrefab;

    // Runtime properties.
    [System.NonSerialized] public RectTransform rectTransform;
    [System.NonSerialized] public GameObject uiObjectRuntime;
    [System.NonSerialized] public bool isFilled = false;

    // UI Theme inputs.
    [SerializeField] private UIObjectTextColor uiObjectTextColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;
    [SerializeField] private UIObjectImageColor uiObjectImageColor = UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL;
    [SerializeField] private bool shouldFillImage = true;

    // Event handling.
    [System.NonSerialized] public System.Action<string> onClickUIObjectDelegate;


    public virtual void OnLoad(UITheme uiTheme) {
        if (uiObjectRuntime == null) {
            return;
        }
        MaybeFillUIObject(uiTheme);
    }

    // FILLERS - BASE
    // ----------------------------------------------------------------------------------------------
    public void MaybeFillUIObject(UITheme uiTheme) {
        rectTransform = uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        if (shouldFillImage) {
            MaybeFillUIObjectImage(uiTheme);
        }
    }

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

    // CALLBACKS - EXTENSIONS
    // ----------------------------------------------------------------------------------------------
    public virtual void OnClickUIObject() {
        onClickUIObjectDelegate?.Invoke(uiObjectName);
    }
}
