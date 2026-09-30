using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{
    [SerializeField] public Projectile ExProjectile;
    [SerializeField] private float superDuration = 2.2f;
    [SerializeField] public PlayerBeam beam;

    private float superTimeRemaining;
    private bool isSuperActive;
    private float superDirection;
    private PlayerBeam activeBeam;
    public bool IsSuperActive => isSuperActive;
    private PlayerShooting playerShoot;
    private PlayerInputReader inputReader;
    private PlayerEnergy energy;
    private PlayerMovement movement;
    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        energy = GetComponent<PlayerEnergy>();
        playerShoot = GetComponent<PlayerShooting>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSuperActive)
        {
            superTimeRemaining -= Time.deltaTime;
            if(superTimeRemaining <= 0)
            {
                if(activeBeam != null)
                {
                    Destroy(activeBeam.gameObject);
                    activeBeam = null;
                }
                isSuperActive = false;
                movement.EndSuperLock();
            }

            return;
        }

        if (inputReader.SpecialPressed)
        {
            //player can not use EX/super while dashing
            if (movement.IsDashing) return;

            PlayerEnergy.SpecialType result = energy.TrySpendSpecial();
            playerShoot.GetShotInfo(out Vector3 direction, out Vector3 spawnPosition);
            //print(result);
            if (result == PlayerEnergy.SpecialType.EX)
            {
                Projectile exShot = Instantiate(ExProjectile, spawnPosition, Quaternion.identity);
                exShot.SetDirection(direction);
            }else if (result == PlayerEnergy.SpecialType.Super)
            {
                //print("Super!");
                isSuperActive = true;
                superTimeRemaining = superDuration;
                superDirection = movement.FacingDirection;
                Quaternion beamRotation = superDirection < 0f
                    ? Quaternion.Euler(0f, 180f, 0f)
                    : Quaternion.identity; 
                activeBeam = Instantiate(beam, playerShoot.SideFirePosition, beamRotation);
                movement.BeginSuperLock();
            }
        }
    }
}
