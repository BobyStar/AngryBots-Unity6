using UnityEngine;
using System.Collections;

[System.Serializable]
public class RainBox : MonoBehaviour
{
    private MeshFilter mf;
    private Vector3 defaultPosition;
    //private Bounds bounds;
    private RainManager manager;
    private Transform cachedTransform;
    private float cachedMinY;
    private float cachedAreaHeight;
    private float cachedFallingSpeed;
    public virtual void Start()
    {
        this.manager = this.transform.parent.GetComponent<RainManager>();
        //this.bounds = new Bounds(new Vector3(this.transform.position.x, this.manager.minYPosition, this.transform.position.z), new Vector3(this.manager.areaSize * 1.35f, Mathf.Max(this.manager.areaSize, this.manager.areaHeight) * 1.35f, this.manager.areaSize * 1.35f));
        this.mf = this.GetComponent<MeshFilter>();
        this.mf.sharedMesh = this.manager.GetPreGennedMesh();
        this.cachedTransform = this.transform;
        this.cachedMinY = this.manager.minYPosition;
        this.cachedAreaHeight = this.manager.areaHeight;
        this.cachedFallingSpeed = this.manager.fallingSpeed;
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

    public virtual void Update()
    {
        this.cachedTransform.position = this.cachedTransform.position - ((Vector3.up * Time.deltaTime) * this.cachedFallingSpeed);
        if ((this.cachedTransform.position.y + this.cachedAreaHeight) < this.cachedMinY)
        {
            this.cachedTransform.position = this.cachedTransform.position + ((Vector3.up * this.cachedAreaHeight) * 2f);
        }
    }

    public virtual void OnDrawGizmos()
    {
        // do not display a weird mesh in edit mode
        if (!Application.isPlaying)
        {
            this.mf = this.GetComponent<MeshFilter>();
            this.mf.sharedMesh = null;
        }
        if (this.transform.parent)
        {
            Gizmos.color = new Color(0.2f, 0.3f, 3f, 0.35f);
            RainManager manager = ((RainManager) this.transform.parent.GetComponent(typeof(RainManager))) as RainManager;
            if (manager)
            {
                Gizmos.DrawWireCube(this.transform.position + ((this.transform.up * manager.areaHeight) * 0.5f), new Vector3(manager.areaSize, manager.areaHeight, manager.areaSize));
            }
        }
    }

}