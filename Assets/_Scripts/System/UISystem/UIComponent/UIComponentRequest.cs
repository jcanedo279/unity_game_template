public class UIComponentRequest {
    public enum UIComponentRequestMode {
        REQUEST_MODE_ENABLE,
        REQUEST_MODE_DISABLE,
        REQUEST_MODE_FLIP_ENABLE,
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

public class UIObjectRequest {
    public enum UIObjectRequestMode {
        REQUEST_MODE_ENABLE,
        REQUEST_MODE_DISABLE,
        REQUEST_MODE_RENDER,
    }

    private UIObjectRuntimePropertiesId _propertyId;
    public UIObjectRuntimePropertiesId propertyId { get => _propertyId; private set {
        _propertyId = value;
    }}

    private UIObjectRequestMode _uiObjectRequestMode;
    public UIObjectRequestMode uiObjectRequestMode { get => _uiObjectRequestMode; private set {
        _uiObjectRequestMode = value;
    }}

    public UIObjectRequest(UIObjectRuntimePropertiesId propertyId,
                           UIObjectRequestMode uiObjectRequestMode) {
        this.propertyId = propertyId;
        this.uiObjectRequestMode = uiObjectRequestMode;
    }
}
