using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AgentWanderingState : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField] private float timer;
    [SerializeField] [Range(1f, 50f)] private float wanderRadius;
    [SerializeField] [Range(1f, 50f)] private float chaseRadius;

    [SerializeField] [Range(5f, 35f)] public float randomSpeed = 5f;
    
    private LeaderAgent targetLeaderAgent;
    [SerializeField] private Transform target;
    [SerializeField] private AgentState state = AgentState.Wander;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float fleeDistance;

    public event Action<GameObject> OnDestroyAction = new Action<GameObject>(o => {});
    private int seed;
    
    private void Start()
    {
        TryGetComponent(out this.agent);
        this.randomSpeed = Random.Range(5f, this.randomSpeed);
        
        this.agent.speed = this.randomSpeed;
        
        AIDebouncer.instance.RegisterAgent(this);
    }

    private void OnDestroy()
    {
        this.OnDestroyAction.Invoke(this.gameObject);
        AIDebouncer.instance.UnregisterAgent(this);
    }

    private bool IsTargetVisible()
    {
        Vector3 directionToTarget = (this.target.position - this.transform.position).normalized;
        float distanceToTarget = Vector3.Distance(this.transform.position, this.target.position);
        
        Physics.Raycast(this.transform.position, directionToTarget * this.chaseRadius, out RaycastHit hit, distanceToTarget);
        if (distanceToTarget <= this.chaseRadius && hit.collider != null)
        {
            return true;
        }
        
        return false;
    }

    private void Wander()
    {
        if (!this.agent.pathPending && this.agent.remainingDistance <= this.agent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Mathf.Max(this.wanderRadius, 5f);
            randomDirection = new(randomDirection.x, 0f, randomDirection.y);
            Vector3 computed = this.transform.position + randomDirection;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(computed, out hit, this.wanderRadius, NavMesh.AllAreas))
            {
                this.targetPosition = hit.position;
                this.agent.SetDestination(hit.position);
            }
        }
    }
    
    private void Chase()
    {
        if (!this.target)
        {
            Wander();
            return;
        }
        bool targetVisibility = IsTargetVisible();
        
        if (targetVisibility)
            this.agent.SetDestination(this.target.position);
    }
    
    private void Flee()
    {
        if (!this.target)
        {
            Wander();
            return;
        }
        
        bool targetVisibility = IsTargetVisible();

        if (targetVisibility)
        {
            Vector3 fleeDirection = (this.transform.position - this.target.position).normalized;
            Vector3 theoreticalDestination = this.transform.position + fleeDirection * this.fleeDistance;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(theoreticalDestination, out hit, this.fleeDistance, NavMesh.AllAreas))
                this.agent.SetDestination(hit.position);
        }
    }
    
    public void SetSpeed(float speed) => this.agent.speed = speed;
    public void SetFleeDistance(float distance) => this.fleeDistance = distance;
    public void SetChaseRadius(float radius) => this.chaseRadius = radius;
    
    private void Follow()
    {
        if (!this.target || !this.targetLeaderAgent)
        {
            Wander();
            return;
        }
        
        Random.InitState(this.seed);

        Vector3 randomDirection = Random.insideUnitSphere * (5f + this.targetLeaderAgent.size);
        Vector3 computed = this.target.position + randomDirection;
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(computed, out hit, Mathf.Infinity, NavMesh.AllAreas))
            this.agent.SetDestination(hit.position);
    }

    public void SetTarget(Transform target = null)
    {
        this.target = target;
        target.TryGetComponent(out this.targetLeaderAgent);
    } 
    public void SetState(AgentState newState) => this.state = newState;
    public void SetSeed(int seed) => this.seed = seed;
    
    public void EnumerateAIState()
    {
        if (!this.target && this.state == AgentState.Wander)
            Wander();
        else
            switch (this.state)
            {
                case AgentState.Chase:
                    Chase();
                    break;
                case AgentState.Flee:
                    Flee();
                    break;
                case AgentState.Follow:
                    Follow();
                    break;
                default:
                    Wander();
                    break;
            }
    }
}

public enum AgentState
{
    Wander,
    Chase,
    Flee,
    Follow
}