using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyPotController : MonoBehaviour
{
    public List<PlayerController> playerControllers = new List<PlayerController>();
    public Transform projectileSpawnPosition;
    public GameObject projectilePrefab; // Assign this in the Inspector with your projectile prefab
    public GameObject hitParticlePrefab;
    public float projectileSpeed = 5f; // Speed at which the projectile will move
    public float jumpPower = 2f; // Power of the jump
    public int numJumps = 1; // Number of jumps before reaching the target
    public float attackDelay = 3f; // Delay in seconds between each attack
    public AnimationCurve attackCurve;
    private bool canAttack = true;
    public Vector3 endParticleOffset;

    private void Update()
    {
        if (playerControllers.Count > 0 && canAttack)
        {
            StartCoroutine(ShootAtPlayer());
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        canAttack = false;
        foreach (PlayerController player in playerControllers)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
            Vector3 targetPosition = player.transform.position + endParticleOffset;

            // Calculate duration based on distance and speed to ensure the particle spawns at the right moment
            float duration = (targetPosition - projectileSpawnPosition.position).magnitude / projectileSpeed;

            projectile.transform.DOJump(targetPosition, jumpPower, numJumps, duration)
                .SetEase(attackCurve)
                .OnComplete(() =>
                {
                    // Spawn the hit particle at the projectile's current position
                    GameObject hitParticle = Instantiate(hitParticlePrefab, projectile.transform.position, Quaternion.identity);

                    // Destroy the projectile immediately
                    Destroy(projectile);

                    // Optionally destroy the hit particle after 1 second if it doesn't auto-destroy
                    Destroy(hitParticle, 2f);
                });
        }

        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController enteringPlayer = other.GetComponent<PlayerController>();
        if (enteringPlayer != null && !playerControllers.Contains(enteringPlayer))
        {
            playerControllers.Add(enteringPlayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController exitingPlayer = other.GetComponent<PlayerController>();
        if (exitingPlayer != null)
        {
            playerControllers.Remove(exitingPlayer);
        }
    }
}
