using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LeaderAgent))]
public class LeaderObtainment : MonoBehaviour
{
    private LeaderAgent leaderAgent;

    private void Awake()
    {
        this.leaderAgent = GetComponent<LeaderAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
        {
            other.TryGetComponent(out MinionAgent minionAgent);

            if (minionAgent)
            {
                Transform ifLeader = minionAgent.GetLeader();
                if (ifLeader)
                    return;
                
                if (this.transform.CompareTag("Player"))
                {
                    GameUI.instance.SetEventUI("RECRUITED NEW MINION!");
                }
                this.leaderAgent.AddMinion(other.transform);
            }
        }

        LeaderAgent otherLeader;
        if (other.gameObject.TryGetComponent(out otherLeader))
        {
            if (otherLeader.minions.Count > this.leaderAgent.minions.Count)
            {
                this.leaderAgent.TransformMinionsToLeader(otherLeader);

                if (otherLeader.CompareTag("Player"))
                    WinCondition.instance.IncrementKills();
                
                GameUI.instance.SetEventUI($"{this.transform.name.ToUpper()} KILLED BY {otherLeader.transform.name.ToUpper()}!");   
                Destroy(this.gameObject);
            }
        }
    }
}
