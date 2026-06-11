using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
   
    [SerializeField] private AudioClip[] audioIndexes;

    private void Awake() => instance = this;

    public void PlaySoundAtPoint(Vector3 position, int index, float volume)
    {
        AudioSource.PlayClipAtPoint(this.audioIndexes[index], position, volume);
    }
}
