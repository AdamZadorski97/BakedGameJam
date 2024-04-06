using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomController : MonoBehaviour
{
    public List<PlayerController> playerControllers = new List<PlayerController>();
    public List<Renderer> wallsToFade = new List<Renderer>();
    private float fadeDuration = 0.5f;
    private float targetOpacity = 0.1f;
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    private void Start()
    {
        // Remember the original color (including opacity) of each wall
        foreach (Renderer wallRenderer in wallsToFade)
        {
            if (wallRenderer.material.HasProperty("_BaseColor"))
            {
                originalColors[wallRenderer] = wallRenderer.material.GetColor("_BaseColor");
            }
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
                FadeWalls(targetOpacity);
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
                RestoreWallsColor();
            }
        }
    }

    private void FadeWalls(float targetOpacity)
    {
        foreach (Renderer wallRenderer in wallsToFade)
        {
            if (wallRenderer.material.HasProperty("_BaseColor"))
            {
                Color currentColor = wallRenderer.material.GetColor("_BaseColor");
                Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);
                // Animate the color change
                DOTween.To(() => wallRenderer.material.GetColor("_BaseColor"), x => wallRenderer.material.SetColor("_BaseColor", x), targetColor, fadeDuration);
            }
        }
    }

    private void RestoreWallsColor()
    {
        foreach (var wallEntry in originalColors)
        {
            if (wallEntry.Key.material.HasProperty("_BaseColor"))
            {
                // Animate the color change back to the original color
                DOTween.To(() => wallEntry.Key.material.GetColor("_BaseColor"), x => wallEntry.Key.material.SetColor("_BaseColor", x), wallEntry.Value, fadeDuration);
            }
        }
    }
}
