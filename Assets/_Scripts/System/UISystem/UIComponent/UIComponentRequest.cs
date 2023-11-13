public class UIComponentRequest {
    public enum UIComponentRequestMode {
        REQUEST_MODE_ENABLE,
        REQUEST_MODE_DISABLE,
        REQUEST_MODE_UNLOAD,
    }
    private string _uiComponentName;
    public string uiComponentName { get => _uiComponentName; private set {
        _uiComponentName = value;
    }}
    private UIComponentRequestMode _uiComponentRequestMode;
    public UIComponentRequestMode uiComponentRequestMode { get => _uiComponentRequestMode; private set {
        _uiComponentRequestMode = value;
    }}

    public UIComponentRequest(string uiComponentName,
                              UIComponentRequestMode uiComponentRequestMode) {
        this.uiComponentName = uiComponentName;
        this.uiComponentRequestMode = uiComponentRequestMode;
    }
}
