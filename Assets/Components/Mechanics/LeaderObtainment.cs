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
        Debug.Log("Trigger enter STATE");
        if (other.CompareTag("Minion"))
        {
            other.TryGetComponent(out MinionAgent minionAgent);

            if (minionAgent)
            {
                Transform ifLeader = minionAgent.GetLeader();
                if (ifLeader) return;
                
                Debug.Log("Add Minion");
                this.leaderAgent.AddMinion(other.transform);
            }
        }
    }
}
