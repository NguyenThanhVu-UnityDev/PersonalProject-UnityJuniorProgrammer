using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    [SerializeField] private ParticleSystem _collectedParticlePrefab;
    public virtual void Collect(CollectiblesDetector target)
    {
        PlayParticles();
        ProcessCollect(target);
        gameObject.SetActive(false);
    }

    protected virtual void PlayParticles()
    {
        if (PoolManager.Instance == null) return;

        var collectedParticle = PoolManager.Instance.SpawnObject(_collectedParticlePrefab, transform.position, Quaternion.identity, PoolManager.PoolType.Particle);
    }
    protected virtual void ProcessCollect(CollectiblesDetector target) { }
}
