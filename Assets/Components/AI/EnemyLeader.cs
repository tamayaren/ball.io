using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLeader : MonoBehaviour
{
    private static int chaseRange = 64;
    private static int minionRange = 32;
    private static float pollingRate = .3f;

    private float pollTime;
    [SerializeField] private Transform target;
    
    private AgentWanderingState agentWanderingState;
    private LeaderAgent leaderAgent;

    private NavMeshAgent playerNavMeshAgent;
    private LeaderAgent playerLeaderAgent;
    
    
    private void Awake()
    {
        TryGetComponent(out this.agentWanderingState);
        TryGetComponent(out this.leaderAgent);
    }

    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
        
        this.target.TryGetComponent(out this.playerLeaderAgent);
        this.target.TryGetComponent(out this.playerNavMeshAgent);
    }


    private Transform FindNearestMinion()
    {
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("Minion"))
        {
            float distance = Vector3.Distance(this.transform.position, child.transform.position);
            if (distance < minionRange)
            {
                if (!this.leaderAgent.minions.Contains(child.transform))
                    return child.transform;
            }
        }

        return null;
    }

    private void FixedUpdate()
    {
        this.agentWanderingState.SetTarget(this.target);
        this.agentWanderingState.SetFleeDistance(EnemyLeader.chaseRange);
        this.agentWanderingState.SetSpeed(this.playerNavMeshAgent.speed * .8f);
        this.agentWanderingState.SetChaseRadius(chaseRange);
        
        float distance =  Vector3.Distance(this.transform.position, this.target.position);
        if (distance <= EnemyLeader.chaseRange)
        {
            this.agentWanderingState.SetTarget(this.target);
            this.agentWanderingState.SetState(this.playerLeaderAgent.size > this.leaderAgent.size ? AgentState.Flee : AgentState.Chase);
            return;
        }
        
        this.pollTime += Time.fixedDeltaTime;
        if (this.pollTime >= pollingRate)
        {
            this.pollTime = 0f;
            Transform nearestMinion = FindNearestMinion();

            if (nearestMinion != null)
            {
                this.agentWanderingState.SetTarget(nearestMinion);
                this.agentWanderingState.SetState(AgentState.Chase);
            }
            else
            {
                this.agentWanderingState.SetState(AgentState.Wander);
            }
        }
    }
}
