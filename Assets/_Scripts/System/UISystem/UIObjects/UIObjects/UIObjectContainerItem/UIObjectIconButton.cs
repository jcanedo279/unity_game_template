using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectIconButton", menuName = "UI System/UI Objects/UI Object Icon Button")]
public class UIObjectIconButton : UIObject, IUIObjectWithContainerItem, IUIObjectWithImageChild {
    // INTERFACE - IUIObjectWithSize
    [SerializeField] private Vector2 _objectSize = new Vector2(320,60);
    public Vector2 objectSize { get { return _objectSize; } }

    // INTERFACE - IUIObjectWithImage
    [SerializeField] private Sprite _imageSprite;
    public Sprite imageSprite { get { return _imageSprite; } set { _imageSprite = value; } }

    // INTERFACE - IUIObjectWithImageColor
    [SerializeField] private UIObjectImageColor _imageColor = UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL;
    public UIObjectImageColor imageColor { get { return _imageColor; } }

    // INTERFACE - IUIObjectWithImageChild
    [SerializeField] private Sprite _childImageSprite;
    public Sprite childImageSprite { get { return _childImageSprite; } set { _childImageSprite = value; } }

    // INTERFACE - IUIObjectWithStringValueClick
    [SerializeField] private string _uiObjectValue;
    public string uiObjectValue { get { return _uiObjectValue; } set { _uiObjectValue = value; } }
}
