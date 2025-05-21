using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    private Animator animator;

    [SerializeField]
    private ContainerCounter containerCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.ResetTrigger(OPEN_CLOSE);
    }

    private void Start()
    {
        // Listen Event
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
