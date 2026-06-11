using UnityEngine;
using UnityEngine.AI;

public class MinionAgent : MonoBehaviour
{
    public static Color minionDefaultColor = Color.gray8;
    //

    private AgentWanderingState agentAI;
    private NavMeshAgent agent;
    private Renderer renderer;
    
    [SerializeField] private Transform currentLeader;
    private Transform lastLeader;
    private void Awake()
    {
        TryGetComponent(out this.agentAI);
        TryGetComponent(out this.agent);
        TryGetComponent(out this.renderer);
    }

    private void Start()
    {
        this.renderer.material.color = minionDefaultColor;
    }

    public void SetLeader(LeaderAgent leader = null)
    {
        SoundManager.instance.PlaySoundAtPoint(this.transform.position, 1, 10f);
        leader.TryGetComponent(out NavMeshAgent agent);
        this.currentLeader = leader.transform;
        this.renderer.material.color = leader.leaderColor;

        this.agentAI.SetTarget(this.currentLeader);
        this.agentAI.SetState(this.currentLeader == null ? AgentState.Wander : AgentState.Follow);
        this.agentAI.SetSeed(leader.leaderSeed);
        
        this.agentAI.SetSpeed(this.currentLeader ? agent.speed * 2f : this.agentAI.randomSpeed);
        //
        // Physics.IgnoreCollision(GetComponent<Collider>(), this.currentLeader.GetComponent<Collider>(), false);
        // if (this.lastLeader)
        //     Physics.IgnoreCollision(GetComponent<Collider>(), this.lastLeader.GetComponent<Collider>(), true);
        //
        this.agent.radius = leader != null ? .01f : 1f;
        this.lastLeader = leader.transform;
    }
    
    public Transform GetLeader() => this.currentLeader;
}
