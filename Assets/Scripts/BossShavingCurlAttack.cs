using System.Collections;
using UnityEngine;

public class BossShavingCurlAttack : BossAttack
{
    [SerializeField] private ShavingCurl shavingCurl;
    [SerializeField] private Transform upperStart;
    [SerializeField] private Transform upperTurn;
    [SerializeField] private Transform lowerTurn;
    [SerializeField] private Transform lowerEnd;
    [SerializeField] private float startupDelay = 3f;
    [SerializeField] private BossAnimationController bossAnimation;

    private ShavingCurl activeCurl;

    protected override IEnumerator ExecuteAttack()
    {
        bossAnimation.PlayBoomerang();

        yield return new WaitForSeconds(startupDelay);

        activeCurl = Instantiate(
            shavingCurl,
            upperStart.position,
            Quaternion.identity
        );

        activeCurl.Initialize(
            upperTurn,
            lowerTurn,
            lowerEnd
        );

        SoundController.Instance.Play(
            SoundController.SoundEffect.BoomerangSpit
        );

        yield return new WaitUntil(() => activeCurl == null);

        activeCurl = null;
    }

    public override void CancelAttack()
    {
        if (activeCurl != null)
        {
            Destroy(activeCurl.gameObject);
            activeCurl = null;
        }

        base.CancelAttack();
    }
}