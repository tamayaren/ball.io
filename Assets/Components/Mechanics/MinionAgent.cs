using UnityEngine;
using UnityEngine.AI;

public class MinionAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform currentLeader;

    private void Awake()
    {
        this.agent = GetComponent<NavMeshAgent>();
    }

    public void SetLeader(Transform leader = null)
    {
        this.currentLeader = leader;
    }
}
