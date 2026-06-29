using System;
using UnityEngine;

public interface IPoolObject
{
    public Action<GameObject> OnReturnToPool { get; set; }
}
