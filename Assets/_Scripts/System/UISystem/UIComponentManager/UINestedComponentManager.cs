using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// The goal of this class is to intercept outgoing UI events and use them to drive
/// UI state changes and construct a self-expanding/destructing UI Component hierarchy.
/// </summary>
public class UINestedComponentManager : UIComponentManager {
    private UIObjectResponseEventChannelListener onClickUIObjectEventChannelListener;
    private List<string> nestedUIComponentNameHierarchy;
    private HashSet<string> nestedUIComponentNames;
    private HashSet<string> leafUIComponentNames;

    private static Dictionary<string,UIObjectResponseInterpretation> uiObjectNameToResponseInterpretation
        = new Dictionary<string,UIObjectResponseInterpretation> {
            {"ButtonSettings", new UIObjectResponseInterpretation("SettingsComponent")},
            {"ButtonBack", new UIObjectResponseInterpretation(UINestedComponentInterpretationType.INTERPRETATION_POP)},
            {"ButtonPlay", new UIObjectResponseInterpretation(UINestedComponentInterpretationType.INTERPRETATION_POP)},
        };

    protected override void Awake() {
        onClickUIObjectEventChannelListener = GetComponent<UIObjectResponseEventChannelListener>();
        if (onClickUIObjectEventChannelListener == null) {
            Debug.Log($"No UIObjectResponseEventChannelListener in the UI Nested Component Manager. Early exiting.");
            return;
        }
        onClickUIObjectEventChannelListener.UnityEventResponse += OnClickUIObject;
        base.Awake();
        // After we initialize the base ComponentManager, add the request interceptor.
        base.onActiveUIComponentsChange = OnActiveUIComponentChange;
        nestedUIComponentNameHierarchy = new List<string>();
        nestedUIComponentNames = new HashSet<string>();
        leafUIComponentNames = new HashSet<string>();
    }

    // When a UI Component is enabled/disabled, update our internal model.
    public void OnActiveUIComponentChange(UIComponentRequest uiComponentRequest) {
        string uiComponentName = uiComponentRequest.uiComponentName;
        UIComponentRequest.UIComponentRequestMode requestMode = uiComponentRequest.uiComponentRequestMode;
        int nestedHierarchySize = nestedUIComponentNameHierarchy.Count;
        switch (uiComponentNameToComponent[uiComponentName].uiNestedComponentType) {
            case UINestedComponentType.STATE_NESTED_MAIN:
                if (requestMode == UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE) {
                    // Since we are enabling a new nested component, disable whatever was on top before (if it exists).
                    if (nestedHierarchySize > 0) {
                        string topNestedComponentName = nestedUIComponentNameHierarchy[nestedHierarchySize-1];
                        base.SetUIComponentActive(topNestedComponentName,false);
                    }
                    nestedUIComponentNameHierarchy.Add(uiComponentName);
                    nestedUIComponentNames.Add(uiComponentName);
                } else {
                    // Since we are disabling the top component, enable whatever is underneath (if it exists).
                    if (nestedHierarchySize > 1) {
                        string underneathNestedComponentName = nestedUIComponentNameHierarchy[nestedHierarchySize-2];
                        base.SetUIComponentActive(underneathNestedComponentName,true);
                    }
                    nestedUIComponentNameHierarchy.Remove(uiComponentName);
                    nestedUIComponentNames.Remove(uiComponentName);
                }
                break;
            case UINestedComponentType.STATE_LEAF:
                if (requestMode == UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE) {
                    leafUIComponentNames.Add(uiComponentName);
                } else {
                    leafUIComponentNames.Remove(uiComponentName);
                }
                break;
        }
    }

