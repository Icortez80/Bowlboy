using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    //boss has a total of 1000 HP
    private enum BossPhase
    {
        first, // hp > .8
        second, // hp > .5
        third, //hp > 0
        defeat,
        idle, //use this for maybe a start and end screen
        debug //activate only certain attacks or whatever
    }

    [SerializeField] private BossDialSpinAttack spinAttack;
    [SerializeField] private BossGraphiteBlastAttack blastAttack;
    [SerializeField] private BossPencilNibAttack nibAttack;
    [SerializeField] private BossShavingCurlAttack curlAttack;
    [SerializeField] private PlatformSpikeController platformSpikes;
    [SerializeField] private IdleSpikes spikeZone;


    private BossHealth bossHealth;
    private BossPhase currentPhase;

    private void Awake()
    {
        bossHealth = GetComponent<BossHealth>();
        //platformSpikes = GetComponentInChildren<PlatformSpikeController>();
        DisableAllAttacks();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentPhase = BossPhase.debug; //swap with line under for actual gameplay
        currentPhase = BossPhase.first;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPhase == BossPhase.debug)
        {
            //platformSpikes.enabled = true;
        }

        if (currentPhase == BossPhase.first)
        {
            curlAttack.enabled = true;
            spinAttack.enabled = true;
            nibAttack.enabled = true;

            //entering second phase
            if (bossHealth.HealthFraction <= .8)
            {
                DisableAllAttacks();
                currentPhase = BossPhase.second;
                print("Boss phase: " + currentPhase);
                curlAttack.enabled = true;
                spinAttack.enabled = true;
                blastAttack.enabled = true;
                nibAttack.enabled = true;
            }
        }
        else if(currentPhase == BossPhase.second)
        {
            //entering final phase
            if (bossHealth.HealthFraction <= .5)
            {
                DisableAllAttacks();
                currentPhase = BossPhase.third;
                print("Boss phase: " + currentPhase);
                spinAttack.enabled = true;
                spikeZone.enabled = true;
                platformSpikes.enabled = true;
            }
        }
        else if(currentPhase == BossPhase.third)
        {
            //boss defeated
            if(bossHealth.HealthFraction <= 0)
            {
                DisableAllAttacks();
            }
        }
    }



    private void DisableAllAttacks()
    {
        spinAttack.enabled = false;
        blastAttack.enabled = false;
        nibAttack.enabled = false;
        curlAttack.enabled = false;

        platformSpikes.StopAttack();
        platformSpikes.enabled = false;

        if (spikeZone != null)
        {
            spikeZone.Hide();
            spikeZone.enabled = false;
        }
    }
}
