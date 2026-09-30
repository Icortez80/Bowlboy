using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] public Projectile projectile;
    [SerializeField] public GameObject firePoint;
    [SerializeField] public GameObject firePointUp;
    [SerializeField] public GameObject firePointDown;
    [SerializeField] public float shotInterval = .5f;

    public Vector3 SideFirePosition => firePoint.transform.position;
    private float timer = 0f;
    private PlayerMovement movement;
    private PlayerInputReader inputReader;
    private PlayerEnergy energyOwner;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        inputReader = GetComponent<PlayerInputReader>();
        energyOwner = GetComponent<PlayerEnergy>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(inputReader.ShootHeld && timer <= 0)
        {
            //Vector3 direction = new Vector3(0f, 0f, 0f);
            //if (Keyboard.current.dKey.isPressed)
            //{
            //    direction.x += 1f;
            //}
            //if (Keyboard.current.aKey.isPressed)
            //{
            //    direction.x += -1f;
            //}
            //if (Keyboard.current.sKey.isPressed)
            //{
            //    direction.y += -1f;
            //}
            //if (Keyboard.current.wKey.isPressed)
            //{
            //    direction.y += 1f;
            //}

            GetShotInfo(out Vector3 direction, out Vector3 spawnPosition);

            Projectile shot = Instantiate(projectile, spawnPosition, Quaternion.identity);
            shot.SetDirection(direction);
            shot.SetEnergyOwner(energyOwner);
            timer = shotInterval;
        }

    }

    public void GetShotInfo(out Vector3 direction, out Vector3 spawnPosition)
    {
        Vector2 aim = inputReader.MoveAim;
        direction = new Vector3(aim.x, aim.y, 0f);

        if (direction.x != 0f && direction.y != 0f)
        {
            direction.y *= 0.8f;
        }

        //if player is stationary shoot in the last facing direction
        if (direction == Vector3.zero)
        {
            direction = new Vector3(movement.FacingDirection, 0f, 0f);
        }

        //change the firepoint position based on the facing direction
        Vector3 point = firePoint.transform.localPosition;
        point.x = Mathf.Abs(point.x) * movement.FacingDirection;
        firePoint.transform.localPosition = point;

        //spawn a projectile
        spawnPosition = firePoint.transform.position;

        if (direction.x == 0)
        {
            if (direction.y > 0)
            {
                spawnPosition = firePointUp.transform.position;
            }
            else if (direction.y < 0)
            {
                spawnPosition = firePointDown.transform.position;
            }
        }
        
    }

}
