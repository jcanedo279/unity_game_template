using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectIconButton", menuName = "UI System/UI Objects/UI Object Icon Button")]
public class UIObjectIconButton : UIObjectButton, IUIObjectWithImageChild {

    // INTERFACE - IUIObjectWithImage
    [SerializeField] private Sprite _childImageSprite;
    public Sprite childImageSprite { get { return _childImageSprite; } }
    public Image childImage { get; set; }
}
