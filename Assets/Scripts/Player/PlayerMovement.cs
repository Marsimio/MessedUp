using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private MMF_Player headBobFeedback;
    [SerializeField] private MMF_Player resetHeadBobFeedback;

    private Vector3 playerVelocity;

    private bool isGrounded;
    private bool wasMoving;

    public float gravity = -9.8f;

    private void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        moveDirection.x = input.x;
        moveDirection.z = input.y;

        bool isMoving = input.sqrMagnitude > 0.01f && isGrounded;

        if (isMoving && !wasMoving)
        {
            resetHeadBobFeedback?.StopFeedbacks();
            headBobFeedback?.PlayFeedbacks();
        }
        else if (!isMoving && wasMoving)
        {
            headBobFeedback?.StopFeedbacks();
            resetHeadBobFeedback?.PlayFeedbacks();
        }

        wasMoving = isMoving;

        controller.Move(
            transform.TransformDirection(moveDirection) *
            (StatsManager.Instance.speed * Time.deltaTime)
        );

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }
}