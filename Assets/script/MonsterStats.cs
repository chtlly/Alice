using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [Header("기본 스탯")]
    public int maxHealth = 0;       // 최대 체력
    public int attackDamage = 0;    // 공격력 
    public int defense = 0;         // 방어력

    [Header("보상 설정")]
    public int expReward = 1;
    public int goldReward = 5;

    // 내부 변수
    private int currentHealth;
    private MonsterSpawner mySpawner; // 나를 소환한 스포너 (죽을 때 알리기 위함)

    void Start()
    {
        // 게임 시작 시 현재 체력을 최대 체력으로 설정
        currentHealth = maxHealth;
    }

    // 스포너가 몬스터를 생성할 때 호출해서 연결해주는 함수
    public void Setup(MonsterSpawner spawner)
    {
        mySpawner = spawner;
    }

    // 외부에서 몬스터에게 데미지를 줄 때 호출하는 함수
    public void TakeDamage(int damage)
    {
        // 체력 감소
        currentHealth -= damage;
        Debug.Log(gameObject.name + "의 남은 체력: " + currentHealth);

        // 체력이 0 이하가 되면 사망 처리
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. "Player" 태그를 가진 오브젝트를 찾음
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 2. 플레이어의 스크립트를 가져옴
            PlayerStats playerStats = player.GetComponent<PlayerStats>();

            // 3. 스크립트가 있다면 보상 함수 호출
            if (playerStats != null)
            {
                // 일단 함수 이름을 GetReward로 함
                playerStats.GetReward(expReward, goldReward);
            }
        }
        // 1. 스포너에게 사망 사실 알림 (카운트 감소)
        if (mySpawner != null)
        {
            mySpawner.DecreaseCount();
        }

        // 2. 몬스터 오브젝트 파괴
        Destroy(gameObject);
        Debug.Log(gameObject.name + "가 사망했습니다.");
    }
}
