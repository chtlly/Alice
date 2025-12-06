using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [Header("기본 스탯")]
    public int maxHealth = 50;
    public int attackDamage = 10;
    public int defense = 0;

    [Header("보상 설정 (기존 값 사용)")]
    public int expReward = 1;  // 이 값을 경험치 티켓에 넣을 겁니다.
    public int goldReward = 5; // 이 값을 골드 아이템에 넣을 겁니다.

    [Header("드랍 아이템 프리팹")]
    public GameObject goldItemPrefab; // 골드 아이템 프리팹만 연결하세요.
    public GameObject expItemPrefab;  // 경험치 아이템 프리팹만 연결하세요.

    [Header("UI 설정")]
    public GameObject damageTextPrefab;

    private int currentHealth;
    private MonsterSpawner mySpawner;
    private float damageInterval = 1.0f;
    private float lastDamageTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Setup(MonsterSpawner spawner)
    {
        mySpawner = spawner;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} 피격! 남은 체력: {currentHealth}");

        if (damageTextPrefab != null)
        {
            GameObject hud = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            hud.transform.position += new Vector3(0, 0.5f, 0);
            hud.GetComponent<FloatingText>().SetDamage(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 아이템 드랍 함수 호출
        DropItems();

        if (mySpawner != null)
        {
            mySpawner.DecreaseCount();
        }

        Destroy(gameObject);
    }

    // [수정됨] 기존 보상 변수(goldReward, expReward)를 사용하도록 변경
    void DropItems()
    {
        // 1. 골드 드랍 (설정된 goldReward 만큼)
        if (goldItemPrefab != null && goldReward > 0)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject item = Instantiate(goldItemPrefab, dropPos, Quaternion.identity);

            ItemPickup pickup = item.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                // [핵심] 몬스터마다 다르게 설정된 goldReward 값을 전달!
                pickup.amount = goldReward;
            }
        }

        // 2. 경험치 드랍 (설정된 expReward 만큼)
        if (expItemPrefab != null && expReward > 0)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject item = Instantiate(expItemPrefab, dropPos, Quaternion.identity);

            ItemPickup pickup = item.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                // [핵심] 몬스터마다 다르게 설정된 expReward 값을 전달!
                pickup.amount = expReward;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                Playeractive player = other.GetComponent<Playeractive>();
                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}