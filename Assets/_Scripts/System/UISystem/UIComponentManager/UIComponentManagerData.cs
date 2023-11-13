using UnityEngine;


[CreateAssetMenu(fileName = "UIComponentManagerData", menuName = "UI System/UI Component Manager Data")]
public class UIComponentManagerData : ScriptableObject {
    [SerializeField] public UITheme uiTheme;
    public GameObject uiComponentPrefab;
    [SerializeField] public UIComponent MainMenuComponent;
    [SerializeField] public UIComponent SettingsComponent;
    [SerializeField] public UIComponent BackComponent;
}
