using UnityEngine;


[CreateAssetMenu(fileName = "UIObject", menuName = "UI System/UI Objects/UI Object")]
public class UIObject : ScriptableObject {
    public string uiObjectName;
    public GameObject uiObjectPrefab;
    // Runtime properties.
    [System.NonSerialized] public RectTransform rectTransform;
    [System.NonSerialized] public GameObject uiObjectRuntime;
    [System.NonSerialized] public bool isFilled = false;


    public virtual void FillFromComponentManager(UIComponent parentComponent,
                      // This may be different than parentComponent if we are filling from a UIObject Container.
                      Transform parentTransform,
                      UITheme uiTheme,
                      Vector2 uiObjectPosition) {
        isFilled = false;
        uiObjectRuntime = Instantiate(uiObjectPrefab, parentTransform);
        rectTransform = uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = uiObjectPosition;
        InterfaceBasedFilling(parentComponent, uiTheme);
        isFilled = true;
    }

    // Fill in UIObjects which implement an IUIObject interface.
    private void InterfaceBasedFilling(UIComponent parentComponent, UITheme uiTheme) {
        IUIObjectFillers.MaybeFillUIObjectSize(this);
        IUIObjectFillers.MaybeFillUIObjectImageColor(this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectImage(this);
        IUIObjectFillers.MaybeFillUIObjectTextChild(this, uiTheme);
        IUIObjectFillers.MaybeFillUIObjectButton(parentComponent, this);
        IUIObjectFillers.MaybeFillUIObjectImageChild(this);
    }

    // FILLERS - EXTENSIONS
    // ----------------------------------------------------------------------------------------------
    protected void MaybeFillUIObjectSize(Vector2 objectSize) {
        rectTransform.sizeDelta = objectSize;
    }
}