    // When we intercept UI Object clicks, potentially drive the internal model state.
    void OnClickUIObject(UIObjectResponse uiObjectResponse) {
        string uiObjectName = uiObjectResponse.uiObjectName;
        string uiComponentName = uiObjectResponse.uiComponentName;
        print($"Intercepting ObjectResponse object: {uiObjectName} from component: {uiComponentName}.");

        if (!uiObjectNameToResponseInterpretation.ContainsKey(uiObjectName)) {
            return;
        }
        UIObjectResponseInterpretation objectResponseInterpretation = uiObjectNameToResponseInterpretation[uiObjectName];
    
        int nestedHierarchySize = nestedUIComponentNameHierarchy.Count;
        switch (objectResponseInterpretation.currentNestedUIComponentInterpretationType) {
            case UINestedComponentInterpretationType.INTERPRETATION_POP:
                if (nestedHierarchySize == 0) {
                    throw new System.ArgumentOutOfRangeException(
                        $"UI Object: {uiObjectName} is trying to 'POP' an empty nested hierarchy :<");
                }
                // Maybe delete the back button when removing a nested UI layer.
                // Do this before disabling the current nested component to get an accurate hierarchy size.
                if (activeUIComponents.Contains("BackComponent") && nestedHierarchySize<=2) {
                    StartCoroutine(DisableUIComponent("BackComponent"));
                }
                StartCoroutine(DisableUIComponent(nestedUIComponentNameHierarchy[nestedHierarchySize-1]));
                break;
            case UINestedComponentInterpretationType.INTERPRETATION_ADD:
                // Maybe add the back button when adding a nested UI layer.
                if (!activeUIComponents.Contains("BackComponent") && nestedHierarchySize>0) {
                    StartCoroutine(EnableUIComponent("BackComponent"));
                }
                StartCoroutine(EnableUIComponent(objectResponseInterpretation.targetNestedUIComponentName));
                break;
        }
        foreach (string targetLeafUIComponentName in objectResponseInterpretation.targetLeafUIComponentNames) {
            StartCoroutine(EnableUIComponent(targetLeafUIComponentName));
        }
    }

    public class UIObjectResponseInterpretation {
        public UINestedComponentInterpretationType currentNestedUIComponentInterpretationType;
        public List<string> targetLeafUIComponentNames = new List<string>();
        public string targetNestedUIComponentName = "";

        public UIObjectResponseInterpretation(UINestedComponentInterpretationType currentNestedUIComponentInterpretationType,
                                              List<string> targetLeafUIComponentNames = null) {
            if (currentNestedUIComponentInterpretationType == UINestedComponentInterpretationType.INTERPRETATION_ADD) {
                throw new System.ArgumentOutOfRangeException("The interpretationType: Add reques a component to add, use the other constructor.");
            }
            this.currentNestedUIComponentInterpretationType = currentNestedUIComponentInterpretationType;
            MaybeAddTargetLeafUIComponentNames(targetLeafUIComponentNames);
        }
        public UIObjectResponseInterpretation(string targetNestedUIComponentName,
                                              List<string> targetLeafUIComponentNames = null) {
            this.targetNestedUIComponentName = targetNestedUIComponentName;
            this.currentNestedUIComponentInterpretationType = UINestedComponentInterpretationType.INTERPRETATION_ADD;
            MaybeAddTargetLeafUIComponentNames(targetLeafUIComponentNames);
        }
        public void MaybeAddTargetLeafUIComponentNames(List<string> targetLeafUIComponentNames = null) {
            if (targetLeafUIComponentNames != null) {
                this.targetLeafUIComponentNames = targetLeafUIComponentNames;
            }
        }
    }
}


/// <summary>
/// We can either:
/// - Do nothing to the main nested hierarchy (add a peripheral UI).
/// - Remove the current nested component and enable the one below (ie pressing back button).
/// - Add a new nested component and disable the current (this requires a component).
/// </summary>
public enum UINestedComponentInterpretationType {
    INTERPRETATION_NONE,
    INTERPRETATION_POP,
    INTERPRETATION_ADD, // This is not used, instead we have a new entry with the target component (constructor).
}

public enum UINestedComponentType {
    STATE_LEAF,
    STATE_NESTED_MAIN,
}
