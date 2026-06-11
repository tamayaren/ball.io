using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class LeaderAgent : MonoBehaviour
{
    private Renderer renderer;
    
    [SerializeField] public List<Transform> minions = new List<Transform>();
    public UnityEvent<Transform> MinionChanged = new UnityEvent<Transform>();

    public Color leaderColor;
    public int size;

    public int leaderSeed;
    

    private void Awake()
    {
        TryGetComponent(out this.renderer);
        this.leaderColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        this.leaderSeed = Random.Range(0, int.MaxValue);
    }
    
    private void Start()
    {
        this.MinionChanged.AddListener((Transform minion) =>
        {
            this.size = this.minions.Count; 
        });
        
        this.renderer.material.color = this.leaderColor;
    }

    private void OnDestroy()
    {
        GameEffectsMain.instance.Explode(this.transform.position, this.leaderColor);
    }

    public void AddMinion(Transform minion)
    {
        MinionAgent minionAgent = minion.GetComponent<MinionAgent>();
        minionAgent.SetLeader(this);
        
        this.minions.Add(minion);
        this.MinionChanged.Invoke(minion);
    }

    public void RemoveMinion(Transform minion)
    {
        MinionAgent minionAgent = minion.GetComponent<MinionAgent>();
        minionAgent.SetLeader();
        
        this.minions.Remove(minion);
        this.MinionChanged.Invoke(minion);
    }

    public void TransformMinionsToLeader(LeaderAgent leaderAgent)
    {
        foreach (Transform minion in this.minions)
        {
            leaderAgent.AddMinion(minion);
        }
        
        this.minions.Clear();
    }
}
