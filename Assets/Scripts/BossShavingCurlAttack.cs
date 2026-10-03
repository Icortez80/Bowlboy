using UnityEngine;

public class BossShavingCurlAttack : MonoBehaviour
{
    [SerializeField] ShavingCurl shavingCurl;
    [SerializeField] Transform upperStart;
    [SerializeField] Transform upperTurn;
    [SerializeField] Transform lowerTurn;
    [SerializeField] Transform lowerEnd;
    [SerializeField] public float spawnDelay = 3f;

    private ShavingCurl activeCurl;
    private float spawnTimer = 0f;
    private void Start()
    {
        spawnTimer = spawnDelay;
    }
    // Update is called once per frame
    void Update()
    {
        //if there is a curl active simply return
        if (activeCurl) return;
        {
            //this timer starts when the active curl has DESPAWNED
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0)
            {
                activeCurl = Instantiate(shavingCurl, upperStart.position, Quaternion.identity);
                activeCurl.Initialize(upperTurn, lowerTurn, lowerEnd);
                spawnTimer = spawnDelay;
            }
        }
    }
}
