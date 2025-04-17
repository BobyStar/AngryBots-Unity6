using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.RequireComponent(typeof(PerFrameRaycast))]
public partial class AutoFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float frequency;
    public float coneAngle;
    public bool firing;
    public float damagePerSecond;
    public float forcePerSecond;
    public float hitSoundVolume;
    public GameObject muzzleFlashFront;
    private float lastFireTime;
    private PerFrameRaycast raycast;
    public virtual void Awake()
    {
        this.muzzleFlashFront.SetActive(false);
        this.raycast = this.GetComponent<PerFrameRaycast>();
        if (this.spawnPoint == null)
        {
            this.spawnPoint = this.transform;
        }
    }

    public virtual void Update()
    {
        if (this.firing)
        {
            if (Time.time > (this.lastFireTime + (1 / this.frequency)))
            {
                // Spawn visual bullet
                Quaternion coneRandomRotation = Quaternion.Euler(Random.Range(-this.coneAngle, this.coneAngle), Random.Range(-this.coneAngle, this.coneAngle), 0);
                GameObject go = Spawner.Spawn(this.bulletPrefab, this.spawnPoint.position, this.spawnPoint.rotation * coneRandomRotation) as GameObject;
                SimpleBullet bullet = go.GetComponent<SimpleBullet>();
                this.lastFireTime = Time.time;
                // Find the object hit by the raycast
                RaycastHit hitInfo = this.raycast.GetHitInfo();
                if (hitInfo.transform)
                {
                    // Get the health component of the target if any
                    Health targetHealth = hitInfo.transform.GetComponent<Health>();
                    if (targetHealth)
                    {
                        // Apply damage
                        targetHealth.OnDamage(this.damagePerSecond / this.frequency, -this.spawnPoint.forward);
                    }
                    // Get the rigidbody if any
                    if (hitInfo.rigidbody)
                    {
                        // Apply force to the target object at the position of the hit point
                        Vector3 force = this.transform.forward * (this.forcePerSecond / this.frequency);
                        hitInfo.rigidbody.AddForceAtPosition(force, hitInfo.point, ForceMode.Impulse);
                    }
                    // Ricochet sound
                    AudioClip sound = MaterialImpactManager.GetBulletHitSound(hitInfo.collider.sharedMaterial);
                    AudioSource.PlayClipAtPoint(sound, hitInfo.point, this.hitSoundVolume);
                    bullet.dist = hitInfo.distance;
                }
                else
                {
                    bullet.dist = 1000;
                }
            }
        }
    }

    public virtual void OnStartFire()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        this.firing = true;
        this.muzzleFlashFront.SetActive(true);
        if (this.GetComponent<AudioSource>())
        {
            this.GetComponent<AudioSource>().Play();
        }
    }

    public virtual void OnStopFire()
    {
        this.firing = false;
        this.muzzleFlashFront.SetActive(false);
        if (this.GetComponent<AudioSource>())
        {
            this.GetComponent<AudioSource>().Stop();
        }
    }

    public AutoFire()
    {
        this.frequency = 10;
        this.coneAngle = 1.5f;
        this.damagePerSecond = 20f;
        this.forcePerSecond = 20f;
        this.hitSoundVolume = 0.5f;
        this.lastFireTime = -1;
    }

}