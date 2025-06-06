using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectsVolume";

    [SerializeField]
    AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        // Store user data between section
        PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnSuccess;

        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnFailed;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        Player.Instance.OnPickedSomething += Player_OnPickedSomething;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;

        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObject;
    }

    private void TrashCounter_OnAnyObject(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnSuccess(object sender, System.EventArgs e)
    {
        DeliverCounter deliverCounter = DeliverCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliverCounter.transform.position);
    }

    private void DeliveryManager_OnFailed(object sender, System.EventArgs e)
    {
        DeliverCounter deliverCounter = DeliverCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliverCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayCountdonwSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    

    public void PlayWarningSound(Vector3  pos) {
        PlaySound(audioClipRefsSO.warning, pos);
    }

    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
