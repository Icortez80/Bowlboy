using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] public Collider playerCollider;
    [SerializeField] public Collider platformCollider;
    // Update is called once per frame
    private bool collisionAllowed = false;
    private bool dropThroughActive = false;

    void FixedUpdate()
    {
        if (dropThroughActive && !(playerCollider.bounds.max.y < platformCollider.bounds.min.y))
        {
            Physics.IgnoreCollision(playerCollider, platformCollider, true);
        }
        else
        {
            dropThroughActive = false;

            if (!collisionAllowed)
            {
                if (playerCollider.bounds.min.y >= (platformCollider.bounds.max.y - 0.05f))
                {
                    collisionAllowed = true;
                }
            }
            else
            {
                if (playerCollider.bounds.max.y < platformCollider.bounds.min.y)
                {
                    collisionAllowed = false;
                }
            }

            Physics.IgnoreCollision(playerCollider, platformCollider, !collisionAllowed);
        }
    }

    public void DropThrough()
    {
        dropThroughActive = true;
    }
}
