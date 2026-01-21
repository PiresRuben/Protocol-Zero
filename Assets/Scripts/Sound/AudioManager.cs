using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource src;

    public List<AudioClip> footSteps;

    private AudioClip lastFootClip;

    public static AudioManager GetInstance()
    {
        return Instance;
    }

    private void Reset()
    {
        src = GetComponent<AudioSource>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void ChangeSoundTrack(AudioClip clip)
    {
        src.clip = clip;
        src.Play();
    }

    public void PlayFootStep()
    {
        AudioClip currentAudio = footSteps[Random.Range(0, footSteps.Count)];

        if (currentAudio == lastFootClip) currentAudio = footSteps[Random.Range(0, footSteps.Count)];  // Pour reduire les chance d'avoir deux fois le mm audio
        lastFootClip = currentAudio;
        src.clip = currentAudio;
        src.Play();
    }
}
