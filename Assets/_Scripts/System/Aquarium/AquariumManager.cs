using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AquariumManager : MonoBehaviour {
    [SerializeField] private Sprite aquariumSprite;
    [SerializeField] private PlayerController playerController; // This is required for 'rooting' the temporary nextAquarium.
    [SerializeField] private UIComponentRequestEventChannel uiComponentRequestEventChannel;
    [SerializeField] private UIObjectRequestEventChannel uiObjectRequestEventChannel;
    [System.NonSerialized] private UIObjectResponseEventChannelListener objectResponseEventChannelListener;
    [System.NonSerialized] private GameObject nextAquarium;
    [System.NonSerialized] private SpriteRenderer nextAquariumSpriteRenderer;
    private IEnumerator aquariumValidationRoutine;

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
        if (uiObjectResponse.uiComponentName=="PurchaseAquariumComponent" && uiObjectResponse.uiObjectName=="PurchaseAquariumItem") {
            OnOpenPurchaseAquariumUI(uiObjectResponse);
        }
        if (uiObjectResponse.uiComponentName=="PurchaseAquariumEduComponent" && uiObjectResponse.uiObjectValue=="CheckMark") {
            OnPurchaseAquarium();
        }
        if (uiObjectResponse.uiComponentName=="PurchaseAquariumEduComponent" && uiObjectResponse.uiObjectValue=="Cross") {
            OnCancelPurchaseAquarium();
        }
    }

    void OnOpenPurchaseAquariumUI(UIObjectResponse uiObjectResponse) {
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest(uiObjectResponse.uiComponentName,
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumEduComponent",
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_ENABLE));
        nextAquarium = new GameObject("NextAquarium");
        nextAquarium.layer = LayerMask.NameToLayer("Aquarium");
        nextAquariumSpriteRenderer = nextAquarium.AddComponent<SpriteRenderer>();
        nextAquariumSpriteRenderer.sprite = aquariumSprite;
        nextAquariumSpriteRenderer.color = new Color(0.25f,0.25f,0.25f);
        nextAquarium.transform.SetParent(playerController.transform, false);
        nextAquarium.transform.localPosition = new Vector3(aquariumSprite.rect.size.x/(aquariumSprite.pixelsPerUnit*2f)
                                                            +playerController.spriteRenderer.sprite.rect.size.x/(playerController.spriteRenderer.sprite.pixelsPerUnit*2f),
                                                           aquariumSprite.rect.size.y/(aquariumSprite.pixelsPerUnit*2f)
                                                            -playerController.spriteRenderer.sprite.rect.size.y/(playerController.spriteRenderer.sprite.pixelsPerUnit*2f),
                                                           0.1f);
        aquariumValidationRoutine = AquariumValidationRoutine(nextAquarium);
        StartCoroutine(aquariumValidationRoutine);
    }

    IEnumerator AquariumValidationRoutine(GameObject nextAquarium) {
        bool canPlace = false;
        nextAquariumSpriteRenderer.color = new Color(0,0,0);
        while (true) {
            //Use the OverlapBox to detect if there are any other colliders within this box area.
            //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(nextAquarium.transform.position, aquariumSprite.rect.size/aquariumSprite.pixelsPerUnit, 0, layerMask:LayerMask.GetMask("Aquarium"));
            if (canPlace && hitColliders.Length > 0) {
                // We can no longer place aquarium.
                uiObjectRequestEventChannel.RaiseEvent(new UIObjectRequest(
                    new UIObjectRuntimePropertiesId {
                        uiComponentName="PurchaseAquariumEduComponent",uiObjectName="Confirmation",uiObjectValue="CheckMark"},
                    UIObjectRequest.UIObjectRequestMode.REQUEST_MODE_DISABLE));
                uiObjectRequestEventChannel.RaiseEvent(new UIObjectRequest(
                    new UIObjectRuntimePropertiesId {
                        uiComponentName="PurchaseAquariumEduComponent",uiObjectName="PurchaseAquariumEdu",uiObjectValue=""},
                    UIObjectRequest.UIObjectRequestMode.REQUEST_MODE_RENDER));
                nextAquariumSpriteRenderer.color = new Color(0,0,0);
                canPlace = false;
            } else if (!canPlace && hitColliders.Length == 0) {
                // We can now place aquarium.
                uiObjectRequestEventChannel.RaiseEvent(new UIObjectRequest(
                    new UIObjectRuntimePropertiesId {
                        uiComponentName="PurchaseAquariumEduComponent",uiObjectName="Confirmation",uiObjectValue="CheckMark"},
                    UIObjectRequest.UIObjectRequestMode.REQUEST_MODE_ENABLE));
                uiObjectRequestEventChannel.RaiseEvent(new UIObjectRequest(
                    new UIObjectRuntimePropertiesId {
                        uiComponentName="PurchaseAquariumEduComponent",uiObjectName="PurchaseAquariumEdu",uiObjectValue=""},
                    UIObjectRequest.UIObjectRequestMode.REQUEST_MODE_RENDER));
                nextAquariumSpriteRenderer.color = new Color(0.25f,0.25f,0.25f);
                canPlace = true;
            }
            yield return 5;
        }
    }

    void OnPurchaseAquarium() {
        StopCoroutine(aquariumValidationRoutine);
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumEduComponent",
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
        nextAquarium.transform.SetParent(this.transform);
        BoxCollider2D aquariumCollider = nextAquarium.AddComponent<BoxCollider2D>();
        aquariumCollider.isTrigger = true;
        nextAquariumSpriteRenderer.color = new Color(1,1,1);
    }
    void OnCancelPurchaseAquarium() {
        StopCoroutine(aquariumValidationRoutine);
        uiComponentRequestEventChannel.RaiseEvent(new UIComponentRequest("PurchaseAquariumEduComponent",
                                                                         UIComponentRequest.UIComponentRequestMode.REQUEST_MODE_DISABLE));
        Destroy(nextAquarium);
    }
}
