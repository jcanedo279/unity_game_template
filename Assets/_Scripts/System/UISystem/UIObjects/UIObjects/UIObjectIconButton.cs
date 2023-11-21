using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectIconButton", menuName = "UI System/UI Objects/UI Object Icon Button")]
public class UIObjectIconButton : UIObjectButton, IUIObjectWithImageChild {
    [System.NonSerialized] public GameObject iconObjectRuntime;

    // INTERFACE - IUIObjectWithImage
    [SerializeField] private Sprite _imageSprite;
    public Sprite childImageSprite { get { return _imageSprite; } }
    public Image childImage { get; set; }
}
