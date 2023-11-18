using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIObjectHorizontalShelf", menuName = "UI System/UI Object Horizontal Shelf")]
public class UIObjectHorizontalShelf : UIObject {
    [SerializeField] UIObject shelfItemPrefab;
    // The Content is located in ScrollView->Viewport->Content which is a nested GetComponent,
    // store reference to Content here.
    [SerializeField] GameObject shelfContentContainer;
    [System.NonSerialized] List<GameObject> shelfItemsRuntime;

    public override void OnLoad(UITheme uiTheme) {
        base.OnLoad(uiTheme);

        if (shelfItemPrefab==null || shelfContentContainer==null) {
            return;
        }
        shelfItemsRuntime = new List<GameObject>();
        
    }
}
