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
    [SerializeField] private PencilMinionController pencilMinions;

    private Coroutine idleSpikeRoutine;

    [SerializeField] private float delayBetweenAttacks = 1f;

    private BossPhase currentPhase;
    private Queue<BossAttack> attackQueue = new Queue<BossAttack>();
    private BossAttack activeAttack;
    // Handle to the currently running attack coroutine.
    // null means "no attack is running, safe to start the next one."
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
        // health and phase checks
        if (bossHealth.HealthFraction <= 0f)
        {
            EnterDefeat();
            return;
        }

        CheckForPhaseChange();

        // If nothing is running, start the next attack.
        // Update runs every frame, so this null check keeps us from
        // starting a new coroutine on top of one that's still going.
        if (attackRoutine == null)
        {
            if (attackQueue.Count == 0)
            {
                FillAttackQueue();
            }

            // Start the coroutine and keep the handle so StopCoroutine can find it later.
            attackRoutine = StartCoroutine(RunNextAttack());
        }
    }

    // IEnumerator return type + yield return = this method can pause and resume.
    private IEnumerator RunNextAttack()
    {
        activeAttack = attackQueue.Dequeue();

        // Start the attack's own coroutine and wait here until it finishes.
        // The boss is "stuck" on this line while the attack plays out,
        // but the rest of the game keeps running normally.
        yield return StartCoroutine(activeAttack.Run());

        activeAttack = null;

        // Cooldown between attacks: pause here for delayBetweenAttacks seconds.
        yield return new WaitForSeconds(delayBetweenAttacks);

        // Reaching the end means the whole sequence finished normally.
        // Setting this to null is what lets Update start the next attack.
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

        if (newPhase == BossPhase.Second)
        {
            pencilMinions.BeginAttack();
        }

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
            // Same pattern: save the handle so the hazard can be stopped on phase change or defeat.
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

        if (pencilMinions != null)
        {
            pencilMinions.StopAttack();
        }

        currentPhase = BossPhase.Defeat;
        bossAnimation.PlayDefeat();

        enabled = false;
    }

    private void StopCurrentAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            // A stopped coroutine never reaches its last line, so
            // "attackRoutine = null" inside RunNextAttack won't run.
            // We have to clear it manually here.
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