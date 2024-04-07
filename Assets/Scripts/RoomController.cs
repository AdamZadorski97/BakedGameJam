using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomController : MonoBehaviour
{
    public List<PlayerController> playerControllers = new List<PlayerController>();
    public List<Renderer> wallsToFade = new List<Renderer>();
    public Material transparentMaterial; // Assign this material in the Inspector
    private float fadeDuration = 0.5f;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    private void Start()
    {
        // Remember the original material of each wall
        foreach (Renderer wallRenderer in wallsToFade)
        {
            originalMaterials[wallRenderer] = wallRenderer.material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController enteringPlayer = other.GetComponent<PlayerController>();
        if (enteringPlayer != null && !playerControllers.Contains(enteringPlayer))
        {
            playerControllers.Add(enteringPlayer);
            if (playerControllers.Count == 1)
            {
                FadeWalls();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController exitingPlayer = other.GetComponent<PlayerController>();
        if (exitingPlayer != null)
        {
            playerControllers.Remove(exitingPlayer);
            if (playerControllers.Count == 0)
            {
                RestoreWallsMaterial();
            }
        }
    }

    private void FadeWalls()
    {
        foreach (Renderer wallRenderer in wallsToFade)
        {
            // Immediately switch to the transparent material
            wallRenderer.material = transparentMaterial;
        }
    }

    private void RestoreWallsMaterial()
    {
        foreach (var wallEntry in originalMaterials)
        {
            // Restore the original material
            wallEntry.Key.material = wallEntry.Value;
        }
    }
}