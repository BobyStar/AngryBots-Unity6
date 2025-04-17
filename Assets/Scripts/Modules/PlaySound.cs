using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;
    public virtual void Awake()
    {
        if (!this.audioSource && this.GetComponent<AudioSource>())
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }
    }

    public virtual void OnSignal()
    {
        if (this.sound)
        {
            this.audioSource.clip = this.sound;
        }
        this.audioSource.Play();
    }

}