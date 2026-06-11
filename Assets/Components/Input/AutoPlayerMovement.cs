using System;
using UnityEngine;
using UnityEngine.AI;

public class AutoPlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform map;
    [SerializeField] private GameObject target;
    
    private NavMeshAgent agent;
    
    private void Awake()
    {
        if (!this.target)
            this.target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        this.agent = this.target.GetComponent<NavMeshAgent>();
    }

    private void OnDestroy()
    {
        GameUI.instance.Lose();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) return;
        if (hit.collider.transform.parent == this.map)
            this.agent.SetDestination(hit.point);
    }
}
