using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public SpriteRenderer sRenderer;
    public Animator animator;
    public Transform hurtEffect;
    public string deathSfxName;

    public bool IsDead { get { return currentHealth <= 0; } }
    public bool enableDamageParticles = true;

    public int maxHealth = 100;
    int currentHealth;

    public float deadFadeDelay = 1f;
    public float deadFadeLength = 1f;

    public bool canBeInvincible = false;
    public float invincibleDuration = 3f;
    private float flashDuration = 0.25f;
    WaitForSeconds invincibleFlash;
    private bool isInvincible = false;

    DataManager dataManager;

    IEnumerator Invincibility()
    {
        isInvincible = true;
        float invincibleElapsed = 0f;
        bool inFlash = true;
        while (invincibleElapsed < invincibleDuration)
        {
            ToggleTransparency(inFlash);
            yield return invincibleFlash;
            invincibleElapsed += flashDuration;
            inFlash = !inFlash;
        }
        isInvincible = false;
    }

    void ToggleTransparency(bool isOn)
    {
        if (isOn)
        {
            Color cColor = sRenderer.color;
            sRenderer.color = new Color(cColor.r, cColor.g, cColor.b, 0.30f);
        }
        else
        {
            Color cColor = sRenderer.color;
            sRenderer.color = new Color(cColor.r, cColor.g, cColor.b, 1f);
        }
    }

    void Awake()
    {
        invincibleFlash = new WaitForSeconds(flashDuration);

        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
    }

    void Start()
    {
        currentHealth = dataManager.GetPlayerHealth();
    }

    void OnDisable()
    {
        dataManager.savePlayerHealth(currentHealth);
    }

    public void TakeDamage(Transform attacker, int damage)
    {
        if (isInvincible)
            return;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        Collider2D objectCollider = transform.GetComponent<Collider2D>();

        // generate hurt particles (if enabled)
        if (enableDamageParticles)
        {
            Transform hurtPrefab = Instantiate(hurtEffect,
                    objectCollider.bounds.center,
                    Quaternion.identity);
            hurtPrefab.up = new Vector3(attacker.position.x - objectCollider.transform.position.x, 0f, 0f);
        }

        AudioManager.Instance.PlaySFX("player_take_damage");

        if (currentHealth <= 0)
            Die();

        if (currentHealth > 0 && canBeInvincible)
            StartCoroutine(Invincibility());
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        AudioManager.Instance.PlaySFX(deathSfxName);
        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PlayerMovement>().enabled = false;

        StartCoroutine(AfterDeath());
    }

    IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(deadFadeDelay);
        GameObject.Find("StageLoader").GetComponent<StageLoader>().LoadNewStage("this");

        yield return new WaitForSeconds(deadFadeLength);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<PlayerMovement>().enabled = true;
        currentHealth = maxHealth;
        dataManager.savePlayerHealth(currentHealth);
    }
}
