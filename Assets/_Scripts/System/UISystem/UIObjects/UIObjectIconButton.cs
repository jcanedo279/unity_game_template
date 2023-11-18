using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectIconButton", menuName = "UI System/UI Object Icon Button")]
public class UIObjectIconButton : UIObjectButton {
    [SerializeField] private Sprite buttonIcon;
    [System.NonSerialized] private GameObject iconObjectRuntime;
    [System.NonSerialized] private Image buttonIconImage;

    public override void OnLoad(UITheme uiTheme) {
        base.OnLoad(uiTheme);
        MaybeFillUIObjectIconButton(uiTheme);
    }

    public void MaybeFillUIObjectIconButton(UITheme uiTheme) {
        if (buttonIcon == null) {
            Debug.Log("A button icon (Sprite) is required for the IconButton to render.");
            return;
        }
        iconObjectRuntime = new GameObject(buttonIcon.name);
        iconObjectRuntime.transform.SetParent(uiObjectRuntime.transform, false);
        buttonIconImage = iconObjectRuntime.AddComponent<Image>();
        buttonIconImage.sprite = buttonIcon;
    }
}
