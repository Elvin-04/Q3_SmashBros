using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance {get; private set; }

    public AudioSource audioSource;

    public AudioClip deathSound;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayerDeath()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
    }
}
