using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectStringButton", menuName = "UI System/UI Objects/UI Object String Button")]
public class UIObjectStringButton : UIObject, IUIObjectWithStringValueClick {
    [System.NonSerialized] public Button uiObjectButton;
    public string buttonTextContent;
    public Vector2 buttonSize = new Vector2(320,60);

    public override void FillFromTheme(UITheme uiTheme) {
        base.FillFromTheme(uiTheme);
        MaybeFillUIObjectButton(uiTheme);
    }


    // INTERFACE - IUIObjectWithStringValueClick
    public System.Action<string,string> OnClickUIObjectDelegate { get; set; }
    private void MaybeFillUIObjectButton(UITheme uiTheme) {
        uiObjectButton = uiObjectRuntime.GetComponent<Button>();
        uiObjectButton.onClick.AddListener( delegate {OnClickUIObject();} );
        MaybeFillUIObjectText(uiTheme, buttonTextContent);
        MaybeFillUIObjectSize(buttonSize);
    }
    public void OnClickUIObject() {
        OnClickUIObjectDelegate(uiObjectName, buttonTextContent);
    }
}
