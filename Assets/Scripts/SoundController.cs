using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    public enum SoundEffect
    {
        PlayerDash,
        PlayerShot,
        PlayerHurt,
        ExShot,
        SuperShot,

        BossLaugh,
        BoomerangObject,
        BoomerangSpit,
        BeamShot,
        GraphiteBlastCue,
        GraphiteBlastBottom,
        NibBlastCue,

        SpikeRumble,
        SpikeActivate
    }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource beamSource;
    [SerializeField] private AudioSource playerBeamSource;
    [SerializeField] private AudioSource boomerangSource;


    [Header("Player")]
    [SerializeField] private AudioClip playerDash;
    [SerializeField] private AudioClip playerShot;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip exShot;
    [SerializeField] private AudioClip superShot;

    [Header("Boss")]
    [SerializeField] private AudioClip bossLaugh;
    [SerializeField] private AudioClip boomerangObject;
    [SerializeField] private AudioClip boomerangSpit;
    [SerializeField] private AudioClip beamShot;
    [SerializeField] private AudioClip graphiteBlastCue;
    [SerializeField] private AudioClip graphiteBlastBottom;
    [SerializeField] private AudioClip nibBlastCue;

    [Header("Spikes")]
    [SerializeField] private AudioClip spikeRumble;
    [SerializeField] private AudioClip spikeActivate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }
    }

    public void Play(SoundEffect effect)
    {
        AudioClip clip = GetClip(effect);

        if (clip == null) return;

        if (effect == SoundEffect.PlayerShot)
        {
            sfxSource.pitch = Random.Range(0.80f, 1.08f);
            sfxSource.PlayOneShot(clip);
            sfxSource.pitch = 1f;
        }
        else
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private AudioClip GetClip(SoundEffect effect)
    {
        return effect switch
        {
            SoundEffect.PlayerDash => playerDash,
            SoundEffect.PlayerShot => playerShot,
            SoundEffect.PlayerHurt => playerHurt,
            SoundEffect.ExShot => exShot,
            SoundEffect.SuperShot => superShot,

            SoundEffect.BossLaugh => bossLaugh,
            SoundEffect.BoomerangObject => boomerangObject,
            SoundEffect.BoomerangSpit => boomerangSpit,
            SoundEffect.BeamShot => beamShot,
            SoundEffect.GraphiteBlastCue => graphiteBlastCue,
            SoundEffect.GraphiteBlastBottom => graphiteBlastBottom,
            SoundEffect.NibBlastCue => nibBlastCue,

            SoundEffect.SpikeRumble => spikeRumble,
            SoundEffect.SpikeActivate => spikeActivate,

            _ => null
        };
    }

    public void PlayBeam()
    {
        if (beamSource == null || beamShot == null) return;

        beamSource.clip = beamShot;
        beamSource.loop = true;
        beamSource.Play();
    }

    public void StopBeam()
    {
        if (beamSource == null) return;

        beamSource.Stop();
        beamSource.clip = null;
    }

    public void PlayPlayerBeam()
    {
        if (playerBeamSource == null || superShot == null) return;

        playerBeamSource.clip = superShot;
        playerBeamSource.loop = true;
        playerBeamSource.Play();
    }

    public void StopPlayerBeam()
    {
        if (playerBeamSource == null) return;

        playerBeamSource.Stop();
        playerBeamSource.clip = null;
    }

    public void PlayBoomerang()
    {
        if (boomerangSource == null || boomerangObject == null) return;

        boomerangSource.clip = boomerangObject;
        boomerangSource.loop = true;
        boomerangSource.Play();
    }

    public void StopBoomerang()
    {
        if (boomerangSource == null) return;

        boomerangSource.Stop();
        boomerangSource.clip = null;
    }
}