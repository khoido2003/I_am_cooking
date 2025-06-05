using UnityEngine;

public class ResetDataStaticsManager : MonoBehaviour
{
    // Reset data from previous game state
    // This class is important to remove all the static event state since those state does not get reset by Unity so we need to do it by hand

    // For example: CuttingCounter, Basecounter, TrashCounter use static event
    // All those class need to reset static event so when go to new game, no error can happend since it can try to access old state game which is already get destroyed by Unity

    private void Awake()
    {
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
