using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    private Animator animator;

    [SerializeField]
    private CuttingCounter cuttingCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.ResetTrigger(CUT);
    }

    private void Start()
    {
        // Listen Event
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
