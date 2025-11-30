using UnityEngine;

public class PlayerSkillBase : MonoBehaviour
{
    [Header("스킬 공통 설정")]
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

    // [중요] virtual을 붙여야 자식(차원도약)이 자기 맘대로 고쳐 쓸 수 있음
    public virtual void TryUseSkill()
    {
        if (currentCooldown > 0)
        {
            Debug.Log($"[Skill] 쿨타임... ({currentCooldown:F1}초)");
            return;
        }

        if (player != null && player.UseMana(manaCost))
        {
            currentCooldown = cooldownTime;
            ActivateSkill();
        }
    }

    protected virtual void ActivateSkill()
    {
    }
}