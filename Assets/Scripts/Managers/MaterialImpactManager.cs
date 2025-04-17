using UnityEngine;
using System.Collections;

[System.Serializable]
public class MaterialImpact : object
{
    public PhysicsMaterial physicMaterial;
    public AudioClip[] playerFootstepSounds;
    public AudioClip[] mechFootstepSounds;
    public AudioClip[] spiderFootstepSounds;
    public AudioClip[] bulletHitSounds;
}
[System.Serializable]
public class MaterialImpactManager : MonoBehaviour
{
    public MaterialImpact[] materials;
    private static System.Collections.Generic.Dictionary<PhysicsMaterial, MaterialImpact> dict;
    private static MaterialImpact defaultMat;
    public virtual void Awake()
    {
        MaterialImpactManager.defaultMat = this.materials[0];
        MaterialImpactManager.dict = new System.Collections.Generic.Dictionary<PhysicsMaterial, MaterialImpact>();
        int i = 0;
        while (i < this.materials.Length)
        {
            MaterialImpactManager.dict.Add(this.materials[i].physicMaterial, this.materials[i]);
            i++;
        }
    }

    public static AudioClip GetPlayerFootstepSound(PhysicsMaterial mat)
    {
        MaterialImpact imp = MaterialImpactManager.GetMaterialImpact(mat);
        return MaterialImpactManager.GetRandomSoundFromArray(imp.playerFootstepSounds);
    }

    public static AudioClip GetMechFootstepSound(PhysicsMaterial mat)
    {
        MaterialImpact imp = MaterialImpactManager.GetMaterialImpact(mat);
        return MaterialImpactManager.GetRandomSoundFromArray(imp.mechFootstepSounds);
    }

    public static AudioClip GetSpiderFootstepSound(PhysicsMaterial mat)
    {
        MaterialImpact imp = MaterialImpactManager.GetMaterialImpact(mat);
        return MaterialImpactManager.GetRandomSoundFromArray(imp.spiderFootstepSounds);
    }

    public static AudioClip GetBulletHitSound(PhysicsMaterial mat)
    {
        MaterialImpact imp = MaterialImpactManager.GetMaterialImpact(mat);
        return MaterialImpactManager.GetRandomSoundFromArray(imp.bulletHitSounds);
    }

    public static MaterialImpact GetMaterialImpact(PhysicsMaterial mat)
    {
        if (mat && MaterialImpactManager.dict.ContainsKey(mat))
        {
            return MaterialImpactManager.dict[mat];
        }
        return MaterialImpactManager.defaultMat;
    }

    public static AudioClip GetRandomSoundFromArray(AudioClip[] audioClipArray)
    {
        if (audioClipArray.Length > 0)
        {
            return audioClipArray[Random.Range(0, audioClipArray.Length)];
        }
        return null;
    }

}