using UnityEngine;

public class Destiny : MonoBehaviour
{
    Bossactive bossactive;
    GameObject player;
    public float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.bossactive = GameObject.Find("Rabbit_Ssi").GetComponent<Bossactive>();
        this.player = GameObject.Find("player");
        time = 2.0f;
        bossactive.ATKBuff = true;
        bossactive.coolATK = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
            bossactive.BossSkillend();
        }
    }
}
