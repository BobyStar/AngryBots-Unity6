using UnityEngine;
using System.Collections;

[System.Serializable]
public class RainsplashBox : MonoBehaviour
{
    private MeshFilter mf;
    //private Bounds bounds;
    private RainsplashManager manager;
    public virtual void Start()
    {
        this.transform.localRotation = Quaternion.identity;
        this.manager = this.transform.parent.GetComponent<RainsplashManager>();
        //this.bounds = new Bounds(new Vector3(this.transform.position.x, 0f, this.transform.position.z), new Vector3(this.manager.areaSize, Mathf.Max(this.manager.areaSize, this.manager.areaHeight), this.manager.areaSize));
        this.mf = this.GetComponent<MeshFilter>();
        this.mf.sharedMesh = this.manager.GetPreGennedMesh();
        this.enabled = false;
    }

    public virtual void OnBecameVisible()
    {
        this.enabled = true;
    }

    public virtual void OnBecameInvisible()
    {
        this.enabled = false;
    }

    public virtual void OnDrawGizmos()
    {
        if (this.transform.parent)
        {
            this.manager = this.transform.parent.GetComponent<RainsplashManager>();
            Gizmos.color = new Color(0.5f, 0.5f, 0.65f, 0.5f);
            if (this.manager)
            {
                Gizmos.DrawWireCube(this.transform.position + ((this.transform.up * this.manager.areaHeight) * 0.5f), new Vector3(this.manager.areaSize, this.manager.areaHeight, this.manager.areaSize));
            }
        }
    }

}