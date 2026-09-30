using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        animator.SetBool("isRunning", movement.IsRunning);
        animator.SetBool("isJumping", movement.IsJumping);
        // TODO: should these two have the vertical value instead
        // of two separated booleans? The smell is that they
        // are exclusive, i.e. can never be both true at the same time
        animator.SetBool("isAimingUp", movement.IsAimingUp);
        animator.SetBool("isAimingDown", movement.IsAimingDown);
    }
}
