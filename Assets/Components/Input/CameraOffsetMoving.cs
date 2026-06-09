using UnityEngine;

public class CameraOffsetMoving : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CameraMain cameraMain;
    
    [SerializeField] private Vector3 rawDelta;
    [SerializeField] private Vector3 delta;

    [SerializeField] private float offsetMultiplier;
    private void Awake()
    {
        if (!this.target)
            this.target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
    
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(this.target.up, this.target.position);

        if (playerPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            
            this.rawDelta = worldPosition - this.target.position;
            this.delta = this.rawDelta.normalized;
        }
        
        this.cameraMain.cameraOffset = this.delta * this.offsetMultiplier;
    }
}
