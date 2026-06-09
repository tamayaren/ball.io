using UnityEngine;
using UnityEngine.AI;

public class AgentWanderingState : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField] private float timer;
    [SerializeField] [Range(1f, 50f)] private float wanderRadius;
    [SerializeField] [Range(1f, 50f)] private float chaseRadius;
    [SerializeField] [Range(1f, 180f)] private float dotFieldOfView;

    [SerializeField] [Range(5f, 35f)] public float randomSpeed = 5f;
    
    private LeaderAgent targetLeaderAgent;
    [SerializeField] private Transform target;
    [SerializeField] private AgentState state = AgentState.Wander;

    [SerializeField] private Vector3 targetPosition;

    private int seed;
    
    private void Start()
    {
        TryGetComponent(out this.agent);
        this.randomSpeed = Random.Range(5f, this.randomSpeed);
        
        this.agent.speed = this.randomSpeed;
    }

    private bool IsTargetVisible()
    {
        Vector3 directionToTarget = (this.target.position - this.transform.position).normalized;
        float distanceToTarget = Vector3.Distance(this.transform.position, this.target.position);
        
        Physics.Raycast(this.transform.position, directionToTarget * this.chaseRadius, out RaycastHit hit, distanceToTarget);
        if (distanceToTarget <= this.chaseRadius && hit.collider != null)
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                return false;
            }
            
            float angleCosineThreshold = Mathf.Cos(this.dotFieldOfView * .5f * Mathf.Deg2Rad);
            if (Vector3.Dot(this.transform.forward, directionToTarget) >= angleCosineThreshold)
            {
                return true;
            }
        }
        
        return false;
    }

    private void Wander()
    {
        if (!this.agent.pathPending && this.agent.remainingDistance <= this.agent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Mathf.Max(this.wanderRadius, 5f);
            Vector3 computed = this.transform.position + randomDirection;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(computed, out hit, 1.0f, NavMesh.AllAreas))
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
        
        // TODO Implement Chase & Flee Dynamic State
    }
    
    public void SetSpeed(float speed) => this.agent.speed = speed;
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
        
        if (NavMesh.SamplePosition(computed, out hit, 5.0f, NavMesh.AllAreas))
            this.agent.SetDestination(hit.position);
    }

    public void SetTarget(Transform target = null)
    {
        this.target = target;
        target.TryGetComponent(out this.targetLeaderAgent);
    } 
    public void SetState(AgentState newState) => this.state = newState;
    public void SetSeed(int seed) => this.seed = seed;

    private void Update()
    {
        if (!this.target && this.state == AgentState.Wander)
            Wander();
        else
            switch (this.state)
            {
                case AgentState.Chase:
                    Chase();
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
    Follow
}