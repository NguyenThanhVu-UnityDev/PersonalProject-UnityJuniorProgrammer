using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    [SerializeField] private ParticleSystem _collectedParticlePrefab;
    [SerializeField] private AudioClip _collectAudio;
    [SerializeField] private float _collectAudioVolume = 0.3f;
    public virtual void Collect(CollectiblesDetector target)
    {
        PlayCollectAudio();
        PlayParticles();
        ProcessCollect(target);
        gameObject.SetActive(false);
    }

    protected virtual void PlayCollectAudio()
    {
        UIEvents.PlaySFX(_collectAudio, _collectAudioVolume);
    }

    protected virtual void PlayParticles()
    {
        if (PoolManager.Instance == null || _collectedParticlePrefab == null) return;

        PoolManager.Instance.SpawnObject(_collectedParticlePrefab, transform.position, Quaternion.identity, PoolManager.PoolType.Particle);
    }

    protected virtual void ProcessCollect(CollectiblesDetector target) { }
}
