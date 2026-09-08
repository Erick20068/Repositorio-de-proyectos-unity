using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;

    public Transform[] destinos;

    public float distanceToFollowPath = 2;

    private int i = 0;

    [Header("-----------Follow Player?-----------")]

    public bool followPlayer;

    private GameObject player;

    private float distanceToPlayer;

    private float distanceToFollowPlayer = 10;



    
    void Start()
    {
        navMeshAgent.destination = destinos[i].transform.position;
        player = FindAnyObjectByType<PlayerMovimiento>().gameObject;
    
    }

    
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);
        if (distanceToPlayer <= distanceToFollowPlayer && followPlayer)
        {
            FollowPlayer();
        }
        else
        {
            EnemyPath();
        }
   
    }

    public void EnemyPath()
    {
        navMeshAgent.destination = destinos[i].position;

        if (Vector3.Distance(transform.position, destinos[i].position) <= distanceToFollowPath)
        {
            if (destinos[i] != destinos[destinos.Length - 1])
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }

    public void FollowPlayer()
    {
        navMeshAgent.destination = player.transform.position;
    }
}
