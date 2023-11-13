using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


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
        if (uiComponentManagerData.MainMenuComponent != null) {
            RegisterUIComponent(uiComponentManagerData.MainMenuComponent);
        }
        if (uiComponentManagerData.SettingsComponent != null) {
            RegisterUIComponent(uiComponentManagerData.SettingsComponent);
        }
        if (uiComponentManagerData.BackComponent != null) {
            RegisterUIComponent(uiComponentManagerData.BackComponent);
        }
    }

    protected void RegisterUIComponent(UIComponent uiComponent) {
        uiComponentNameToComponent.Add(uiComponent.uiComponentName, uiComponent);
    }

    void DidReceiveUIComponentRequest(UIComponentRequest uiComponentRequest) {
        string uiComponentName = uiComponentRequest.uiComponentName;
        print($"here too: {uiComponentName}");
        switch (uiComponentRequest.uiComponentRequestMode) {
            case (UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE):
                StartCoroutine(EnableUIComponent(uiComponentName));
                break;
            case (UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE):
                StartCoroutine(DisableUIComponent(uiComponentName));
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
        if (loadedUIComponents.Contains(uiComponentName)) {
            print($"re-enabling: {uiComponentName}");
            // We have already loaded the UI Component, just enable it.
            SetUIComponentActive(uiComponentName,true);
        } else {
            // The UI Component is not loaded, load it.
            StartCoroutine(LoadUIComponent(uiComponentName));
        }
        activeUIComponents.Add(uiComponentName);
        onActiveUIComponentsChange?.Invoke(
            new UIComponentRequest(uiComponentName,UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE));
    }

    protected void SetUIComponentActive(string uiComponentName, bool isActive) {
        uiComponentNameToComponent[uiComponentName].uiComponentRuntime.SetActive(isActive);
    }

    protected IEnumerator DisableUIComponent(string uiComponentName) {
        if (!activeUIComponents.Contains(uiComponentName)) {
            // This component is not active, early return.
            yield break;
        }
        uiComponentNameToComponent[uiComponentName].uiComponentRuntime.SetActive(false);
        activeUIComponents.Remove(uiComponentName);
        onActiveUIComponentsChange?.Invoke(
            new UIComponentRequest(uiComponentName,UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
    }

    IEnumerator LoadUIComponent(string uiComponentName) {
        // Retrieve component from library, instantiate in the globalCanvas, and add it to the active set.
        UIComponent uiComponent = uiComponentNameToComponent[uiComponentName];
        uiComponent.uiComponentRuntime = Instantiate(uiComponentManagerData.uiComponentPrefab);
        uiComponent.uiComponentRuntime.transform.SetParent(globalCanvas.transform, false);
        loadedUIComponents.Add(uiComponentName);

        // Iterate over sub objects and load them.
        float uiObjectSpacing = uiComponent.uiObjectSpacing;
        Vector2 currentUIObjectPosition = new Vector2(uiComponent.startingUIObjectPosition.x,
                                                      uiComponent.startingUIObjectPosition.y);
        foreach (UIObject uiObject in uiComponent.uiObjects) {
            yield return StartCoroutine(LoadUIObject(uiComponent, uiObject, currentUIObjectPosition));
            currentUIObjectPosition -= new Vector2(0f,
                                                   uiObjectSpacing+uiObject.rectTransform.rect.height);
        }
    }

    IEnumerator LoadUIObject(UIComponent parentComponent,
                                  UIObject uiObject,
                                  Vector2 uiObjectPosition) {
        if (uiObject == null) {
            yield break;
        }
        uiObject.isFilled = false;
        GameObject uiObjectClone = Instantiate(uiObject.uiObjectPrefab, parentComponent.uiComponentRuntime.transform);
        uiObject.uiObjectRuntime = uiObjectClone;
        uiObject.onClickUIObjectDelegate = parentComponent.onClickUIObject;
        uiObject.OnLoad(uiComponentManagerData.uiTheme);
        uiObject.rectTransform.localPosition = uiObjectPosition;
        uiObject.isFilled = true;
    }

    static void PrintUIComponentNotRegisered(string uiComponentName) {
        Debug.Log($"The UI Component name: {uiComponentName} is not registered.");
        Debug.Log("Check that it is registered in RegisterUIComponents() and that it exists in the UIComponentManagerData.");
            
    }
}
