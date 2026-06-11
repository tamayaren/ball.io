using System.Collections;
using UnityEngine;

public class GameEffectsMain : MonoBehaviour
{
    public static GameEffectsMain instance { get; private set; }

    private void Awake() => instance = this;

    [SerializeField] private GameObject explosionPrefab;

    private IEnumerator Debris(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
            Destroy(obj);
    }
    
    public void Explode(Vector3 position, Color color)
    {
        GameObject explosion = Instantiate(this.explosionPrefab,  position, Quaternion.identity);
        
        ParticleSystem explosionParticle = explosion.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = explosionParticle.main;

        SoundManager.instance.PlaySoundAtPoint(position, 0, 20f);
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(0f, 1f)
            }
        );
        
        main.startColor = new ParticleSystem.MinMaxGradient(gradient);
        explosionParticle.Emit(128);
        StartCoroutine(Debris(explosion, explosionParticle.main.duration));
    }
}
