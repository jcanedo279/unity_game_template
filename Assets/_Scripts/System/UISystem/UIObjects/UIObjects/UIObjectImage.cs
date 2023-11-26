using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectImage", menuName = "UI System/UI Objects/UI Object Image")]
public class UIObjectImage : UIObject, IUIObjectWithImageColor, IUIObjectWithImage {

    // INTERFACE - IUIObjectWithImage
    [SerializeField] private Sprite _imageSprite;
    public Sprite imageSprite { get { return _imageSprite; } set { _imageSprite = value; } }

    // INTERFACE - IUIObjectWithImageColor
    [SerializeField] private UIObjectImageColor _imageColor = UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL;
    public UIObjectImageColor imageColor { get { return _imageColor; } }
}
