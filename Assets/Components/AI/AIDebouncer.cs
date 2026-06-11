using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AIDebouncer : MonoBehaviour
{
    public static AIDebouncer instance { get; private set; }
    public float calculationTimeOut = 2.0f;
    private List<AgentWanderingState> registeredAgents = new List<AgentWanderingState>();
    
    private void Awake() => instance = this;
    private void Start() => StartCoroutine(ProcessLogicRoutine());
    
    public void RegisterAgent(AgentWanderingState agent) => this.registeredAgents.Add(agent);
    public void UnregisterAgent(AgentWanderingState agent) => this.registeredAgents.Remove(agent);

    private IEnumerator ProcessLogicRoutine()
    {
        Stopwatch stopwatch = new();

        while (true)
        {
            stopwatch.Restart();

            for (int i = this.registeredAgents.Count - 1; i >= 0; i--)
            {
                if (this.registeredAgents[i] != null)
                {
                    this.registeredAgents[i].EnumerateAIState();
                }

                if (stopwatch.Elapsed.TotalMilliseconds > this.calculationTimeOut)
                {
                    stopwatch.Stop();
                    yield return null;
                    stopwatch.Start();
                }
            }

            yield return null;
        }
    }
}
