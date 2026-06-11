using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Transform currentObstacle;
    
    private void Update()
    {
        Vector3 direction = this.target.position - this.transform.position;
        RaycastHit hit;
        
        if (Physics.Raycast(this.transform.position, direction, out hit, 2048f))
        {
            Debug.DrawRay(this.transform.position, this.transform.position - hit.point, Color.red); 
            if (!hit.transform.CompareTag("Obstacle"))
            {
                ResetObstacle();
                return;
            };
            if (this.currentObstacle != hit.transform)
            {
                ResetObstacle();
                this.currentObstacle = hit.transform;
                this.currentObstacle.TryGetComponent(out Renderer obstacleRenderer);
                if (obstacleRenderer != null)
                    obstacleRenderer.enabled = false;
            }
        }
        else
            ResetObstacle();
    }

    private void ResetObstacle()
    {
        if (this.currentObstacle == null) return;
        this.currentObstacle.TryGetComponent(out Renderer obstacleRenderer);
        if (obstacleRenderer != null)
        {
            obstacleRenderer.enabled = true;
        }

        this.currentObstacle = null;
    }
}
