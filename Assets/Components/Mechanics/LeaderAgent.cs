using UnityEngine;
using UnityEngine.Events;

public class LeaderAgent : MonoBehaviour
{
    [SerializeField] private Transform[] minions;
    public UnityEvent<Transform> MinionChanged = new UnityEvent<Transform>();

    public int size;

    private void Start()
    {
        this.MinionChanged.AddListener((Transform minion) =>
        {
            this.size = this.minions.Length; 
        });
        
    }
    
    private void Update()
    {
        
    }

    public void AddMinion(Transform minion)
    {
        MinionAgent minionAgent = minion.GetComponent<MinionAgent>();
        this.minions[^1] = minion;
        minionAgent.SetLeader(this.transform);
        
        this.MinionChanged.Invoke(this.minions[^1]);
    }

    public void RemoveMinion()
    {
        Transform minion = this.minions[^1];
        MinionAgent minionAgent = minion.GetComponent<MinionAgent>();
        minionAgent.SetLeader(null);
        
        this.minions[^1] = null;
        this.MinionChanged.Invoke(minion);
    }

    public void TransformMinionsToLeader(LeaderAgent leaderAgent)
    {
        
    }
}
