using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class LevelSpawner : MonoBehaviour
{
    public enum TrainKind { None, Danger, Safe }

    [Header("Track layout")]
    [Min(1)]
    [SerializeField] int _tracksCount = 3;
    [SerializeField] float _trackWidth = 10f;
    [SerializeField] float _trackSpacing = 5f;
    [SerializeField] Vector3 _centerPosition = Vector3.zero;

    [Header("Trains Spawn settings")]
    [SerializeField] float _trainSpawnY = 0f;
    [SerializeField] float _trainSpawnZ = 50f;
    [Tooltip("How often the spawner attempts to spawn a train (seconds)")]
    [SerializeField, Min(0f)] float _trainSpawnInterval = 1.5f;

    [Header("Train properties (shared across prefabs)")]
    [SerializeField, Min(0.01f)] float _trainLength = 10f;
    [Tooltip("Extra gap required between trains to avoid overlap")]
    [SerializeField, Min(0f)] float _minTrainSpacing = 0.5f;

    [Header("Train Prefabs")]
    [SerializeField] List<MovingObject> _dangerTrains = new();
    [SerializeField] List<MovingObject> _safeTrains = new();

    [Header("Collectible Spawn Settings")]
    [SerializeField] float _collectibleSpawnY = 1.5f;
    [SerializeField] float _collectibleSpawnZ = 50f;
    [Tooltip("How often the spawner attempts to spawn a collectible (seconds)")]
    [SerializeField, Min(0f)] float _collectiblesSpawnInterval = 2f;
    [Tooltip("Extra gap required between collectibe and trains to avoid overlap")]
    [SerializeField, Min(0f)] float _minCollectibleSpacing = 0.5f;

    [Header("Collectible Prefabs")]
    [SerializeField] List<CollectibleSpawnRate> _collectibles = new();


    private TrackInfo[] _trackInfos;
    private Coroutine _trainsSpawnCoroutine;
    private Coroutine _collectiblesSpawnCoroutine;

    private void Awake()
    {
        _trackInfos = new TrackInfo[_tracksCount];
    }

    void OnEnable()
    {
        // start spawning
        _trainsSpawnCoroutine = StartCoroutine(TrainsSpawnLoop());

        _collectiblesSpawnCoroutine = StartCoroutine(CollectiblesSpawnLoop());
    }

    void OnDisable()
    {
        // Stop spawning when the game end
        if (_trainsSpawnCoroutine != null) StopCoroutine(_trainsSpawnCoroutine);
        _trainsSpawnCoroutine = null;
    }

    void OnValidate()
    {
        if (_trackWidth < 0f) _trackWidth = 0f;
        if (_trackSpacing < 0f) _trackSpacing = 0f;
    }


    // left-anchor approach — same math PlayerController uses
    Vector3 GetLanePosition(int index)
    {
        index = Mathf.Clamp(index, 0, _tracksCount - 1);

        // total width occupied by lane centers
        float totalWidth = _tracksCount * _trackWidth + (_tracksCount - 1) * _trackSpacing;

        // x position of the first (left-most) lane center
        float firstLaneX = _centerPosition.x - totalWidth * 0.5f + _trackWidth * 0.5f;

        // distance between adjacent lane centers
        float stride = _trackWidth + _trackSpacing;

        float x = firstLaneX + index * stride;
        return new Vector3(x, _trainSpawnY, _trainSpawnZ);
    }

    IEnumerator TrainsSpawnLoop()
    {
        while (true)
        {
            TrySpawnTrainOnce();
            yield return new WaitForSeconds(Mathf.Max(0.001f, _trainSpawnInterval));
        }
    }

    private void TrySpawnTrainOnce()
    {
        float now = Time.time;
        float requiredDistance = _trainLength + _minTrainSpacing;

        // count how many tracks currently have an active danger train
        int dangerActiveCount = 0;
        for (int i = 0; i < _trackInfos.Length; i++)
        {
            if (_trackInfos[i].lastKind == TrainKind.Danger && _trackInfos[i].HasActive(now, requiredDistance))
                dangerActiveCount++;
        }

        // Make sure to spawn at least one safe train
        bool mustSpawnSafe = dangerActiveCount >= Mathf.Max(0, _tracksCount - 1);

        // Tracks that doesnt not have an active train
        List<int> freeTracks = new List<int>(_tracksCount);
        for (int i = 0; i < _trackInfos.Length; i++)
        {
            if (!_trackInfos[i].HasActive(now, requiredDistance)) freeTracks.Add(i);
        }

        if (freeTracks.Count == 0) return;

        // pick a random free track
        int chosenIndex = freeTracks[UnityEngine.Random.Range(0, freeTracks.Count)];

        // decide kind
        TrainKind chosenKind;
        MovingObject chosenPrefab = null;

        if (mustSpawnSafe)
        {
            chosenKind = TrainKind.Safe;
            if (_safeTrains != null && _safeTrains.Count > 0) chosenPrefab = _safeTrains[UnityEngine.Random.Range(0, _safeTrains.Count)];
            else return; // no prefabs at all
        }
        else
        {
            // randomly choose between danger and safe (50/50)
            bool pickDanger = UnityEngine.Random.value > 0.5f;
            if (pickDanger && _dangerTrains.Count > 0)
            {
                chosenKind = TrainKind.Danger;
                chosenPrefab = _dangerTrains[UnityEngine.Random.Range(0, _dangerTrains.Count)];
            }
            else if (!pickDanger && _safeTrains.Count > 0)
            {
                chosenKind = TrainKind.Safe;
                chosenPrefab = _safeTrains[UnityEngine.Random.Range(0, _safeTrains.Count)];
            }
            else
            {
                // if chosen list is empty, fallback to whichever is available
                if (_dangerTrains.Count > 0)
                {
                    chosenKind = TrainKind.Danger;
                    chosenPrefab = _dangerTrains[UnityEngine.Random.Range(0, _dangerTrains.Count)];
                }
                else if (_safeTrains.Count > 0)
                {
                    chosenKind = TrainKind.Safe;
                    chosenPrefab = _safeTrains[UnityEngine.Random.Range(0, _safeTrains.Count)];
                }
                else return; // nothing to spawn
            }
        }

        // instantiate train
        if (chosenPrefab == null || PoolManager.Instance == null) return;

        Vector3 spawnPos = GetLanePosition(chosenIndex);
        MovingObject newTrain = PoolManager.Instance.SpawnObject(chosenPrefab, spawnPos, Quaternion.identity);
        
        if (newTrain != null)
        {
            // record spawn info for that track
            _trackInfos[chosenIndex].lastSpawnTime = now;
            _trackInfos[chosenIndex].lastKind = chosenKind;
            _trackInfos[chosenIndex].lastSpawnTrain = newTrain;
        }
    }

    public void StartSpawningTrains()
    {
        if (_trainsSpawnCoroutine == null) _trainsSpawnCoroutine = StartCoroutine(TrainsSpawnLoop());
    }

    public void StopSpawningTrains()
    {
        if (_trainsSpawnCoroutine != null) StopCoroutine(_trainsSpawnCoroutine);
        _trainsSpawnCoroutine = null;
    }

    IEnumerator CollectiblesSpawnLoop()
    {
        while (true)
        {
            TrySpawnCollectibleOnce();
            yield return new WaitForSeconds(Mathf.Max(0.001f, _collectiblesSpawnInterval));
        }
    }

    private void TrySpawnCollectibleOnce()
    {
        if (_collectibles.Count == 0) return;

        // Tracks that doesnt not have an active train
        float now = Time.time;
        float requiredDistance = _trainLength + _minCollectibleSpacing;
        List<int> freeTracks = new List<int>(_tracksCount);
        for (int i = 0; i < _trackInfos.Length; i++)
        {
            if (!_trackInfos[i].HasActive(now, requiredDistance)) freeTracks.Add(i);
        }

        if (freeTracks.Count == 0) return;

        // pick a random free track
        int chosenIndex = freeTracks[UnityEngine.Random.Range(0, freeTracks.Count)];

        float randomValue = UnityEngine.Random.Range(0f, 1.0f);
        float accumulateSpawnRate = 0;
        Collectible chosenCollectiblePrefab = null;

        foreach (var collectible in _collectibles)
        {
            if (collectible.CollectiblePrefab == null) continue;
            accumulateSpawnRate += collectible.SpawnRate;
            if (randomValue <= accumulateSpawnRate)
            {
                chosenCollectiblePrefab = collectible.CollectiblePrefab;
                break;
            }
        }

        if (chosenCollectiblePrefab == null || PoolManager.Instance == null) return;

        Vector3 spawnPos = GetLanePosition(chosenIndex);

        var newCollectible = PoolManager.Instance.SpawnObject(chosenCollectiblePrefab, spawnPos, Quaternion.identity);
    }

    // per-track spawn info
    private struct TrackInfo
    {
        public float lastSpawnTime;
        public MovingObject lastSpawnTrain;
        public TrainKind lastKind;

        public bool HasActive(float now, float requiredDistance)
        {
            if (lastKind == TrainKind.None) return false;
            if (lastSpawnTrain == null || !lastSpawnTrain.gameObject.activeInHierarchy) return false;
            float moved = (now - lastSpawnTime) * lastSpawnTrain.RelativeSpeed;
            return moved < requiredDistance;
        }
    }

    [Serializable]
    private struct CollectibleSpawnRate
    {
        [SerializeField] Collectible _collectiblePrefab;
        [Range(0.0f, 1.0f)]
        [SerializeField] float _spawnRate;

        public Collectible CollectiblePrefab { get => _collectiblePrefab; }
        public float SpawnRate { get => _spawnRate; }
    }
}