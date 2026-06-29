using System;
using UnityEngine;

public class Train : MonoBehaviour, IPoolObject
{
    public Action<GameObject> OnReturnToPool { get; set; }

    private void OnDisable()
    {
        OnReturnToPool?.Invoke(gameObject);
    }

}
