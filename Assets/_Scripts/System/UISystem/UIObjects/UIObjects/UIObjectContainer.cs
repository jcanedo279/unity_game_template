using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;


[CreateAssetMenu(fileName = "UIObjectContainer", menuName = "UI System/UI Objects/UI Object Container")]
public class UIObjectContainer : UIObject {
    [SerializeField] private GameObject containerPrefab;
    [System.NonSerialized] private GameObject contentGameObject;
    [SerializeField] private List<UIObjectStringButton> items;
    [System.NonSerialized] private List<GameObject> itemsRuntime;


    public override void FillFromComponentManager(UIComponent parentComponent,
                      Transform parentTransform,
                      UITheme uiTheme,
                      Vector2 uiObjectPosition) {
        if (containerPrefab==null) {
            return;
        }
        isFilled = false;
        uiObjectRuntime = Instantiate(containerPrefab, parentTransform);
        rectTransform = uiObjectRuntime.gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = uiObjectPosition;
    
        // Container-specific logic.
        FillContainer(parentComponent, uiTheme, uiObjectPosition);
        isFilled = true;
    }

    public void FillContainer(UIComponent parentComponent,
                              UITheme uiTheme,
                              Vector2 uiObjectPosition) {
        if (items==null) {
            return;
        }
        // The Content is located in ScrollView->Viewport->Content which is a nested GetComponent,
        // store reference to Content here.
        contentGameObject = uiObjectRuntime.transform.Find("Viewport").transform.Find("Content").gameObject;
        Debug.Log(contentGameObject.transform);
        itemsRuntime = new List<GameObject>();
        foreach (UIObjectStringButton itemObject in items) {
            itemObject.FillFromComponentManager(parentComponent, contentGameObject.transform, uiTheme, uiObjectPosition);
        }
    }
}

// TODO: Create extension which contains description and title.
