using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private float hitInvulnerabilityDuration = 1f;
    private float hitInvulnerabilityRemaining = 0f;
    [SerializeField]private int currentHealth;
    [SerializeField] private SpriteRenderer healthMarker1;
    [SerializeField] private SpriteRenderer healthMarker2;
    [SerializeField] private SpriteRenderer healthMarker3;
    [SerializeField] private Animator animator;
    [SerializeField] private SceneController sceneManager;
    [SerializeField] private Volume globalVolume;
    private ChromaticAberration aberration;

    public bool IsDefeated => currentHealth <= 0;

    private PlayerSpecialAttack special;

    private void Awake()
    {
        special = GetComponent<PlayerSpecialAttack>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        if (globalVolume == null)
        {
            globalVolume = FindAnyObjectByType<Volume>();
        }

        // 2. Fetch the Chromatic Aberration settings from the profile
        if (globalVolume != null && globalVolume.profile.TryGet(out aberration))
        {
            // 3. Override and change the intensity (Value ranges from 0 to 1)
            aberration.intensity.Override(0.2f);
        }
    }
    private void Update()
    {
        if(hitInvulnerabilityRemaining > 0)
        {
            hitInvulnerabilityRemaining -= Time.deltaTime;
        }
        updateUI();
    }

    public bool TakeDamage(int damage)
    {
        //player already DED
        if (currentHealth <= 0) return false;
        //player currently using super
        if (special.IsSuperActive) return false;
        if (hitInvulnerabilityRemaining > 0) return false;
        if (sceneManager.GameOver) return false;
       
        currentHealth -= damage;
        IncreaseAberrationIntensity();
        animator.SetTrigger("Hurt");
        SoundController.Instance.Play(SoundController.SoundEffect.PlayerHurt);
        //print("Player health -1");
        hitInvulnerabilityRemaining = hitInvulnerabilityDuration;
        if (currentHealth <= 0)
        {
            print("Player defeated!");
            sceneManager.EndGame(false);
        }

        return true;
    }

    private void updateUI()
    {
        if(currentHealth == 3)
        {
            return;
        }
        else if (currentHealth == 2)
        {
            //disable health marker3
            healthMarker3.enabled = false;
        }
        else if(currentHealth == 1)
        {
            //disable health marker2
            healthMarker2.enabled = false;
        }
        else
        {
            //disable health marker1
            healthMarker1.enabled = false;

        }
    }

    public void IncreaseAberrationIntensity()
    {
        if (aberration != null)
        {
            aberration.intensity.value += Mathf.Clamp01(.1f);
        }
    }
}
