using UnityEngine;

public class Basic_Attack : MonoBehaviour
{
    Bossactive bossactive;
    SpriteRenderer direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossactive = GameObject.Find("Rabbit_Ssi").GetComponent<Bossactive>();
        this.direction = GetComponent<SpriteRenderer>();
        //보스 방향에 따라 뒤집기
        if (bossactive.bossrenderer.flipX == false)
        {
            this.direction.flipX = true;
        }
        else
        {
            this.direction.flipX = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossactive.IsAttacking == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 들어온 오브젝트의 태그가 Player면
        if (other.CompareTag("Player"))
        {
            Debug.Log("아야");
        }
    }
}
