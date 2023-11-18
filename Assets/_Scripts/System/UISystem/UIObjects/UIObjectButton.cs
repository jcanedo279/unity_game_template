using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIObjectButton", menuName = "UI System/UI Object Button")]
public class UIObjectButton : UIObject {
    private Button uiObjectButton;
    [SerializeField] private string buttonTextContent;
    [SerializeField] private Vector2 buttonSize = new Vector2(320,60);

    public override void OnLoad(UITheme uiTheme) {
        base.OnLoad(uiTheme);
        MaybeFillUIObjectButton(uiTheme);
    }

    private void MaybeFillUIObjectButton(UITheme uiTheme) {
        uiObjectButton = uiObjectRuntime.GetComponent<Button>();
        uiObjectButton.onClick.AddListener( delegate {OnClickUIObject();} );
        MaybeFillUIObjectText(uiTheme, buttonTextContent);
        MaybeFillUIObjectSize(buttonSize);
    }
}
