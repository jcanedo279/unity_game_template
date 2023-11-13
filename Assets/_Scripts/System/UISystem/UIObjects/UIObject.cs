using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Object")]
public class UIObject : ScriptableObject {
    public bool isFilled = false;
    [SerializeField] public string uiObjectName;
    [SerializeField] public GameObject uiObjectPrefab;
    public RectTransform rectTransform;
    public GameObject uiObjectRuntime;

    // UI Theme inputs.
    [SerializeField] public UIObjectImageColor uiObjectImageColor = UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL;
    [SerializeField] private bool shouldFillImage = true;

    // Event handling.
    public System.Action<string> onClickUIObjectDelegate;


    public virtual void OnLoad(UITheme uiTheme) {
        if (uiObjectRuntime == null) {
            return;
        }
        MaybeFillUIObject(uiTheme);
    }

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

    public void MaybeFillUIObjectText(UITheme uiTheme, string textContent) {
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
    }

    public virtual void OnClickUIObject() {
        // No-op.
    }
}
