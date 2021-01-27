using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float walkSpeed = 2.5f;
    public float runSpeed = 4.0f;
    public float radiusMovement = 25.0f;
    public float waitTime = 3.0f;
    public float viewRadius = 20.0f;
    [Range(0, 360)]
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public bool playerInRange;

    Transform player;
    NavMeshAgent navMeshAgent;
    Animator m_Animator;
    EnemyHealth enemyHealth;        
    float m_WaitTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        navMeshAgent.speed = walkSpeed;

        m_WaitTimer = waitTime;

        SpawnEnemyRandom();
    }

    private void Update()
    {        
        if (!enemyHealth.isDeath)
        {
            EnviromentView();
            if (playerInRange)
            {
                ChasingPlayer();
            }
            else
            {
                PatrolMovement();
            }
            m_Animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else
        {
            navMeshAgent.speed = walkSpeed;
            navMeshAgent.enabled = false;
        }
    }

    private void SpawnEnemyRandom()
    {
        transform.position = RandomDestination(Vector3.zero, 50.0f);
    }

    private void PatrolMovement()
    {
        navMeshAgent.speed = walkSpeed;
        if (Vector3.Distance(navMeshAgent.destination, transform.position) <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTimer <= 0)
            {
                navMeshAgent.SetDestination(RandomDestination(transform.position, radiusMovement));
                m_WaitTimer = waitTime;
            }
            else
            {
                m_WaitTimer -= Time.deltaTime;
            }
        }
    }

    private void ChasingPlayer()
    {
        navMeshAgent.speed = runSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    private Vector3 RandomDestination(Vector3 center, float distance)
    {
        Vector3 randomPosition = center + Random.insideUnitSphere * distance;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }

    private void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    this.playerInRange = true;
                }
            }
        }
    }
}
