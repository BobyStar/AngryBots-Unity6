using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SpawnAtCheckpoint : MonoBehaviour
{
    public Transform checkpoint;
    public virtual void OnSignal()
    {
        this.transform.position = this.checkpoint.position;
        this.transform.rotation = this.checkpoint.rotation;
        SpawnAtCheckpoint.ResetHealthOnAll();
    }

    public static void ResetHealthOnAll()
    {
        Health[] healthObjects = (Health[]) FindObjectsByType(typeof(Health), FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Health health in healthObjects)
        {
            health.dead = false;
            health.health = health.maxHealth;
        }
    }

}