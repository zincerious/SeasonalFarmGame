using System.Collections;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioClip digSound;
    public AudioClip plantSound;
    public AudioClip waterSound;
    public AudioClip harvestSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayDig() => audioSource.PlayOneShot(digSound);
    public void PlayPlant() => audioSource.PlayOneShot(plantSound);
    public IEnumerator PlayWater()
    {
        audioSource.PlayOneShot(waterSound);
        yield return new WaitForSeconds(4f);
        audioSource.Stop();
    }
    public void PlayHarvest() => audioSource.PlayOneShot(harvestSound);
}
