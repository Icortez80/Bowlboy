using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] public Projectile projectile;
    [SerializeField] public GameObject firePoint;
    [SerializeField] public GameObject firePointUp;
    [SerializeField] public GameObject firePointDown;
    [SerializeField] public float shotInterval = .5f;
    private float timer = 0f;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(Keyboard.current.pKey.isPressed && timer <= 0)
        {
            Vector3 direction = new Vector3(0f, 0f, 0f);
            if (Keyboard.current.dKey.isPressed)
            {
                direction.x += 1f;
            }
            if (Keyboard.current.aKey.isPressed)
            {
                direction.x += -1f;
            }
            if (Keyboard.current.sKey.isPressed)
            {
                direction.y += -1f;
            }
            if (Keyboard.current.wKey.isPressed)
            {
                direction.y += 1f;
            }
            if (direction.x != 0f && direction.y != 0f)
            {
                direction.y *= 0.6f;
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

            if(direction.x == 0)
            {
                if(direction.y > 0)
                {
                    Projectile shot = Instantiate(projectile, firePointUp.transform.position, Quaternion.identity);
                    shot.SetDirection(direction);
                }else if(direction.y < 0)
                {
                    Projectile shot = Instantiate(projectile, firePointDown.transform.position, Quaternion.identity);
                    shot.SetDirection(direction);
                }
                //timer = shotInterval;
            }
            else
            {
                //spawn a projectile
                Projectile shot = Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
                shot.SetDirection(direction);
                //timer = shotInterval;
            }
            timer = shotInterval;
        }

    }
}
