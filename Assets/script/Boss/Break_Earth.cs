using UnityEngine;

public class Break_Earth : BossAttackBase
{
    public float time = 1.0f;
    public float moveSpeed = 10.0f;
    SpriteRenderer direction;

    protected override void Start()
    {
        base.Start();
        this.direction = GetComponent<SpriteRenderer>();
        if (bossactive != null)
        {
            if (bossactive.bossrenderer.flipX == false) { this.direction.flipX = true; moveSpeed = 10.0f; }
            else { this.direction.flipX = false; moveSpeed = -10.0f; }
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if (time <= 0)
        {
            Destroy(gameObject);
            if (bossactive != null) bossactive.BossSkillend();
        }
    }

    // [덮어쓰기] 최대 체력 비례 데미지
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Playeractive player = other.GetComponent<Playeractive>();
        if (player != null)
        {
            float damageAmount = player.MaxHp * 0.10f; // 10%
            Debug.Log($"브레이크 어스 적중! (데미지: {damageAmount})");
            player.TakeDamage(damageAmount);
        }
    }
}