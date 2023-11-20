public class UIObjectResponse {
    public string uiObjectName;
    public string uiObjectValue;
    public string uiComponentName;

    public UIObjectResponse(string uiObjectName,
                            string uiComponentName) {
                                this.uiObjectName = uiObjectName;
                                this.uiComponentName = uiComponentName;
                            }
    public UIObjectResponse(string uiObjectName,
                            string uiObjectValue,
                            string uiComponentName) {
                                this.uiObjectName = uiObjectName;
                                this.uiObjectValue = uiObjectValue;
                                this.uiComponentName = uiComponentName;
                            }
}