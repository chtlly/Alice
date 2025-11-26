using UnityEngine;

public class Blood_Blossom : MonoBehaviour
{
    Bossactive bossactive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.bossactive = GameObject.Find("Rabbit_Ssi").GetComponent<Bossactive>();
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
            Debug.Log("플레이어의 체력 감소"); //최대체력의 15%감소 + 보스는 데미지입힌 만큼 회복
        }
    }
}
