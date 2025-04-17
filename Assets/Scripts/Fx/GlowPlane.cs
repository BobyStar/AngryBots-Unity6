using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GlowPlane : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 pos;
    private Vector3 scale;
    public float minGlow;
    public float maxGlow;
    public Color glowColor;
    private Material mat;
    public virtual void Start()
    {
        if (!this.playerTransform)
        {
            this.playerTransform = GameObject.FindWithTag("Player").transform;
        }
        this.pos = this.transform.position;
        this.scale = this.transform.localScale;
        this.mat = this.GetComponent<Renderer>().material;
        this.enabled = false;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = this.glowColor;

        {
            float _37 = this.maxGlow * 0.25f;
            Color _38 = Gizmos.color;
            _38.a = _37;
            Gizmos.color = _38;
        }
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Vector3 scale = 5f * Vector3.Scale(Vector3.one, new Vector3(1, 0, 1));
        Gizmos.DrawCube(Vector3.zero, scale);
        Gizmos.matrix = Matrix4x4.identity;
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = this.glowColor;

        {
            float _39 = this.maxGlow;
            Color _40 = Gizmos.color;
            _40.a = _39;
            Gizmos.color = _40;
        }
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Vector3 scale = 5f * Vector3.Scale(Vector3.one, new Vector3(1, 0, 1));
        Gizmos.DrawCube(Vector3.zero, scale);
        Gizmos.matrix = Matrix4x4.identity;
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
        Vector3 vec = this.pos - this.playerTransform.position;
        vec.y = 0f;
        float distance = vec.magnitude;
        this.transform.localScale = Vector3.Lerp(Vector3.one * this.minGlow, this.scale, Mathf.Clamp01(distance * 0.35f));
        this.mat.SetColor("_TintColor", this.glowColor * Mathf.Clamp(distance * 0.1f, this.minGlow, this.maxGlow));
    }

    public GlowPlane()
    {
        this.minGlow = 0.2f;
        this.maxGlow = 0.5f;
        this.glowColor = Color.white;
    }

}