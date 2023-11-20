using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIComponentManager : MonoBehaviour {
    [SerializeField] public GameObject globalCanvas;
    public UIComponentManagerData uiComponentManagerData;
    // Components which are interpreted.
    protected Dictionary<string,UIComponent> uiComponentNameToComponent;
    // Components which are currently displayed.
    protected HashSet<string> activeUIComponents;
    // Components who are loaded in the globalCanvas.
    private HashSet<string> loadedUIComponents;
    protected UIComponentRequestEventChannelListener uiComponentRequestEventChannelListener;
    protected System.Action<UIComponentRequest> onActiveUIComponentsChange;

    protected virtual void Awake() {
        if (uiComponentManagerData == null) {
            Debug.Log("No UI Component Manager Data provided :<");
            return;
        }
        if (globalCanvas == null) {
            Debug.Log("No Canvas configured as a Global Canvas.");
            return;
        } else if (uiComponentManagerData.uiTheme == null) {
            Debug.Log("No UI Theme detected.");
            return;
        } else if (uiComponentManagerData.uiComponentPrefab == null) {
            Debug.Log("No UI Component Prefab provided. This is needed to propagate the canvasRenderer and raycaster clicks.");
            return;
        }
        uiComponentNameToComponent = new Dictionary<string,UIComponent>();
        activeUIComponents = new HashSet<string>();
        loadedUIComponents = new HashSet<string>();
        uiComponentRequestEventChannelListener = GetComponent<UIComponentRequestEventChannelListener>();
        if (uiComponentRequestEventChannelListener == null) {
            Debug.Log("No UI ComponentRequest EventChannel Listener attached, please attach one to listen to component requests.");
            return;
        }
        uiComponentRequestEventChannelListener.UnityEventResponse += DidReceiveUIComponentRequest;
        RegisterUIComponents();
    }

    void Start() {
        // We start ourself for now.
        StartCoroutine(EnableUIComponent("MainMenuComponent"));
    }

    // Registers the available components (manually).
    void RegisterUIComponents() {
        foreach (UIComponent uiComponent in uiComponentManagerData.registeredComponents) {
            uiComponentNameToComponent.Add(uiComponent.uiComponentName, uiComponent);
        }
    }

    void DidReceiveUIComponentRequest(UIComponentRequest uiComponentRequest) {
        string uiComponentName = uiComponentRequest.uiComponentName;
        switch (uiComponentRequest.uiComponentRequestMode) {
            case (UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE):
                StartCoroutine(EnableUIComponent(uiComponentName));
                break;
            case (UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE):
                StartCoroutine(DisableUIComponent(uiComponentName));
                break;
            case (UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_FLIP_ENABLE):
                if (activeUIComponents.Contains(uiComponentName)) {
                    StartCoroutine(DisableUIComponent(uiComponentName));
                } else {
                    StartCoroutine(EnableUIComponent(uiComponentName));
                }
                break;
        }
    }

    protected IEnumerator EnableUIComponent(string uiComponentName) {
        if (activeUIComponents.Contains(uiComponentName)) {
            // A UI Component already exists with this name.
            yield break;
        } else if (!uiComponentNameToComponent.ContainsKey(uiComponentName)) {
            UIComponentManager.PrintUIComponentNotRegisered(uiComponentName);
           yield break;
        }
        if (!loadedUIComponents.Contains(uiComponentName)) {
            // The UI Component is not loaded, load it.
            StartCoroutine(LoadUIComponent(uiComponentName));
        }
        SetUIComponentActive(uiComponentName, true);
        onActiveUIComponentsChange?.Invoke(
            new UIComponentRequest(uiComponentName,UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE));
    }

    protected IEnumerator DisableUIComponent(string uiComponentName) {
        if (!activeUIComponents.Contains(uiComponentName)) {
            // This component is not active, early return.
            yield break;
        }
        SetUIComponentActive(uiComponentName,false);
        onActiveUIComponentsChange?.Invoke(
            new UIComponentRequest(uiComponentName,UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
    }

    protected void SetUIComponentActive(string uiComponentName, bool isActive) {
        uiComponentNameToComponent[uiComponentName].uiComponentRuntime.SetActive(isActive);
        if (isActive) {
            activeUIComponents.Add(uiComponentName);
        } else {
            activeUIComponents.Remove(uiComponentName);
        }
    }

    IEnumerator LoadUIComponent(string uiComponentName) {
        // Retrieve component from library, instantiate in the globalCanvas, and add it to the active set.
        UIComponent uiComponent = uiComponentNameToComponent[uiComponentName];
        uiComponent.uiComponentRuntime = Instantiate(uiComponentManagerData.uiComponentPrefab);
        uiComponent.uiComponentRuntime.transform.SetParent(globalCanvas.transform, false);
        loadedUIComponents.Add(uiComponentName);

        // Iterate over sub objects and load them.
        float uiObjectSpacing = uiComponentManagerData.uiTheme.UISpacingValueFromEnum(uiComponent.uiObjectSpacing);
        Vector2 currentUIObjectPosition = new(uiComponent.startingUIObjectPosition.x,
                                                      uiComponent.startingUIObjectPosition.y);
        foreach (UIObject uiObject in uiComponent.uiObjects) {
            uiObject.FillFromComponentManager(uiComponent,uiComponent.uiComponentRuntime.transform,
                                              uiComponentManagerData.uiTheme,currentUIObjectPosition);
            currentUIObjectPosition -= new Vector2(0f,
                                                   uiObjectSpacing+uiObject.rectTransform.rect.height);
        }
        yield return 0;
    }

    static void PrintUIComponentNotRegisered(string uiComponentName) {
        Debug.Log($"The UI Component name: {uiComponentName} is not registered.");
        Debug.Log("Check that it is registered in RegisterUIComponents() and that it exists in the UIComponentManagerData.");
            
    }
}
