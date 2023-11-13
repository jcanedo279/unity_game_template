using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIObjectButton", menuName = "UI System/UI Object Button")]
public class UIObjectButton : UIObject {
    private Button uiObjectButton;
    [SerializeField] private string buttonTextContent;

    public override void OnLoad(UITheme uiTheme) {
        uiObjectButton = uiObjectRuntime.GetComponent<Button>();
        uiObjectButton.onClick.AddListener( delegate {OnClickUIObject();} );
        base.OnLoad(uiTheme);
        MaybeFillUIObjectText(uiTheme, buttonTextContent);
    }

    public override void OnClickUIObject() {
        onClickUIObjectDelegate?.Invoke(uiObjectName);
    }
}
