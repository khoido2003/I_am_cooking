using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField]
    private GameObject stoveGameObject;

    [SerializeField]
    private GameObject particlesGameObject;

    [SerializeField]
    private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual =
            e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        stoveGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
