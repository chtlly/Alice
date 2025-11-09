using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Bossactive : MonoBehaviour
{
    GameObject player;
    SpriteRenderer bossrenderer;
    Animator bossanimator;
    public float speed; //속도
    public float tracestart; //추적 시작 거리
    public float face; //방향

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.player = GameObject.Find("Player");
        bossrenderer = GetComponent<SpriteRenderer>();
        bossanimator = GetComponent<Animator>();
        face = 1;
    }

    /*public void Selector()
    {
    }*/

    public void BasicAttack()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pb = transform.position;//보스의 위치
        Vector2 pp = this.player.transform.position;//플레이어의 위치
        Vector2 tracedir = pp - pb; //보스에서 플레이어를 향하는 벡터
        if (tracedir.x < 0) //스프라이트 뒤집기
        {
            bossrenderer.flipX = true;
        }
        else
        {
            bossrenderer.flipX = false;
        }
        float d = tracedir.magnitude;
        if (d >= tracestart) //추격 시작
        {
            this.bossanimator.SetBool("BossStopTrig", false);
            this.bossanimator.SetBool("BossWalkTrig", true);

            transform.Translate(tracedir * speed * Time.deltaTime);
        }
        else
        {
            this.bossanimator.SetBool("BossWalkTrig", false);
            this.bossanimator.SetBool("BossStopTrig", true);
        }
        
    }
}
