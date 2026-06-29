using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticle : MonoBehaviour, IPoolObject
{
    public Action<GameObject> OnReturnToPool { get; set; }

    private void Awake()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        // Make sure to use callback when the particle system stop to return it back to its pool
        var particleMain = particleSystem.main;
        particleMain.stopAction = ParticleSystemStopAction.Callback;
    }
    private void OnParticleSystemStopped()
    {
        OnReturnToPool?.Invoke(gameObject);
    }

}
