using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;
    private Vector3 moveDirection;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDirection = new Vector3(h, 0, v);

        if (moveDirection.magnitude > 0.1f)
        {
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}
