using SkalluUtils.Utils.Sound;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager _self;
    public static SoundManager self => _self;

    public Sound[] globalSounds;
    private AudioSource globalAudioSource;

    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _self = this;

            globalAudioSource = GetComponent<AudioSource>();
        }
    }

}