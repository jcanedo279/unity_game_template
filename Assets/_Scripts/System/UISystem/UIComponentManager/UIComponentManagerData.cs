using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "UIComponentManagerData", menuName = "UI System/UI Component Manager Data")]
public class UIComponentManagerData : ScriptableObject {
    [SerializeField] public UITheme uiTheme;
    public GameObject uiComponentPrefab;
    [SerializeField] public List<UIComponent> registeredComponents;
}
