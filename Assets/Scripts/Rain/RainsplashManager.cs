using UnityEngine;
using System.Collections;

[System.Serializable]
public class RainsplashManager : MonoBehaviour
{
    public int numberOfParticles;
    public float areaSize;
    public float areaHeight;
    public float fallingSpeed;
    public float flakeWidth;
    public float flakeHeight;
    public float flakeRandom;
    public Mesh[] preGennedMeshes;
    private int preGennedIndex;
    public bool generateNewAssetsOnStart;
    public virtual void Start()
    {
        if (this.generateNewAssetsOnStart)
        {
            // create & save 3 meshes
            Mesh m1 = this.CreateMesh();
            Mesh m2 = this.CreateMesh();
            Mesh m3 = this.CreateMesh();
            UnityEditor.AssetDatabase.CreateAsset(m1, ("Assets/Objects/RainFx/" + this.gameObject.name) + "_LQ0.asset");
            UnityEditor.AssetDatabase.CreateAsset(m2, ("Assets/Objects/RainFx/" + this.gameObject.name) + "_LQ1.asset");
            UnityEditor.AssetDatabase.CreateAsset(m3, ("Assets/Objects/RainFx/" + this.gameObject.name) + "_LQ2.asset");
            Debug.Log("Created new rainsplash meshes in Assets/Objects/RainFx/");
        }
    }

    public virtual Mesh GetPreGennedMesh()
    {
        return this.preGennedMeshes[this.preGennedIndex++ % this.preGennedMeshes.Length];
    }

    public virtual Mesh CreateMesh()
    {
        Vector3 position = default(Vector3);
        Mesh mesh = new Mesh();
        // we use world space aligned and not camera aligned planes this time
        Vector3 cameraRight = (this.transform.right * Random.Range(0.1f, 2f)) + (this.transform.forward * Random.Range(0.1f, 2f));// Vector3.forward;//Camera.main.transform.right;
        cameraRight = Vector3.Normalize(cameraRight);
        Vector3 cameraUp = Vector3.Cross(cameraRight, Vector3.up);
        cameraUp = Vector3.Normalize(cameraUp);
        int particleNum = QualityManager.quality > Quality.Medium ? this.numberOfParticles : this.numberOfParticles / 2;
        Vector3[] verts = new Vector3[4 * particleNum];
        Vector2[] uvs = new Vector2[4 * particleNum];
        Vector2[] uvs2 = new Vector2[4 * particleNum];
        Vector3[] normals = new Vector3[4 * particleNum];
        int[] tris = new int[(2 * 3) * particleNum];
        int i = 0;
        while (i < particleNum)
        {
            int i4 = i * 4;
            int i6 = i * 6;
            position.x = this.areaSize * (Random.value - 0.5f);
            position.y = 0f;
            position.z = this.areaSize * (Random.value - 0.5f);
            float rand = Random.value;
            float widthWithRandom = this.flakeWidth + (rand * this.flakeRandom);
            float heightWithRandom = widthWithRandom;
            verts[i4 + 0] = position - (cameraRight * widthWithRandom);// - 0.0 * heightWithRandom;
            verts[i4 + 1] = position + (cameraRight * widthWithRandom);// - 0.0 * heightWithRandom;
            verts[i4 + 2] = (position + (cameraRight * widthWithRandom)) + ((cameraUp * 2f) * heightWithRandom);
            verts[i4 + 3] = (position - (cameraRight * widthWithRandom)) + ((cameraUp * 2f) * heightWithRandom);
            normals[i4 + 0] = -Camera.main.transform.forward;
            normals[i4 + 1] = -Camera.main.transform.forward;
            normals[i4 + 2] = -Camera.main.transform.forward;
            normals[i4 + 3] = -Camera.main.transform.forward;
            uvs[i4 + 0] = new Vector2(0f, 0f);
            uvs[i4 + 1] = new Vector2(1f, 0f);
            uvs[i4 + 2] = new Vector2(1f, 1f);
            uvs[i4 + 3] = new Vector2(0f, 1f);
            Vector2 tc1 = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            uvs2[i4 + 0] = new Vector2(tc1.x, tc1.y);
            uvs2[i4 + 1] = new Vector2(tc1.x, tc1.y);
            uvs2[i4 + 2] = new Vector2(tc1.x, tc1.y);
            uvs2[i4 + 3] = new Vector2(tc1.x, tc1.y);
            tris[i6 + 0] = i4 + 0;
            tris[i6 + 1] = i4 + 1;
            tris[i6 + 2] = i4 + 2;
            tris[i6 + 3] = i4 + 0;
            tris[i6 + 4] = i4 + 2;
            tris[i6 + 5] = i4 + 3;
            i++;
        }
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.uv2 = uvs2;
        mesh.RecalculateBounds();
        return mesh;
    }

    public RainsplashManager()
    {
        this.numberOfParticles = 700;
        this.areaSize = 40f;
        this.areaHeight = 15f;
        this.fallingSpeed = 23f;
        this.flakeWidth = 0.4f;
        this.flakeHeight = 0.4f;
        this.flakeRandom = 0.1f;
    }

}