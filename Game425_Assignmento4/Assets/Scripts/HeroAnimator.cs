using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;
    private Vector3 moveDirection;

    void Update()
    {
        // Get input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDirection = new Vector3(h, 0, v);

        // Movement
        if (moveDirection.magnitude > 0.1f)
        {
            // Move hero
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);

            // Rotate hero to face direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Animator parameters
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            // Idle
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }
}
