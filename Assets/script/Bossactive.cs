using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Bossactive : MonoBehaviour
{
    GameObject player;
    public float speed;
    public float tracestart;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.player = GameObject.Find("Player");
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
        Vector2 tracedir = pp - pb;
        float d = tracedir.magnitude;
        if (d >= tracestart)
        {
            transform.Translate(tracedir * speed * Time.deltaTime);
        }
        else
        {
        }
        
    }
}
