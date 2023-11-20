using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectIconButton", menuName = "UI System/UI Objects/UI Object Icon Button")]
public class UIObjectIconButton : UIObjectButton {
    public Sprite buttonIcon;
    [System.NonSerialized] public GameObject iconObjectRuntime;
    [System.NonSerialized] public Image buttonIconImage;

    public override void FillFromTheme(UITheme uiTheme) {
        base.FillFromTheme(uiTheme);
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
