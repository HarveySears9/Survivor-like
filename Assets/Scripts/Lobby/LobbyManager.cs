using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject player;

    public Transform[] spawnLocations;
    // Start is called before the first frame update
    void Start()
    {
        SetPlayerLocation();
    }

    void SetPlayerLocation()
    {
        // Determine the spawn location based on the last scene
        switch (SceneTracker.LastSceneName)
        {
            case "Library":
                player.transform.position = spawnLocations[0].position;
                break;

            case "MainMenu":
                player.transform.position = spawnLocations[1].position;
                break;

            case "Tavern":
                player.transform.position = spawnLocations[2].position;
                break;

            default:
                // Fallback location
                break;
        }
    }
}
