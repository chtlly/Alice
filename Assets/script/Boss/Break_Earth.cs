using UnityEngine;

public class Break_Earth : MonoBehaviour
{
    public float time;
    public float speed;
    SpriteRenderer direction;
    Bossactive bossactive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 1.0f;
        bossactive = GameObject.Find("Rabbit_Ssi").GetComponent<Bossactive>();
        this.direction = GetComponent<SpriteRenderer>();
        //보스 방향에 따라 뒤집기 + 이동 방향 지정
        if (bossactive.bossrenderer.flipX == false)
        {
            this.direction.flipX = true;
            speed = 10.0f;
        }
        else
        {
            this.direction.flipX = false;
            speed = -10.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        this.transform.Translate(transform.right * speed * Time.deltaTime);
        if (time <= 0)
        {
            Destroy(gameObject);
            bossactive.BossSkillend();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 들어온 오브젝트의 태그가 Player면
        if (other.CompareTag("Player"))
        {
            bossactive.CurrentHp -= 10;
        }
    }
}
