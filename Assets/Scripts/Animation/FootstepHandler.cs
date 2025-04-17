using UnityEngine;
using System.Collections;

public enum FootType
{
    Player = 0,
    Mech = 1,
    Spider = 2
}

[System.Serializable]
public partial class FootstepHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public FootType footType;
    private PhysicsMaterial physicMaterial;
    public virtual void OnCollisionEnter(Collision collisionInfo)
    {
        this.physicMaterial = collisionInfo.collider.sharedMaterial;
    }

    public virtual void OnFootstep()
    {
        if (!this.audioSource.enabled)
        {
            return;
        }
        AudioClip sound = null;
        switch (this.footType)
        {
            case FootType.Player:
                sound = MaterialImpactManager.GetPlayerFootstepSound(this.physicMaterial);
                break;
            case FootType.Mech:
                sound = MaterialImpactManager.GetMechFootstepSound(this.physicMaterial);
                break;
            case FootType.Spider:
                sound = MaterialImpactManager.GetSpiderFootstepSound(this.physicMaterial);
                break;
        }
        
        if (!sound)
            return;
        
        this.audioSource.pitch = Random.Range(0.98f, 1.02f);
        this.audioSource.PlayOneShot(sound, Random.Range(0.8f, 1.2f));
    }

}