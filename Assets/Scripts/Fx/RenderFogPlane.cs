using UnityEngine;
using System.Collections;

[System.Serializable]
[ExecuteInEditMode]
public class RenderFogPlane : MonoBehaviour
{
    public Camera cameraForRay;
    private Matrix4x4 frustumCorners;
    private float CAMERA_ASPECT_RATIO;
    private float CAMERA_NEAR;
    private float CAMERA_FAR;
    private float CAMERA_FOV;
    private Mesh mesh;
    private Vector2[] uv;
    public virtual void OnEnable()
    {
        this.GetComponent<Renderer>().enabled = true;
        if (!this.mesh)
        {
            this.mesh = ((MeshFilter) this.GetComponent(typeof(MeshFilter))).sharedMesh;
        }
        // write indices into uv's for fast world space reconstruction		
        if (this.mesh)
        {
            this.uv[0] = new Vector2(1f, 1f); // TR
            this.uv[1] = new Vector2(0f, 0f); // TL
            this.uv[2] = new Vector2(2f, 2f); // BR
            this.uv[3] = new Vector2(3f, 3f); // BL
            this.mesh.uv = this.uv;
        }
        if (!this.cameraForRay)
        {
            this.cameraForRay = Camera.main;
        }

        if (this.cameraForRay)
        {
            this.cameraForRay.depthTextureMode = DepthTextureMode.Depth;
        }
    }

    private bool EarlyOutIfNotSupported()
    {
        if (!this.Supported())
        {
            this.enabled = false;
            return true;
        }
        return false;
    }

    public virtual void OnDisable()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    public virtual bool Supported()
    {
        return this.GetComponent<Renderer>().sharedMaterial.shader.isSupported && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
    }

    public virtual void Update()
    {
        //Ray ray = default(Ray);
        //Vector4 vec = default(Vector4);
        //Vector3 corner = default(Vector3);
        if (this.EarlyOutIfNotSupported())
        {
            this.enabled = false;
            return;
        }
        Renderer r = this.GetComponent<Renderer>();
        if (!r.enabled)
        {
            return;
        }
        this.frustumCorners = Matrix4x4.identity;
        this.CAMERA_NEAR = this.cameraForRay.nearClipPlane;
        this.CAMERA_FAR = this.cameraForRay.farClipPlane;
        this.CAMERA_FOV = this.cameraForRay.fieldOfView;
        this.CAMERA_ASPECT_RATIO = this.cameraForRay.aspect;
        float fovWHalf = this.CAMERA_FOV * 0.5f;
        Vector3 toRight = this.cameraForRay.transform.right * (this.CAMERA_NEAR * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * this.CAMERA_ASPECT_RATIO);
        Vector3 toTop = this.cameraForRay.transform.up * (this.CAMERA_NEAR * Mathf.Tan(fovWHalf * Mathf.Deg2Rad));
        Vector3 topLeft = ((this.cameraForRay.transform.forward * this.CAMERA_NEAR) - toRight) + toTop;
        float CAMERA_SCALE = (topLeft.magnitude * this.CAMERA_FAR) / this.CAMERA_NEAR;

        this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, this.CAMERA_NEAR + 0.0001f);

        this.transform.localScale = new Vector3((toRight * 0.5f).magnitude, 1, (toTop * 0.5f).magnitude);
        
        this.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
        
        
        // write view frustum corner "rays"
        topLeft.Normalize();
        topLeft = topLeft * CAMERA_SCALE;
        Vector3 topRight = ((this.cameraForRay.transform.forward * this.CAMERA_NEAR) + toRight) + toTop;
        topRight.Normalize();
        topRight = topRight * CAMERA_SCALE;
        Vector3 bottomRight = ((this.cameraForRay.transform.forward * this.CAMERA_NEAR) + toRight) - toTop;
        bottomRight.Normalize();
        bottomRight = bottomRight * CAMERA_SCALE;
        Vector3 bottomLeft = ((this.cameraForRay.transform.forward * this.CAMERA_NEAR) - toRight) - toTop;
        bottomLeft.Normalize();
        bottomLeft = bottomLeft * CAMERA_SCALE;
        this.frustumCorners.SetRow(0, topLeft);
        this.frustumCorners.SetRow(1, topRight);
        this.frustumCorners.SetRow(2, bottomRight);
        this.frustumCorners.SetRow(3, bottomLeft);
        r.sharedMaterial.SetMatrix("_FrustumCornersWS", this.frustumCorners);
        r.sharedMaterial.SetVector("_CameraWS", this.cameraForRay.transform.position);
    }

    public RenderFogPlane()
    {
        this.CAMERA_ASPECT_RATIO = 1.333333f;
        this.uv = new Vector2[4];
    }

}