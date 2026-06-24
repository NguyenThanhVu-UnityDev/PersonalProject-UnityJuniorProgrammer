using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public enum TrainKind { None, Danger, Safe }

    [Header("Track layout")]
    [Min(1)]
    [SerializeField] int tracksCount = 3;
    [SerializeField] float trackWidth = 10f;
    [SerializeField] float trackSpacing = 5f;
    [SerializeField] Vector3 centerPosition = Vector3.zero;

    [Header("Spawn settings")]
    [SerializeField] float spawnY = 0f;
    [SerializeField] float spawnZ = 50f;
    [Tooltip("How often the spawner attempts to spawn a train (seconds)")]
    [SerializeField, Min(0f)] float spawnInterval = 1.5f;

    [Header("Train properties (shared across prefabs)")]
    [SerializeField, Min(0.01f)] float trainLength = 10f;
    [Tooltip("Extra gap required between trains to avoid overlap")]
    [SerializeField, Min(0f)] float minSpacing = 0.5f;

    [Header("Prefabs")]
    [SerializeField] List<MovingObject> dangerTrains = new();
    [SerializeField] List<MovingObject> safeTrains = new();

    private Dictionary<MovingObject, List<MovingObject>> spawnedTrains = new();

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

    private TrackInfo[] trackInfos;
    private Coroutine spawnRoutine;

    private void Awake()
    {
        trackInfos = new TrackInfo[tracksCount];
    }

    void OnEnable()
    {
        // start spawning
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        // Stop spawning when the game end
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    void OnValidate()
    {
        if (trackWidth < 0f) trackWidth = 0f;
        if (trackSpacing < 0f) trackSpacing = 0f;
    }


    // left-anchor approach — same math PlayerController uses
    Vector3 GetLanePosition(int index)
    {
        index = Mathf.Clamp(index, 0, tracksCount - 1);

        // total width occupied by lane centers
        float totalWidth = tracksCount * trackWidth + (tracksCount - 1) * trackSpacing;

        // x position of the first (left-most) lane center
        float firstLaneX = centerPosition.x - totalWidth * 0.5f + trackWidth * 0.5f;

        // distance between adjacent lane centers
        float stride = trackWidth + trackSpacing;

        float x = firstLaneX + index * stride;
        return new Vector3(x, spawnY, spawnZ);
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            TrySpawnOnce();
            yield return new WaitForSeconds(Mathf.Max(0.001f, spawnInterval));
        }
    }

    void TrySpawnOnce()
    {
        float now = Time.time;
        float requiredDistance = trainLength + minSpacing;

        // count how many tracks currently have an active danger train
        int dangerActiveCount = 0;
        for (int i = 0; i < trackInfos.Length; i++)
        {
            if (trackInfos[i].lastKind == TrainKind.Danger && trackInfos[i].HasActive(now, requiredDistance))
                dangerActiveCount++;
        }

        // Make sure to spawn at least one safe train
        bool mustSpawnSafe = dangerActiveCount >= Mathf.Max(0, tracksCount - 1);

        // Tracks that doesnt not have an active train
        List<int> freeTracks = new List<int>(tracksCount);
        for (int i = 0; i < trackInfos.Length; i++)
        {
            if (!trackInfos[i].HasActive(now, requiredDistance)) freeTracks.Add(i);
        }

        if (freeTracks.Count == 0) return;

        // pick a random free track
        int chosenIndex = freeTracks[Random.Range(0, freeTracks.Count)];

        // decide kind
        TrainKind chosenKind;
        MovingObject prefab = null;

        if (mustSpawnSafe)
        {
            chosenKind = TrainKind.Safe;
            if (safeTrains != null && safeTrains.Count > 0) prefab = safeTrains[Random.Range(0, safeTrains.Count)];
            else return; // no prefabs at all
        }
        else
        {
            // randomly choose between danger and safe (50/50)
            bool pickDanger = Random.value > 0.5f;
            if (pickDanger && dangerTrains.Count > 0)
            {
                chosenKind = TrainKind.Danger;
                prefab = dangerTrains[Random.Range(0, dangerTrains.Count)];
            }
            else if (!pickDanger && safeTrains.Count > 0)
            {
                chosenKind = TrainKind.Safe;
                prefab = safeTrains[Random.Range(0, safeTrains.Count)];
            }
            else
            {
                // if chosen list is empty, fallback to whichever is available
                if (dangerTrains.Count > 0)
                {
                    chosenKind = TrainKind.Danger;
                    prefab = dangerTrains[Random.Range(0, dangerTrains.Count)];
                }
                else if (safeTrains.Count > 0)
                {
                    chosenKind = TrainKind.Safe;
                    prefab = safeTrains[Random.Range(0, safeTrains.Count)];
                }
                else return; // nothing to spawn
            }
        }

        // instantiate train
        Vector3 spawnPos = GetLanePosition(chosenIndex);
        MovingObject newTrain = null;

        if (spawnedTrains.ContainsKey(prefab))
        {
            foreach (var spawnedTrain in spawnedTrains[prefab])
            {
                if (spawnedTrain == null) return;
                if (!spawnedTrain.gameObject.activeInHierarchy)
                {
                    newTrain = spawnedTrain;
                    break;
                }
            }
        }

        if (newTrain == null)
        {
            newTrain = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            if (!spawnedTrains.ContainsKey(prefab))
            {
                spawnedTrains.Add(prefab, new());
            }
            spawnedTrains[prefab].Add(newTrain);
        }
        
        if (newTrain != null)
        {
            newTrain.gameObject.SetActive(true);
            newTrain.transform.position = spawnPos;
        }

        // record spawn info for that track
        trackInfos[chosenIndex].lastSpawnTime = now;
        trackInfos[chosenIndex].lastKind = chosenKind;
        trackInfos[chosenIndex].lastSpawnTrain = newTrain;
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null) spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }
}