using UnityEngine;

[CreateAssetMenu]
public class AudioClipCollection : ScriptableObject
{
    [SerializeField] private AudioClip[] _clips;

    public AudioClip GetRandomClip()
    {
        return _clips?[Random.Range(0, _clips.Length)];
    }
}
