using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectButton", menuName = "UI System/UI Objects/UI Object Button")]
public class UIObjectButton : UIObject, IUIObjectWithClick {
    public string buttonTextContent;
    public Vector2 buttonSize = new Vector2(320,60);
    [System.NonSerialized] public Button uiObjectButton;

    public override void FillFromTheme(UITheme uiTheme) {
        base.FillFromTheme(uiTheme);
        MaybeFillUIObjectButton(uiTheme);
    }


    // INTERFACE - IUIObjectWithClick
    public System.Action<string> OnClickUIObjectDelegate { get; set; }
    private void MaybeFillUIObjectButton(UITheme uiTheme) {
        uiObjectButton = uiObjectRuntime.GetComponent<Button>();
        uiObjectButton.onClick.AddListener( delegate {OnClickUIObject();} );
        MaybeFillUIObjectText(uiTheme, buttonTextContent);
        MaybeFillUIObjectSize(buttonSize);
    }
    public void OnClickUIObject() {
        OnClickUIObjectDelegate?.Invoke(uiObjectName);
    }
}
