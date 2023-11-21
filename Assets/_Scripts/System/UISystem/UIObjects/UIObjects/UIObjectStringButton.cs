using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIObjectStringButton", menuName = "UI System/UI Objects/UI Object String Button")]
public class UIObjectStringButton : UIObject,
                                    IUIObjectWithSize,
                                    IUIObjectWithImage, IUIObjectWithImageColor,
                                    IUIObjectWithTextChild, IUIObjectWithStringValueClick {

    // INTERFACE - IUIObjectWithSize
    [SerializeField] private Vector2 _objectSize = new Vector2(320,60);
    public Vector2 objectSize { get { return _objectSize; } }

    // INTERFACE - IUIObjectWithImage
    [SerializeField] private Sprite _imageSprite;
    public Sprite imageSprite { get { return _imageSprite; } }
    public Image image { get; set; }

    // INTERFACE - IUIObjectWithImageColor
    [SerializeField] private UIObjectImageColor _imageColor = UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL;
    public UIObjectImageColor imageColor { get { return _imageColor; } }

    // INTERFACE - IUIObjectWithTextChild
    [SerializeField] private string _textContent;
    public string textContent { get { return _textContent; } }
    [SerializeField] private UIObjectTextColor _textColor = UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY;
    public UIObjectTextColor textColor { get { return _textColor; } }

    // INTERFACE - IUIObjectWithStringValueClick
    public Button button { get; set; }
    public System.Action<string,string> OnValueClickUIObjectDelegate { get; set; }
    public void OnClickUIObject() {
        OnValueClickUIObjectDelegate(uiObjectName, textContent);
    }
}
