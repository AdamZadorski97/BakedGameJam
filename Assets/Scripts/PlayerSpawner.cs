using System.Collections.Generic;
using UnityEngine;
using InControl;
using ProjectDawn.SplitScreen;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SplitScreenEffect splitScreenEffect;
    public GameObject playerPrefab; // Assign in the inspector
    public GameObject cameraPrefab;
    private List<GameObject> spawnedPlayers = new List<GameObject>();
    private int playersCount;

    void Start()
    {
        // Initial spawn based on connected devices at start
        SpawnPlayers();

        // Subscribe to device attached event
        InputManager.OnDeviceDetached += OnDeviceDetached;
        InputManager.OnDeviceAttached += OnDeviceAttached;
    }

    void SpawnPlayers()
    {
        foreach (var device in InputManager.Devices)
        {
            if (spawnedPlayers.Count < InputManager.Devices.Count)
            {
                SpawnPlayer(device);
            }
        }
    }

    void SpawnPlayer(InputDevice device)
    {
        playersCount++;

        GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        PlayerController playerController = newPlayer.GetComponent<PlayerController>();
        GameObject newCamera = Instantiate(cameraPrefab, transform.position, Quaternion.identity);

        splitScreenEffect.AddScreen(newCamera.GetComponent<Camera>(), playerController.transform);

        playerController.playerID = playersCount;
        playerController.InputDevice = device; // Assign the device

        spawnedPlayers.Add(newPlayer);
    }

    void OnDeviceAttached(InputDevice device)
    {
        // Spawn a new player for the newly attached device
        SpawnPlayer(device);
    }

    void OnDeviceDetached(InputDevice device)
    {
        // Find the player with the matching input device
        var playerToRemove = spawnedPlayers.Find(player => player.GetComponent<PlayerController>().InputDevice == device);
        if (playerToRemove != null)
        {
            // Optionally, handle other cleanup before destroying the player object
            splitScreenEffect.RemoveScreen(playerToRemove.transform); // Assuming the camera is a child of the player

            spawnedPlayers.Remove(playerToRemove);
            Destroy(playerToRemove);
            playersCount--; // Update players count
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        InputManager.OnDeviceAttached -= OnDeviceAttached;
    }
}
