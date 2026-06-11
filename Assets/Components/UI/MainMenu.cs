using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform ballSpawn; 
    
    private float timer;
    private float nextBall = 1f;

    private void Update()
    {
        this.timer += Time.deltaTime;
        if (this.timer >= this.nextBall)
        {
            this.timer = 0f;
            Transform sBall = Instantiate(this.ball, this.ballSpawn.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)), Quaternion.identity).transform;
            
            sBall.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }
    
    public void Play()
    {
        SceneManager.LoadScene(0);
    }
}
