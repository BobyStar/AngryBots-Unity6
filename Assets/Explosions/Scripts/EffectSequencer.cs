using UnityEngine;
using System.Collections;

[System.Serializable]
public class ExplosionPart : object
{
    public GameObject gameObject;
    public float delay;
    public bool hqOnly;
    public float yOffset;
}
[System.Serializable]
public partial class EffectSequencer : MonoBehaviour
{
    public ExplosionPart[] ambientEmitters;
    public ExplosionPart[] explosionEmitters;
    public ExplosionPart[] smokeEmitters;
    public ExplosionPart[] miscSpecialEffects;
    public virtual IEnumerator Start()
    {
        ExplosionPart go = null;
        float maxTime = 0;
        foreach (ExplosionPart go_20 in this.ambientEmitters)
        {
            go = go_20;
            this.StartCoroutine(this.InstantiateDelayed(go));
            if (go.gameObject.GetComponent<ParticleSystem>())
            {
                maxTime = Mathf.Max(maxTime, go.delay + go.gameObject.GetComponent<ParticleSystem>().main.startLifetime.Evaluate(0));
            }
        }
        foreach (ExplosionPart go_25 in this.explosionEmitters)
        {
            go = go_25;
            this.StartCoroutine(this.InstantiateDelayed(go));
            if (go.gameObject.GetComponent<ParticleSystem>())
            {
                maxTime = Mathf.Max(maxTime, go.delay + go.gameObject.GetComponent<ParticleSystem>().main.startLifetime.Evaluate(0));
            }
        }
        foreach (ExplosionPart go_30 in this.smokeEmitters)
        {
            go = go_30;
            this.StartCoroutine(this.InstantiateDelayed(go));
            if (go.gameObject.GetComponent<ParticleSystem>())
            {
                maxTime = Mathf.Max(maxTime, go.delay + go.gameObject.GetComponent<ParticleSystem>().main.startLifetime.Evaluate(0));
            }
        }
        if (this.GetComponent<AudioSource>() && this.GetComponent<AudioSource>().clip)
        {
            maxTime = Mathf.Max(maxTime, this.GetComponent<AudioSource>().clip.length);
        }
        yield return null;
        foreach (ExplosionPart go_41 in this.miscSpecialEffects)
        {
            go = go_41;
            this.StartCoroutine(this.InstantiateDelayed(go));
            if (go.gameObject.GetComponent<ParticleSystem>())
            {
                maxTime = Mathf.Max(maxTime, go.delay + go.gameObject.GetComponent<ParticleSystem>().main.startLifetime.Evaluate(0));
            }
        }
        UnityEngine.Object.Destroy(this.gameObject, maxTime + 0.5f);
    }

    public virtual IEnumerator InstantiateDelayed(ExplosionPart go)
    {
        if (go.hqOnly && (QualityManager.quality < Quality.High))
        {
            yield break;
        }
        yield return new WaitForSeconds(go.delay);
        UnityEngine.Object.Instantiate(go.gameObject, this.transform.position + (Vector3.up * go.yOffset), this.transform.rotation);
    }

}