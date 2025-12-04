using UnityEngine;

public class PlayerSkillBase : MonoBehaviour
{
    [Header("Skill Settings")]
    public float cooldownTime = 5.0f;
    public float manaCost = 10.0f;
    public GameObject skillPrefab;

    protected float currentCooldown = 0.0f;
    protected Playeractive player;

    protected virtual void Start()
    {
        player = GetComponentInParent<Playeractive>();
        if (player == null)
        {
            GameObject obj = GameObject.Find("Player");
            if (obj != null) player = obj.GetComponent<Playeractive>();
        }
    }

    protected virtual void Update()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
    }

    public virtual void TryUseSkill()
    {
        if (currentCooldown > 0) return;

        if (player != null && player.UseMana(manaCost))
        {
            currentCooldown = cooldownTime;
            ActivateSkill();
        }
    }

    protected virtual void ActivateSkill()
    {
    }

    public virtual void ResetCooldown()
    {
        currentCooldown = 0.0f;
    }

    public virtual float GetCurrentCooldown()
    {
        if (cooldownTime <= 0) return 0;
        return currentCooldown / cooldownTime;
    }

    public virtual int GetStackCount()
    {
        return -1;
    }
}