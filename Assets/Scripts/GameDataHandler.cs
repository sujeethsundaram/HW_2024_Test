using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // for web requests
using System;

public class GameDataHandler : MonoBehaviour
{
    // to show data fetch success
    public static Action OnDataFetchSuccess;

    // Data
    private string dataObject;

    [SerializeField]
    private string serverURL = 
        "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    // variables for player and pulpit data
    public static float PlayerSpeed;
    public static float MinPulpitDestroyTime, MaxPulpitDestroyTime, PulpitSpawnTime;

    void Start()
    {
        StartCoroutine(GetGameDataFromServer()); 
    }
    IEnumerator GetGameDataFromServer()
    {
        using UnityWebRequest server = UnityWebRequest.Get(serverURL); 
        yield return server.SendWebRequest();

        if (server.result == UnityWebRequest.Result.ConnectionError || server.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(server.error); 

            PlayerSpeed = 0f;
            MinPulpitDestroyTime = 0f;
            MaxPulpitDestroyTime = 0f;
            PulpitSpawnTime = 0f;
        }

        else
        {
            dataObject = server.downloadHandler.text; 
            GameData gameData = JsonUtility.FromJson<GameData>(dataObject);

            PlayerSpeed = gameData.player_data.speed;
            MinPulpitDestroyTime = gameData.pulpit_data.min_pulpit_destroy_time;
            MaxPulpitDestroyTime = gameData.pulpit_data.max_pulpit_destroy_time;
            PulpitSpawnTime = gameData.pulpit_data.pulpit_spawn_time;



            OnDataFetchSuccess?.Invoke();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

[Serializable]
public class GameData
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

[Serializable]
public class PlayerData
{
    public float speed;
}

[Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}
