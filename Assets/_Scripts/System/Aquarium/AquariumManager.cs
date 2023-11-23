using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AquariumManager : MonoBehaviour {
    [SerializeField] private Sprite aquariumSprite;
    [System.NonSerialized] private UIObjectResponseEventChannelListener objectResponseEventChannelListener;
    [System.NonSerialized] private GameObject nextAquarium;

    void Awake() {
        objectResponseEventChannelListener = GetComponent<UIObjectResponseEventChannelListener>();
        if (objectResponseEventChannelListener == null) {
            throw new System.ArgumentNullException("The AquariumManager requires a UIObjectResponse listener.");
        }
        objectResponseEventChannelListener.UnityEventResponse += OnUIObjectResponse;
    }

    void OnUIObjectResponse(UIObjectResponse uiObjectResponse) {
        if (uiObjectResponse.uiComponentName!="UIComponentPurchaseAquarium" || 
            uiObjectResponse.uiObjectName!="Purchase Aquarium") {
                return;
        }
        SpriteRenderer nextAquariumSpriteRenderer = new SpriteRenderer();
        if (nextAquarium == null) {
            nextAquarium = new GameObject("NextAquarium");
            nextAquariumSpriteRenderer = nextAquarium.AddComponent<SpriteRenderer>();
            nextAquariumSpriteRenderer.sprite = aquariumSprite;
        }
        if (nextAquariumSpriteRenderer == null) {
            nextAquariumSpriteRenderer = nextAquarium.GetComponent<SpriteRenderer>();
        }
        nextAquariumSpriteRenderer.color = new Color(0f,0f,0f);
        nextAquarium.transform.SetParent(transform, false);
    }
}
