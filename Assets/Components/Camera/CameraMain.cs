using UnityEngine;

public class CameraMain : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float smoothness;

    [SerializeField] private float zoom = 1f;
    [SerializeField] private float maxZoom = 2.5f;
    [SerializeField] private float scrollMouseSensitivity = .75f;
    public Vector3 cameraOffset;
    public bool lookAt;

    private Camera camera;
    private void Awake()
    {
        if (!this.target)
            this.target = GameObject.FindGameObjectWithTag("Player").transform;
        
        this.camera = Camera.main;
    }

    private void Update()
    {
        this.zoom = Mathf.Clamp(this.zoom - (Input.mouseScrollDelta.y * this.scrollMouseSensitivity), 1f, this.maxZoom);
        
        Vector3 offsetComputed = this.offset * this.zoom;
        Vector3 positionComputed = this.target.position + offsetComputed + this.cameraOffset;
        Quaternion rotationComputed = Quaternion.Euler(this.rotation);
        
        if (this.lookAt)
            this.camera.transform.LookAt(this.target.position);
        else
            this.camera.transform.rotation = Quaternion.Lerp(this.camera.transform.rotation, rotationComputed, this.smoothness * Time.deltaTime);

        this.camera.transform.position = Vector3.Lerp(this.camera.transform.position, positionComputed, this.smoothness * Time.deltaTime);
    }
}
