using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsCelebratingHash = Animator.StringToHash("isCelebrating");
    private static readonly int IsAimingDownHash = Animator.StringToHash("isAimingDown");
    private static readonly int IsAimingUpHash = Animator.StringToHash("isAimingUp");
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

    private Animator animator;
    private PlayerMovement movement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        animator.SetBool(IsRunningHash, movement.IsRunning);
        animator.SetBool(IsJumpingHash, movement.IsJumping);
        // TODO: should these two have the vertical value instead
        // of two separated booleans? The smell is that they
        // are exclusive, i.e. can never be both true at the same time
        animator.SetBool(IsAimingUpHash, movement.IsAimingUp);
        animator.SetBool(IsAimingDownHash, movement.IsAimingDown);
    }

    public void OnLevelFinish()
    {
        animator.SetBool(IsCelebratingHash, true);
    }
}
