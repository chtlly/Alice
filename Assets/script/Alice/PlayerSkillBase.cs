using UnityEngine;

public class PlayerSkillBase : MonoBehaviour
{
    [Header("스킬 공통 설정")]
    public float cooldownTime = 5.0f; // 쿨타임
    public float manaCost = 10.0f;    // 마나 소모량
    public GameObject skillPrefab;    // 스킬에 사용할 프리팹 (이펙트/투사체 등)

    protected float currentCooldown = 0.0f; // 현재 남은 쿨타임
    protected Playeractive player;          // 플레이어 스크립트 참조

    protected virtual void Start()
    {
        // 1. 내 부모나 나한테서 플레이어 찾기 (슬롯이 자식으로 있으므로 GetComponentInParent)
        player = GetComponentInParent<Playeractive>();

        // 2. 못 찾았으면 이름으로 찾기
        if (player == null)
        {
            GameObject obj = GameObject.Find("Player");
            // 혹시 이름이 Alice일수도 있으니 예비책
            if (obj == null) obj = GameObject.Find("Alice");

            if (obj != null) player = obj.GetComponent<Playeractive>();
        }
    }

    protected virtual void Update()
    {
        // 쿨타임이 돌아가는 중이면 시간을 줄임
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    // 스킬 매니저가 호출하는 함수 ("스킬 발동 시도해!")
    public void TryUseSkill()
    {
        // 1. 쿨타임 체크
        if (currentCooldown > 0)
        {
            Debug.Log($"[Skill] 쿨타임 중입니다... ({currentCooldown:F1}초 남음)");
            return;
        }

        // 2. 마나 체크 및 사용
        // (player가 없거나, 마나가 부족하면 실행 안 함)
        if (player != null && player.UseMana(manaCost))
        {
            // 성공!
            currentCooldown = cooldownTime; // 쿨타임 초기화
            ActivateSkill(); // 실제 스킬 로직 실행 (자식이 구현함)
        }
        else if (player == null)
        {
            Debug.LogError("플레이어 스크립트를 찾을 수 없습니다!");
        }
    }

    // 자식 스크립트(Skill_Annihilation 등)가 덮어쓸(override) 내용
    protected virtual void ActivateSkill()
    {
    }
}