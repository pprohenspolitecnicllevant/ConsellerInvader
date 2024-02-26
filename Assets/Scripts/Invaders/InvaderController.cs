using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float distanceToStopAttack = 12;

    private Rigidbody rb;
    private List<Transform> wayPoints;
    private int wayPointIndex = 0;
    private Transform finalDestination;

    private Vector3 finalPosition;

    public bool hasFinishedAttack;
    public bool isGoingToPlayer;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        speed = Random.Range(StatsManager.invader.motion.minSpeed, StatsManager.invader.motion.maxSpeed);
        distanceToStopAttack = StatsManager.invader.shooting.distanceStopAttack;

        hasFinishedAttack = false;
        isGoingToPlayer = false;
    }

    private void Update()
    {
        if (target is not null && !hasFinishedAttack)
            transform.LookAt(target);

        if (finalDestination != null)
            finalPosition = finalDestination.position;

        AudioManager.I.PlaySound(SoundName.InvaderZoom, transform.position);
    }

    private void FixedUpdate()
    {
        if (wayPointIndex < wayPoints.Count && wayPoints[wayPointIndex] != null)
        {
            Transform curretnWayPoint = wayPoints[wayPointIndex];
            Vector3 direction = curretnWayPoint.position - transform.position;
            rb.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
            float distanceToPoint = Vector3.Distance(transform.position, curretnWayPoint.position);
            if (distanceToPoint <= 0.5)
            {
                wayPointIndex++;
            }
        } else
        {
            Vector3 targetDirection;
            
            if (target != null && !hasFinishedAttack)
            {
                float distanceToPlayer = Vector3.Distance(GetHorizontalPosition(transform.position), GetHorizontalPosition(target.position));
                targetDirection = GetHorizontalPosition(target.position) - GetHorizontalPosition(transform.position);
                hasFinishedAttack = distanceToPlayer < distanceToStopAttack;
                isGoingToPlayer = true;
            } else
            {
                isGoingToPlayer = false;
                targetDirection = finalPosition - transform.position;
            }

            if (Vector3.Distance(transform.position, finalPosition) < 1)
                Destroy(gameObject);
            
            rb.MovePosition(transform.position + targetDirection.normalized * speed * Time.deltaTime);
        }
        
    }

    private Vector3 GetHorizontalPosition(Vector3 originalPosition)
    {
        return new Vector3(originalPosition.x, 0, originalPosition.z);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetWaypoints(List<Transform> _wayPoints)
    {
        wayPoints = _wayPoints;
    }

    public void SetFinalDestination(Transform _finalDestination)
    {
        finalDestination = _finalDestination;
    }
}
