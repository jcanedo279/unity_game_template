using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectIconButton", menuName = "UI System/UI Object Icon Button")]
public class UIObjectIconButton : UIObjectButton {
    [SerializeField] private Sprite buttonIcon;

    public override void OnLoad(UITheme uiTheme) {
        if (buttonIcon == null) {
            Debug.Log("A button icon (Sprite) is required for the IconButton to render.");
        }
        GameObject IconObject = new GameObject(buttonIcon.name);
        IconObject.transform.SetParent(uiObjectRuntime.transform, false);
        Image buttonImage = IconObject.AddComponent<Image>();
        buttonImage.sprite = buttonIcon;
        base.OnLoad(uiTheme);
    }
}
