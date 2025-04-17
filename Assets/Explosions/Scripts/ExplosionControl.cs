using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ExplosionControl : MonoBehaviour
{
    public GameObject[] trails;
    public ParticleSystem emitter;
    public LineRenderer[] lineRenderer;
    public GameObject lightDecal;
    public float autoDisableAfter;
    public virtual void Awake()
    {
        int i = 0;
        while (i < this.lineRenderer.Length)
        {
            float lineWidth = Random.Range(0.25f, 0.5f);
            this.lineRenderer[i].startWidth = lineWidth;
            this.lineRenderer[i].endWidth = lineWidth;
            this.lineRenderer[i].SetPosition(0, Vector3.zero);
            Vector3 dir = Random.onUnitSphere;
            dir.y = Mathf.Abs(dir.y);
            this.lineRenderer[i].SetPosition(1, dir * Random.Range(8f, 12f));
            i++;
        }
    }

    public virtual void OnEnable()
    {
        this.lightDecal.transform.localScale = Vector3.one;
        this.lightDecal.SetActive(true);
        int i = 0;
        while (i < this.trails.Length)
        {
            this.trails[i].transform.localPosition = Vector3.zero;
            this.trails[i].SetActive(true);
            (((ExplosionTrail) this.trails[i].GetComponent(typeof(ExplosionTrail))) as ExplosionTrail).enabled = true;
            i++;
        }
        i = 0;
        while (i < this.lineRenderer.Length)
        {
            this.lineRenderer[i].transform.localPosition = Vector3.zero;
            this.lineRenderer[i].gameObject.SetActive(true);
            this.lineRenderer[i].enabled = true;
            i++;
        }
        this.emitter.Play();
        this.emitter.gameObject.SetActive(true);
        this.Invoke("DisableEmitter", this.emitter.main.startLifetime.Evaluate(0));
        this.Invoke("DisableStuff", this.autoDisableAfter);
    }

    public virtual void DisableEmitter()
    {
        this.emitter.Play();
    }

    public virtual void DisableStuff()
    {
        this.gameObject.SetActive(false);
    }

    public ExplosionControl()
    {
        this.autoDisableAfter = 2f;
    }

}