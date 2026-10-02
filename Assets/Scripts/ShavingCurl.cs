using UnityEngine;

public class ShavingCurl : MonoBehaviour
{
    private enum TravelState
    {
        UpperSweep,
        Turning,
        LowerSweep
    }

    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 1;

    [SerializeField] private Transform upperTurn;
    [SerializeField] private Transform lowerTurn;
    [SerializeField] private Transform lowerEnd;
    [SerializeField] private TravelState state = TravelState.UpperSweep;

    private Rigidbody rb;
    private float turnAngle = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //upper sweep branch
        if(state == TravelState.UpperSweep)
        {
            //calculate the next position toward the upper turn
            Vector3 nextPosition = Vector3.MoveTowards(rb.position, upperTurn.position, (speed * Time.fixedDeltaTime));
            rb.MovePosition(nextPosition);
            if(nextPosition == upperTurn.position)
            {
                state = TravelState.Turning;
            }
        }
        //turning branch
        else if(state == TravelState.Turning)
        {
            //calculate a half circle from upper turn to lower turn
            Vector3 center = (upperTurn.position + lowerTurn.position) * 0.5f;
            float radius = (upperTurn.position.y - lowerTurn.position.y) * 0.5f;
            turnAngle += (speed / radius) * Time.fixedDeltaTime;
            turnAngle = Mathf.Min(turnAngle, Mathf.PI);
            Vector3 nextPosition = new Vector3(
                center.x - radius * Mathf.Sin(turnAngle),
                center.y + radius * Mathf.Cos(turnAngle),
                center.z
            );

            if (turnAngle >= Mathf.PI)
            {
                nextPosition = lowerTurn.position;
                state = TravelState.LowerSweep;
            }

            rb.MovePosition(nextPosition);
        }
        else if(state == TravelState.LowerSweep)
        {
            //calculate the next position toward the lower end
            Vector3 nextPosition = Vector3.MoveTowards(rb.position, lowerEnd.position, (speed * Time.fixedDeltaTime));
            rb.MovePosition(nextPosition);
            if (nextPosition == lowerEnd.position)
            {
                Destroy(gameObject);
            }
        }


    }
}
