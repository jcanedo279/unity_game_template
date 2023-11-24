using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {
    PlayerInputResponseEventChannelListener playerMoveEventChannelListener;
    Animator animator;
    Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private float movementSpeed;
    private Vector3 movementVector;
    private float maxSpeed = 5f;
    private PlayerAnimationState currentAnimationState;

    public enum PlayerAnimationState {
        PlayerAnimationState_Idle,
        PlayerAnimationState_Walk
    }

    private Dictionary<PlayerAnimationState,string> animation_state_to_name = new Dictionary<PlayerAnimationState, string>() {
        {PlayerAnimationState.PlayerAnimationState_Idle, "PlayerIdle"},
        {PlayerAnimationState.PlayerAnimationState_Walk, "PlayerWalk"}
    };

    public void Awake() {
        animator = GetComponent<Animator>();
        animator.Play(animation_state_to_name[PlayerAnimationState.PlayerAnimationState_Idle]);
        currentAnimationState = PlayerAnimationState.PlayerAnimationState_Idle;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerMoveEventChannelListener = GetComponent<PlayerInputResponseEventChannelListener>();
        playerMoveEventChannelListener.UnityEventResponse += OnPlayerInputResponseEvent;
    }

    public void OnPlayerInputResponseEvent(PlayerInputResponse playerInputResponse) {
        // Flip the player sprite when the transform's scale direction (sprite's direction) is different from the input's.
        if (transform.localScale.x>0 && playerInputResponse.moveVector.x < 0 ||
            transform.localScale.x<0 && playerInputResponse.moveVector.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
        movementVector = transform.right*playerInputResponse.moveVector.normalized.x;
        rigidBody.AddForce(movementVector, ForceMode2D.Impulse);
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeed);

        if (currentAnimationState == PlayerAnimationState.PlayerAnimationState_Idle && Mathf.Abs(playerInputResponse.moveVector.x) > 0) {
            animator.Play(animation_state_to_name[PlayerAnimationState.PlayerAnimationState_Walk]);
            currentAnimationState = PlayerAnimationState.PlayerAnimationState_Walk;
        } else if (currentAnimationState == PlayerAnimationState.PlayerAnimationState_Walk && playerInputResponse.moveVector.x == 0) {
            animator.Play(animation_state_to_name[PlayerAnimationState.PlayerAnimationState_Idle]);
            currentAnimationState = PlayerAnimationState.PlayerAnimationState_Idle;
        }
    }
}
