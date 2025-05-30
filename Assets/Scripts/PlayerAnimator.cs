using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";

    [SerializeField]
    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Aniamtion start");
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
