using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    private enum BossPhase
    {
        First,
        Second,
        Third,
        Defeat
    }

    [Header("References")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private BossAnimationController bossAnimation;

    [Header("Phase One")]
    [SerializeField] private BossAttack[] firstPhaseAttacks;

    [Header("Phase Two")]
    [SerializeField] private BossAttack[] secondPhaseAttacks;

    [Header("Phase Three")]
    [SerializeField] private BossAttack[] thirdPhaseAttacks;
    [SerializeField] private IdleSpikes idleSpike;
    [SerializeField] private PlatformSpikeController platformSpikes;

    private Coroutine idleSpikeRoutine;

    [SerializeField] private float delayBetweenAttacks = 1f;

    private BossPhase currentPhase;
    private Queue<BossAttack> attackQueue = new Queue<BossAttack>();
    private BossAttack activeAttack;
    private Coroutine attackRoutine;

    private void Awake()
    {
        if (bossHealth == null)
        {
            bossHealth = GetComponent<BossHealth>();
        }
    }

    private void Start()
    {
        EnterPhase(BossPhase.First);
    }

    private void Update()
    {
        if (bossHealth.HealthFraction <= 0f)
        {
            EnterDefeat();
            return;
        }

        CheckForPhaseChange();

        if (attackRoutine == null)
        {
            if (attackQueue.Count == 0)
            {
                FillAttackQueue();
            }

            attackRoutine = StartCoroutine(RunNextAttack());
        }
    }

    private IEnumerator RunNextAttack()
    {
        activeAttack = attackQueue.Dequeue();

        yield return StartCoroutine(activeAttack.Run());

        activeAttack = null;

        yield return new WaitForSeconds(delayBetweenAttacks);

        attackRoutine = null;
    }

    private void FillAttackQueue()
    {
        BossAttack[] availableAttacks = GetAttacksForCurrentPhase();

        foreach (BossAttack attack in availableAttacks)
        {
            attackQueue.Enqueue(attack);
        }
    }

    private BossAttack[] GetAttacksForCurrentPhase()
    {
        return currentPhase switch
        {
            BossPhase.First => firstPhaseAttacks,
            BossPhase.Second => secondPhaseAttacks,
            BossPhase.Third => thirdPhaseAttacks,
            _ => new BossAttack[0]
        };
    }

    private void EnterPhase(BossPhase newPhase)
    {
        StopCurrentAttack();
        StopPhaseHazards();

        currentPhase = newPhase;
        attackQueue.Clear();

        if (newPhase == BossPhase.Third)
        {
            StartPhaseThreeHazards();
        }

        Debug.Log("Boss phase: " + currentPhase);
    }

    private void StartPhaseThreeHazards()
    {
        if (idleSpike != null)
        {
            idleSpikeRoutine = StartCoroutine(
                idleSpike.RunPersistent()
            );
        }

        if (platformSpikes != null)
        {
            platformSpikes.BeginAttack();
        }
    }

    private void StopPhaseHazards()
    {
        if (idleSpikeRoutine != null)
        {
            StopCoroutine(idleSpikeRoutine);
            idleSpikeRoutine = null;
        }

        if (idleSpike != null)
        {
            idleSpike.StopAttack();
        }

        if (platformSpikes != null)
        {
            platformSpikes.StopAttack();
        }
        
    }

    private void CheckForPhaseChange()
    {
        if (currentPhase == BossPhase.First &&
            bossHealth.HealthFraction <= 0.8f)
        {
            EnterPhase(BossPhase.Second);
        }
        else if (currentPhase == BossPhase.Second &&
                 bossHealth.HealthFraction <= 0.5f)
        {
            EnterPhase(BossPhase.Third);
        }
    }

    private void EnterDefeat()
    {
        StopCurrentAttack();
        StopPhaseHazards();

        currentPhase = BossPhase.Defeat;
        bossAnimation.PlayDefeat();

        enabled = false;
    }

    private void StopCurrentAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        if (activeAttack != null)
        {
            activeAttack.CancelAttack();
            activeAttack = null;
        }

        bossAnimation.PlayIdle();
    }
}