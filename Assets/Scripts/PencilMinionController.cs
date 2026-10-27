using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMinionController : MonoBehaviour
{
    [SerializeField] private PencilMinion minionPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;
    [SerializeField] private float respawnDelay = 6f;

    private readonly List<PencilMinion> activeMinions = new();

    private Coroutine spawnRoutine;
    private bool isRunning;

    public void BeginAttack()
    {
        if (isRunning) return;

        isRunning = true;
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (isRunning)
        {
            SpawnPair();

            while (isRunning)
            {
                activeMinions.RemoveAll(minion => minion == null);

                if (activeMinions.Count == 0)
                {
                    break;
                }

                yield return null;
            }

            if (!isRunning) yield break;

            yield return new WaitForSeconds(respawnDelay);
        }
    }

    private void SpawnPair()
    {
        if (spawnPoints.Length < 2) return;

        int firstIndex = Random.Range(0, spawnPoints.Length);
        int secondIndex = firstIndex;

        while (secondIndex == firstIndex)
        {
            secondIndex = Random.Range(0, spawnPoints.Length);
        }

        PencilMinion firstMinion = Instantiate(
            minionPrefab,
            spawnPoints[firstIndex].position,
            Quaternion.identity
        );

        PencilMinion secondMinion = Instantiate(
            minionPrefab,
            spawnPoints[secondIndex].position,
            Quaternion.identity
        );

        firstMinion.Initialize(player);
        secondMinion.Initialize(player);

        activeMinions.Add(firstMinion);
        activeMinions.Add(secondMinion);
    }

    public void StopAttack()
    {
        isRunning = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        foreach (PencilMinion minion in activeMinions)
        {
            if (minion != null)
            {
                Destroy(minion.gameObject);
            }
        }

        activeMinions.Clear();
    }
}