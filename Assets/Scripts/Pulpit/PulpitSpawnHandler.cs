using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitSpawnHandler : MonoBehaviour
{
    private float minPulpitDestroyTime, maxPulpitDestroyTime; 
    private float pulpitSpawnTime;                           

    private float destroyTime;          // the time pulpit will stay active for

    private int countPulpitsInScene;    // for number of active pulpits
    private int pulpitSpawnDirection;   // Determines random absolute direction (forward, backward, left, right) to spawn new pulpit

    private bool firstPulpitSpawn;      // Enables initial spawn of first pulpit of the game at specified initial position
    private bool shouldSpawn;           // Determines if spawning is enabled

    [SerializeField] private GameObject pulpitPrefab;   // holds pulpit prefab object
    [SerializeField] private GameObject startBox;       // holds starting platform object

    [SerializeField] private Vector3 pulpitCurSize = new(7f, 1f, 7f);               // current pulpit
    [SerializeField] private Vector3 pulpitNextSize = new(7f, 1f, 7f);              // dimensions of next pulpit
    [SerializeField] private Vector3 pulpitInitialPos = new Vector3(0f, 1f, 0f);    // make position of first pulpit of the game

    [SerializeField] private float minPulpitSizeRange = 8f;     // least size for random-sized pulpit generation
    [SerializeField] private float maxPulpitSizeRange = 10f;    // max size for random-sized pulpit generation

    [SerializeField] private float animationTime = 0.2f;

    [SerializeField] private int maxPulpitsInScene = 2; // maximum pulpits in the game


    private Vector3 pulpitCurPos;   // track of position of current pulpit
    private Vector3 pulpitNextPos;  // calculates value for position of next pulpit to spawn


    private void Awake()
    {
        //method to data fetch success event
        GameDataHandler.OnDataFetchSuccess += InitializePulpitData;
        Death.OnPlayerDeath += DisableSpawning;
    }

    void Start()
    {
        // start count and first spawn
        countPulpitsInScene = 0;
        shouldSpawn = true;
        firstPulpitSpawn = true;

        // halts currently running coroutines just to be safe
        StopAllCoroutines();

        // late spawn startup, lets the object catch up with json fetching
        Invoke("DeferredSpawnStartup", 1f);
    }

    void DeferredSpawnStartup()
    {
        // remove the first platform and let player onto the first pulpit
        Destroy(startBox);
        StartCoroutine(SpawnerCoroutine());
    }

    public IEnumerator SpawnerCoroutine()
    {
        while (shouldSpawn)
        {
            if (countPulpitsInScene < maxPulpitsInScene)
                SpawnPulpit();
            yield return new WaitForSeconds(pulpitSpawnTime);
        }
    }

    IEnumerator DestroyerCoroutine(GameObject pulpit, float destroyTime)
    {

        // Wait for destroyTime before destroying pulpits
        yield return new WaitForSeconds(destroyTime);

        // kill pulpit once animation is complete
        Destroy(pulpit);

        // Update active pulpits count
        countPulpitsInScene--;
    }

    private void SpawnPulpit()
    {
        GameObject pulpitInstance;
        
    
        if (firstPulpitSpawn)
        {
            // make a new first pulpit at defined origin
            pulpitInstance = 
                Instantiate(pulpitPrefab, pulpitInitialPos, pulpitPrefab.transform.rotation);

            firstPulpitSpawn = false;

            // shows current position with that of latest pulpit
            pulpitCurPos = pulpitInitialPos; 
        }

        else
        {
            pulpitSpawnDirection = Random.Range(0, 4);  // choose between 1 of 4 possible adjacent sides at random

            float pulpitNextPosZ = (pulpitCurSize.z / 2) + (pulpitNextSize.z / 2);  
            float pulpitNextPosX = (pulpitCurSize.x / 2) + (pulpitNextSize.x / 2);

            pulpitNextPos = pulpitSpawnDirection switch // chooses which adjacent side to spawn the next platform at
            {
                0 => pulpitCurPos + new Vector3(0f, 0f, pulpitNextPosZ), 
                1 => pulpitCurPos - new Vector3(0f, 0f, pulpitNextPosZ), 
                2 => pulpitCurPos - new Vector3(pulpitNextPosX, 0f, 0f), 
                3 => pulpitCurPos + new Vector3(pulpitNextPosX, 0f, 0f), 
                _ => pulpitCurPos + new Vector3(0f, 0f, pulpitNextPosZ)  
            };

            // make new pulpit at random adjacent position
            pulpitInstance =
                Instantiate(pulpitPrefab, pulpitNextPos, pulpitPrefab.transform.rotation);

            // upate current pulpit size with that of the latest one
            pulpitCurSize = pulpitNextSize;

            // update current position with that of latest pulpit
            pulpitCurPos = pulpitNextPos;
        }

        

        // new pulpit size (if random size every new pulpit)
        pulpitInstance.transform.localScale = pulpitNextSize;
        pulpitCurSize = pulpitNextSize;

        // update number of active pulpits
        countPulpitsInScene++;

        // start destroyTime and start Destroy coroutine
        destroyTime = Random.Range(minPulpitDestroyTime, maxPulpitDestroyTime);
        // instance's destroyTime to generated destroyTime for display purpose
        pulpitInstance.GetComponent<PulpitBehavior>().destroyTime = destroyTime;

        StartCoroutine(DestroyerCoroutine(pulpitInstance, destroyTime));
    }

    void DisableSpawning()
    {
    

        shouldSpawn = false;
    }

    private void OnDestroy()
    {
        // renove method from action event
        GameDataHandler.OnDataFetchSuccess -= InitializePulpitData;
        Death.OnPlayerDeath -= DisableSpawning;
    }

    private void InitializePulpitData()
    {
        // start all fetched json data for the spawn conditions
        minPulpitDestroyTime = GameDataHandler.MinPulpitDestroyTime;
        maxPulpitDestroyTime = GameDataHandler.MaxPulpitDestroyTime;
        pulpitSpawnTime = GameDataHandler.PulpitSpawnTime;
    }
}
