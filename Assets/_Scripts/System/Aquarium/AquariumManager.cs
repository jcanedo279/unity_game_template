using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AquariumManager : MonoBehaviour {
    [SerializeField] private Sprite aquariumSprite;
    [SerializeField] private PlayerController playerController; // This is required for 'rooting' the temporary nextAquarium.
    [SerializeField] private UIComponentRequestEventChannel uiComponentRequestEventChannel;
    [System.NonSerialized] private UIObjectResponseEventChannelListener objectResponseEventChannelListener;
    [System.NonSerialized] private GameObject nextAquarium;
    [System.NonSerialized] private SpriteRenderer nextAquariumSpriteRenderer;

    void Awake() {
        objectResponseEventChannelListener = GetComponent<UIObjectResponseEventChannelListener>();
        if (uiComponentRequestEventChannel == null) {
            throw new System.ArgumentNullException("The AquariumManager requires a UIComponentRequest channel.");
        }
        if (objectResponseEventChannelListener == null) {
            throw new System.ArgumentNullException("The AquariumManager requires a UIObjectResponse listener.");
        }
        objectResponseEventChannelListener.UnityEventResponse += OnUIObjectResponse;
    }

    void OnUIObjectResponse(UIObjectResponse uiObjectResponse) {
        if (uiObjectResponse.uiComponentName=="PurchaseAquariumComponent" && uiObjectResponse.uiObjectName=="PurchaseAquarium") {
            OnOpenPurchaseAquariumUI(uiObjectResponse);
        }
        if (uiObjectResponse.uiComponentName=="PurchaseAquariumEduComponent" && uiObjectResponse.uiObjectValue=="Container/CheckMark") {
            OnPurchaseAquarium();
        }
        if (uiObjectResponse.uiComponentName=="PurchaseAquariumEduComponent" && uiObjectResponse.uiObjectValue=="Container/Cross") {
            OnCancelPurchaseAquarium();
        }
    }

    void OnOpenPurchaseAquariumUI(UIObjectResponse uiObjectResponse) {
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest(uiObjectResponse.uiComponentName,
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumEduComponent",
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE));
        nextAquarium = new GameObject("NextAquarium");
        nextAquariumSpriteRenderer = nextAquarium.AddComponent<SpriteRenderer>();
        nextAquariumSpriteRenderer.sprite = aquariumSprite;
        nextAquariumSpriteRenderer.color = new Color(0.25f,0.25f,0.25f);
        nextAquarium.transform.SetParent(playerController.transform, false);
        nextAquarium.transform.localPosition = new Vector3(aquariumSprite.rect.size.x/(aquariumSprite.pixelsPerUnit*2f)
                                                            +playerController.spriteRenderer.sprite.rect.size.x/(playerController.spriteRenderer.sprite.pixelsPerUnit*2f),
                                                           aquariumSprite.rect.size.y/(aquariumSprite.pixelsPerUnit*2f)
                                                            -playerController.spriteRenderer.sprite.rect.size.y/(playerController.spriteRenderer.sprite.pixelsPerUnit*2f),
                                                           0f);
    }

    void OnPurchaseAquarium() {
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumEduComponent",
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
        nextAquarium.transform.SetParent(this.transform);
        nextAquariumSpriteRenderer.color = new Color(1,1,1);
    }
    void OnCancelPurchaseAquarium() {
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumEduComponent",
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
        Destroy(nextAquarium);
    }
}
