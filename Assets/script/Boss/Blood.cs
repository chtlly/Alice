using UnityEngine;
using UnityEngine.SceneManagement;

public class Blood : MonoBehaviour
{
    Bossactive boss;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.boss = GameObject.Find("Rabbit_Ssi").GetComponent<Bossactive>();
        this.player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.IsAttacking == false)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 들어온 오브젝트의 태그가 Player면
        if (other.CompareTag("Player"))
        {
            boss.hp -= 10;
        }
    }
}
