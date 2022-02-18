using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float crouchingSpeed = 3.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    private float crouchHeight;
    private float standingHeight;
    private float timeToCrouch = 0.25f;

    private Vector3 crouchingCenter;
    private Vector3 standingCenter;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    private bool Should_Player_Crouch => Input.GetKeyDown(Crouch_Key) && !During_Player_Crouch_Animation && characterController.isGrounded;
    private bool During_Player_Crouch_Animation;
    private KeyCode Crouch_Key = KeyCode.LeftControl;
    private bool isCrouching;
    private bool canCrouch = true;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        standingHeight = characterController.height;
        crouchHeight = standingHeight / 4;

        standingCenter = characterController.center;
        crouchingCenter = new Vector3(standingCenter.x, standingCenter.y + 0.5f, standingCenter.z);


        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Assigning Keys to player
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Press Left Shift to run
        float curSpeedX = canMove ? (isCrouching ? crouchingSpeed : isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (characterController.isGrounded ? (isCrouching ? crouchingSpeed : isRunning ? runningSpeed : walkingSpeed) : lookSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Press Left Control to crouch
        if (canCrouch)
        {
            HandleCrouch();
        }


        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void HandleCrouch()
    {
        if (Should_Player_Crouch)
        {
            StartCoroutine(Crouch_Stand());
        }
    }

    private IEnumerator Crouch_Stand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
        {
            yield break;
        }

        During_Player_Crouch_Animation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, (timeElapsed / timeToCrouch));
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, (timeElapsed / timeToCrouch));

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;
        During_Player_Crouch_Animation = false;
    }
}
