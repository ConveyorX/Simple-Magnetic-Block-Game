using ConveyorX.MagneticBricks;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource source;
    AudioSource music;

    private void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
            source = GetComponent<AudioSource>();

            int i = PlayerPrefs.GetInt("SFX", 1);
            source.enabled = i == 1 ? true : false;

            //start music!
            if (AssetsManager.Instance.music != null)
            {
                music = gameObject.AddComponent<AudioSource>();
                music.playOnAwake = true;
                music.clip = AssetsManager.Instance.music;
                music.loop = true;
                music.volume = AssetsManager.Instance.musicVol;
                music.enabled = i == 1 ? true : false;
                music.Play();
            }
        }
    }

    public void Play(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void Toggle(bool val) 
    {
        source.enabled = val;
        music.enabled = val;
    }
}
