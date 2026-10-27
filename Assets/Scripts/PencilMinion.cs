using System.Collections;
using UnityEngine;

public class PencilMinion : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float minimumShotDelay = 1f;
    [SerializeField] private float maximumShotDelay = 3f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private float deathSpriteDuration = 1f;

    private bool isAlive;
    private Transform player;
    private Coroutine firingRoutine;

    public void Initialize(Transform playerTarget)
    {
        player = playerTarget;
        isAlive = true;
        firingRoutine = StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        while (isAlive)
        {
            float delay = Random.Range(
                minimumShotDelay,
                maximumShotDelay
            );

            yield return new WaitForSeconds(delay);

            if (!isAlive || player == null)
            {
                yield break;
            }

            Vector3 direction =
                player.position - firePoint.position;

            direction.z = 0f;
            direction.Normalize();

            Projectile shot = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

            shot.SetDirection(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        isAlive = false;

        if (firingRoutine != null)
        {
            StopCoroutine(firingRoutine);
            firingRoutine = null;
        }

        Collider[] colliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        audioSource.PlayOneShot(deathSound, 0.9f);
        spriteRenderer.sprite = deadSprite;

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathSpriteDuration);

        Destroy(gameObject);
    }
}