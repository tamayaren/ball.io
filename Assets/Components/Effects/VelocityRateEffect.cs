using UnityEngine;
using UnityEngine.AI;

public class VelocityRateEffect : MonoBehaviour
{
    private ParticleSystem particles;
    private ParticleSystem.MainModule particlesMain;
    private ParticleSystem.EmissionModule emission;

    public float emissionMultiplier = 8f;
    private NavMeshAgent parentNavMeshAgent;
    private Renderer parentRenderer;

    private void Start()
    {
        this.transform.parent.TryGetComponent(out this.parentNavMeshAgent);
        this.transform.parent.TryGetComponent(out this.parentRenderer);
        
        TryGetComponent(out this.particles);
        this.emission = this.particles.emission;
        this.particlesMain = this.particles.main;
    }

    private void Update()
    {
        if (!this.parentNavMeshAgent) return;
        float currentSpeed = this.parentNavMeshAgent.velocity.magnitude;
        
        this.emission.rateOverTime = currentSpeed / this.emissionMultiplier;
        if (!this.parentRenderer) return;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(this.parentRenderer.material.color, 0.0f), new GradientColorKey(this.parentRenderer.material.color, 1.0f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(0f, 1f)
            }
            );
        
        this.particlesMain.startColor = new ParticleSystem.MinMaxGradient(gradient);
    }
}
