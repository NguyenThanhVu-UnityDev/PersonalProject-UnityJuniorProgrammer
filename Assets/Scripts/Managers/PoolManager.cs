using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public enum PoolType { GameObject, Particle }

    [SerializeField] private int _defaultCapacity = 20;
    [SerializeField] private int _maxSize = 50;

    private static PoolManager _instance = null;
    private Dictionary<GameObject, ObjectPool<GameObject>> _pools = new();
    private Dictionary<GameObject, GameObject> _poolMap = new();

    // Holders
    private GameObject _generalHolder;
    private GameObject _gameObjectsHolder;
    private GameObject _particlesHolder;

    public static PoolManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        _instance = this;

        SetupHolders();
    }

    private void SetupHolders()
    {
        _generalHolder = new GameObject("GeneralHolder");

        _gameObjectsHolder = new GameObject("GameObjectsHolder");
        _gameObjectsHolder.transform.SetParent(_generalHolder.transform);

        _particlesHolder = new GameObject("ParticlesHolder");
        _particlesHolder.transform.SetParent(_generalHolder.transform);
    }

    private void CreateNewPool(GameObject prefab, PoolType poolType = PoolType.GameObject)
    {
        if (prefab == null || _pools.ContainsKey(prefab)) return;

        var newPool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
            );

        _pools.Add(prefab, newPool);
    }
    private GameObject CreateObject(GameObject prefab, PoolType poolType = PoolType.GameObject)
    {
        bool isPrefabActive = prefab.activeSelf;

        prefab.SetActive(false);

        var newObject = Instantiate(prefab);

        if (isPrefabActive) prefab.SetActive(true);

        GameObject parentHolder = GetPoolHolder(poolType);
        if (parentHolder != null)
        {
            newObject.transform.SetParent(parentHolder.transform);
        }

        return newObject;
    }

    private void OnGetObject(GameObject obj) { }

    private void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);

        if (_poolMap.ContainsKey(obj))
        {
            _poolMap.Remove(obj);
        }
    }

    private void OnDestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    private GameObject GetPoolHolder(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObject:
                return _gameObjectsHolder;
            case PoolType.Particle:
                return _particlesHolder;
            default:
                return null;
        }
    }

    public T SpawnObject<T> (GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObject) where T: Object
    {
        if (!_pools.ContainsKey(prefab))
        {
            CreateNewPool(prefab, poolType);
        }

        var newObj = _pools[prefab].Get();
        if (newObj == null) return null;

        newObj.transform.SetPositionAndRotation(pos, rot);
        newObj.SetActive(true);

        if (!_poolMap.ContainsKey(newObj))
        {
            _poolMap.Add(newObj, prefab);
        }
        else
        {
            _poolMap[newObj] = prefab;
        }

        if (typeof(T) == typeof(GameObject))
        {
            return newObj as T;
        }
        else if (newObj.TryGetComponent(out T component))
        {
            return component;
        }
        return null;
    }

    public T SpawnObject<T> (T prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObject) where T: Component
    {
        return SpawnObject<T>(prefab.gameObject, pos, rot, poolType);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObject)
    {
        return SpawnObject<GameObject>(prefab, pos, rot, poolType);
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        if (_poolMap.TryGetValue(objectToReturn, out var prefab))
        {
            if (_pools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(objectToReturn);
            }
        }
        else
        {
            Debug.LogError("Cannot return: " + objectToReturn.name);
        }
    }
}
