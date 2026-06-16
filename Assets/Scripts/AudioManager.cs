using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Источники звука")]
    public AudioSource musicSource;  
    public AudioSource sfxSource;     

    [Header("Музыка")]
    public AudioClip backgroundMusic;

    [Header("Звуки и их громкость")]
    public AudioClip shootClip;                              
    [Range(0f, 1f)] public float shootVolume = 0.4f;        

    public AudioClip moneyClip;                             
    [Range(0f, 1f)] public float moneyVolume = 0.6f;

    public AudioClip explosionClip;
    [Range(0f, 1f)] public float explosionVolume = 1f;

    public AudioClip damageClip;
    [Range(0f, 1f)] public float damageVolume = 1f;

    public AudioClip noMoneyClip;                            
    [Range(0f, 1f)] public float noMoneyVolume = 0.8f;

    public AudioClip buildClip;
    [Range(0f, 1f)] public float buildVolume = 1f;

    public AudioClip upgradeClip;
    [Range(0f, 1f)] public float upgradeVolume = 1f;

    public AudioClip sellClip;
    [Range(0f, 1f)] public float sellVolume = 1f;

    public AudioClip loseClip;
    [Range(0f, 1f)] public float loseVolume = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("musicVol", 0.6f));
        SetSfxVolume(PlayerPrefs.GetFloat("sfxVol", 1f));

        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }


    void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }
    public void SetMusicVolume(float value)
    {
        if (musicSource != null) musicSource.volume = value;
        PlayerPrefs.SetFloat("musicVol", value);
    }

    public void SetSfxVolume(float value)
    {
        if (sfxSource != null) sfxSource.volume = value;
        PlayerPrefs.SetFloat("sfxVol", value);
    }

    public float MusicVolume => PlayerPrefs.GetFloat("musicVol", 0.6f);
    public float SfxVolume => PlayerPrefs.GetFloat("sfxVol", 1f);

    public static void Shoot() { if (Instance) Instance.PlaySFX(Instance.shootClip, Instance.shootVolume); }
    public static void Money() { if (Instance) Instance.PlaySFX(Instance.moneyClip, Instance.moneyVolume); }
    public static void Explosion() { if (Instance) Instance.PlaySFX(Instance.explosionClip, Instance.explosionVolume); }
    public static void Damage() { if (Instance) Instance.PlaySFX(Instance.damageClip, Instance.damageVolume); }
    public static void NoMoney() { if (Instance) Instance.PlaySFX(Instance.noMoneyClip, Instance.noMoneyVolume); }
    public static void Build() { if (Instance) Instance.PlaySFX(Instance.buildClip, Instance.buildVolume); }
    public static void Upgrade() { if (Instance) Instance.PlaySFX(Instance.upgradeClip, Instance.upgradeVolume); }
    public static void Sell() { if (Instance) Instance.PlaySFX(Instance.sellClip, Instance.sellVolume); }

    public static void Lose()
    {
        if (!Instance) return;
        Instance.PlaySFX(Instance.loseClip, Instance.loseVolume);
        if (Instance.musicSource) Instance.musicSource.Stop();
    }
}