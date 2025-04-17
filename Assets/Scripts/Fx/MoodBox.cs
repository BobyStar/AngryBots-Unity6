using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.RequireComponent(typeof(BoxCollider))]
public partial class MoodBox : MonoBehaviour
{
    public MoodBoxData data;
    public Cubemap playerReflection;
    private MoodBoxManager manager;
    public virtual void Start()
    {
        this.manager = ((MoodBoxManager) this.transform.parent.GetComponent(typeof(MoodBoxManager))) as MoodBoxManager;
        if (!this.manager)
        {
            Debug.Log(("Disabled moodbox " + this.gameObject.name) + " as a MoodBoxManager was not found.", this.transform);
            this.enabled = false;
        }
    }

    public virtual void OnDrawGizmos()
    {
        if (this.transform.parent)
        {
            Gizmos.color = new Color(0.5f, 0.9f, 1f, 0.15f);
            Gizmos.DrawCube(this.GetComponent<Collider>().bounds.center, this.GetComponent<Collider>().bounds.size);
        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        if (this.transform.parent)
        {
            Gizmos.color = new Color(0.5f, 0.9f, 1f, 0.75f);
            Gizmos.DrawCube(this.GetComponent<Collider>().bounds.center, this.GetComponent<Collider>().bounds.size);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.ApplyMoodBox();
        }
    }

    public virtual void ApplyMoodBox()
    {
        // optimization: deactivate rain stuff a little earlier
        if (this.manager.GetData().outside != this.data.outside)
        {
            foreach (GameObject m in this.manager.rainManagers)
            {
                m.SetActive(this.data.outside);
            }
            foreach (GameObject m in this.manager.splashManagers)
            {
                m.SetActive(this.data.outside);
            }
        }
        MoodBoxManager.current = this;
        if (this.manager.playerReflectionMaterials.Length != 0)
        {
            foreach (Material m in this.manager.playerReflectionMaterials)
            {
                m.SetTexture("_Cube", this.playerReflection ? this.playerReflection : this.manager.defaultPlayerReflection);
            }
        }
    }

}