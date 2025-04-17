using UnityEngine;
using System.Collections;

[System.Serializable]
public class RainManager : MonoBehaviour
{
    public float minYPosition;
    public int numberOfParticles;
    public float areaSize;
    public float areaHeight;
    public float fallingSpeed;
    public float particleSize;
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
            Debug.Log("Created new rain meshes in Assets/Objects/RainFx/");
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
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraUp = Vector3.up;
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
            position.y = this.areaHeight * Random.value;
            position.z = this.areaSize * (Random.value - 0.5f);
            float rand = Random.value;
            float widthWithRandom = this.particleSize * 0.215f;
            float heightWithRandom = this.particleSize + (rand * this.flakeRandom);// + rand * flakeRandom;
            verts[i4 + 0] = (position - (cameraRight * widthWithRandom)) - (cameraUp * heightWithRandom);
            verts[i4 + 1] = (position + (cameraRight * widthWithRandom)) - (cameraUp * heightWithRandom);
            verts[i4 + 2] = (position + (cameraRight * widthWithRandom)) + (cameraUp * heightWithRandom);
            verts[i4 + 3] = (position - (cameraRight * widthWithRandom)) + (cameraUp * heightWithRandom);
            normals[i4 + 0] = -Camera.main.transform.forward;
            normals[i4 + 1] = -Camera.main.transform.forward;
            normals[i4 + 2] = -Camera.main.transform.forward;
            normals[i4 + 3] = -Camera.main.transform.forward;
            uvs[i4 + 0] = new Vector2(0f, 0f);
            uvs[i4 + 1] = new Vector2(1f, 0f);
            uvs[i4 + 2] = new Vector2(1f, 1f);
            uvs[i4 + 3] = new Vector2(0f, 1f);
            uvs2[i4 + 0] = new Vector2(Random.Range(-2, 2) * 4f, Random.Range(-1, 1) * 1f);
            uvs2[i4 + 1] = new Vector2(uvs2[i4 + 0].x, uvs2[i4 + 0].y);
            uvs2[i4 + 2] = new Vector2(uvs2[i4 + 0].x, uvs2[i4 + 0].y);
            uvs2[i4 + 3] = new Vector2(uvs2[i4 + 0].x, uvs2[i4 + 0].y);
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

    public RainManager()
    {
        this.numberOfParticles = 400;
        this.areaSize = 40f;
        this.areaHeight = 15f;
        this.fallingSpeed = 23f;
        this.particleSize = 0.2f;
        this.flakeRandom = 0.1f;
    }

}