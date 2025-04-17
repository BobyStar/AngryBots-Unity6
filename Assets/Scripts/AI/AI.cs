using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AI : MonoBehaviour
{
    // Public member data
    public MonoBehaviour behaviourOnSpotted;
    public AudioClip soundOnSpotted;
    public MonoBehaviour behaviourOnLostTrack;
    // Private memeber data
    private Transform character;
    private Transform player;
    private bool insideInterestArea;
    public virtual void Awake()
    {
        this.character = this.transform;
        this.player = GameObject.FindWithTag("Player").transform;
    }

    public virtual void OnEnable()
    {
        this.behaviourOnLostTrack.enabled = true;
        this.behaviourOnSpotted.enabled = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if ((other.transform == this.player) && this.CanSeePlayer())
        {
            this.OnSpotted();
        }
    }

    public virtual void OnEnterInterestArea()
    {
        this.insideInterestArea = true;
    }

    public virtual void OnExitInterestArea()
    {
        this.insideInterestArea = false;
        this.OnLostTrack();
    }

    public virtual void OnSpotted()
    {
        if (!this.insideInterestArea)
        {
            return;
        }
        if (!this.behaviourOnSpotted.enabled)
        {
            this.behaviourOnSpotted.enabled = true;
            this.behaviourOnLostTrack.enabled = false;
            if (this.GetComponent<AudioSource>() && this.soundOnSpotted)
            {
                this.GetComponent<AudioSource>().clip = this.soundOnSpotted;
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    public virtual void OnLostTrack()
    {
        if (!this.behaviourOnLostTrack.enabled)
        {
            this.behaviourOnLostTrack.enabled = true;
            this.behaviourOnSpotted.enabled = false;
        }
    }

    public virtual bool CanSeePlayer()
    {
        RaycastHit hit = default(RaycastHit);
        Vector3 playerDirection = this.player.position - this.character.position;
        Physics.Raycast(this.character.position, playerDirection, out hit, playerDirection.magnitude);
        if (hit.collider && (hit.collider.transform == this.player))
        {
            return true;
        }
        return false;
    }

    public AI()
    {
        this.insideInterestArea = true;
    }

}