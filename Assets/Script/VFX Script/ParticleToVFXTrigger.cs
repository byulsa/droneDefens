using UnityEngine;
using UnityEngine.VFX;

public class ParticleToVFXTrigger : MonoBehaviour
{
    private ParticleSystem targetParticle;
    private VisualEffect targetVFX;
    private bool wasPlaying = false;

    void Awake()
    {
        targetParticle = GetComponent<ParticleSystem>();
        targetVFX = GetComponentInChildren<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetParticle == null || targetVFX == null) return;

        bool isPlaying = targetParticle.isPlaying;

        if (isPlaying && !wasPlaying)
        {
            targetVFX.Play();
        }
        else if (!isPlaying && wasPlaying)
        {
            targetVFX.Stop();
        }

        wasPlaying = isPlaying;

    }
}
