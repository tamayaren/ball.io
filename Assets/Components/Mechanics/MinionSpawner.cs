using UnityEngine;
using UnityEngine.AI;

public class MinionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private Transform ground;

    [SerializeField] [Range(1, 30)] private int minSpawnRate = 3;
    [SerializeField] [Range(1, 30)] private int maxSpawnRate = 6;

    [SerializeField] [Range(.1f, 1.5f)] private float minSpawnTime = .35f;
    [SerializeField] [Range(.1f, 1.5f)] private float maxSpawnTime = .75f;
    [SerializeField] private float currentSpawnTime;

    [SerializeField] private float spawnRadius;
    [SerializeField] private float timer;

    [SerializeField] private int currentMinions;
    [SerializeField] private int maxMinions;
    
    private void Start()
    {
        this.currentSpawnTime = Random.Range(this.minSpawnTime, this.maxSpawnTime);
    }

    private void Spawn()
    {
        Vector3 randomDirection = Random.insideUnitSphere * this.spawnRadius;
        randomDirection += this.ground.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, this.spawnRadius, NavMesh.AllAreas))
        {
            GameObject minion = Instantiate(this.minionPrefab, hit.position, Quaternion.identity);
            minion.transform.name = $"AI_PLAYER_{(int)Random.Range(1000, 9999)}";
            AgentWanderingState minionState = minion.GetComponent<AgentWanderingState>();
            minionState.OnDestroyAction += (o) => this.currentMinions--;
            this.currentMinions++;
        }
    }
    
    private void Update()
    {
        if (this.currentMinions >= this.maxMinions) return;
        this.timer += Time.deltaTime;

        if (this.timer >= this.currentSpawnTime)
        {
            int currentSpawnRate = (int)Random.Range(this.minSpawnRate, this.maxSpawnRate);
            for (int i = 1; i <= currentSpawnRate; i++)
                Spawn();
            
            this.timer = 0f;
            this.currentSpawnTime = Random.Range(this.minSpawnTime, this.maxSpawnTime);
        }
    }
}
