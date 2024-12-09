using UnityEngine;

public class Door : MonoBehaviour
{
    public Lever lever;
    public BoxCollider boxCollider;
    public Animator animator;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        lever.onLeverOn.AddListener(OpenDoor);
        lever.onLeverOff.AddListener(CloseDoor);
    }

    private void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        boxCollider.enabled = false;
    }

    private void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");
        boxCollider.enabled = false;
    }
}
