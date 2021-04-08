using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private bool m_canPlaySound;

    private void Awake()
    {
        if (audioSource != null && audioClip != null)
        {
            m_canPlaySound = true;
        }
    }


    public void Play()
    {
        if (m_canPlaySound)
            audioSource.PlayOneShot(audioClip);
    }
}
