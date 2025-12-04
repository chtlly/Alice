using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Bossactive : MonoBehaviour
{
    GameObject player;
    public SpriteRenderer bossrenderer;
    public Animator bossanimator;
    public float speed;
    public float tracestart;
    public float traceend;

    [Header("Boss Stats")]
    public float MaxHp = 100000.0f;
    public float CurrentHp;
    public float BasicATK = 100.0f;
    public float ATK;
    public float Defense = 10.0f;

    public float touchDamage = 50.0f;
    private float lastTouchTime = 0.0f;
    public float touchInterval = 0.5f;

    public float attackYRange = 1.0f;

    public bool isAtkBuffed = false;
    public float atkBuffTimer = 0.0f;
    private float currentBuffMultiplier = 1.0f;

    public float MAXATK;
    public bool ATKBuff;
    public float coolATK;

    public float attackDelay = 1.5f;
    private float currentWaitTime = 0.0f;

    public bool IsIdle = true;
    public bool IsTracing = false;
    public bool AttackStart = false;
    public bool IsAttacking = false;
    public bool IsBlood = false;

    SkillHelper skillhelper;
    public GameObject Basic_Effect;
    public GameObject Break_Effect;
    public GameObject Destiny_Effect;
    public GameObject Blood_Circle;

    public GameObject Blood0, Blood1, Blood2, Blood3, Blood4;
    public float blood_cool;
    public float blood_coolm = 0.3f;
    public float blood_level;

    [Header("UI ¿¬°á")]
    public GameObject damageTextPrefab;
    public BossHPBar bossHPBar;

    public void Basic_ATK()
    {
        GameObject be = Instantiate(Basic_Effect);
        if (this.bossrenderer.flipX == true)
            be.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, 0);
        else
            be.transform.position = new Vector3(this.transform.position.x + 1, this.transform.position.y, 0);
    }

    public void Destiny()
    {
        GameObject d = Instantiate(Destiny_Effect);
        d.transform.position = this.transform.position;
    }

    public void Break_Earth()
    {
        GameObject b = Instantiate(Break_Effect);
        b.transform.position = this.transform.position;
    }

    public void Blood_Fountain()
    {
        GameObject c1 = Instantiate(Blood_Circle);
        c1.transform.position = this.transform.position;
    }

    public void BossSkillend()
    {
        IsAttacking = false;
        IsIdle = true;
        currentWaitTime = attackDelay;
        this.bossanimator.SetTrigger("BossIdleTrig");
    }

    void Awake()
    {
        if (MaxHp == 0) MaxHp = 10000.0f;
        CurrentHp = MaxHp;
        ATK = BasicATK;

        MAXATK = 150.0f;
        ATKBuff = false;
        coolATK = 0.0f;
    }

    void Start()
    {
        this.player = GameObject.Find("Player");
        GameObject skillHelperObj = GameObject.Find("SkillHelper");
        if (skillHelperObj != null)
            this.skillhelper = skillHelperObj.GetComponent<SkillHelper>();

        bossrenderer = GetComponent<SpriteRenderer>();
        bossanimator = GetComponent<Animator>();

        if (bossHPBar == null)
            bossHPBar = GetComponentInChildren<BossHPBar>();

        if (bossHPBar != null)
            bossHPBar.UpdateHP(CurrentHp, MaxHp);

        IsIdle = true;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 pb = this.transform.position;
        Vector2 pp = this.player.transform.position;
        Vector2 tracedir = pp - pb;
        float d = tracedir.magnitude;
        float yDiff = Mathf.Abs(tracedir.y);

        if (tracedir.x < 0) bossrenderer.flipX = true;
        else bossrenderer.flipX = false;

        if (isAtkBuffed)
        {
            atkBuffTimer -= Time.deltaTime;
            if (atkBuffTimer <= 0)
            {
                isAtkBuffed = false;
                currentBuffMultiplier = 1.0f;
            }
        }
        ATK = BasicATK * currentBuffMultiplier;

        if (IsIdle)
        {
            if (currentWaitTime > 0)
            {
                currentWaitTime -= Time.deltaTime;
                return;
            }

            if (d >= tracestart || yDiff > attackYRange)
            {
                IsIdle = false;
                IsTracing = true;
                this.bossanimator.SetTrigger("BossWalkTrig");
                this.transform.Translate(tracedir * speed * Time.deltaTime);
            }
            else
            {
                IsIdle = false;
                AttackStart = true;
            }
        }
        else if (IsTracing)
        {
            if (d >= traceend || yDiff > attackYRange)
            {
                this.transform.Translate(tracedir * speed * Time.deltaTime);
            }
            else if (d <= traceend && yDiff <= attackYRange)
            {
                IsTracing = false;
                AttackStart = true;
                this.bossanimator.SetTrigger("BossIdleTrig");
            }
        }
        else if (AttackStart)
        {
            AttackStart = false;
            IsAttacking = true;
            skillhelper.BossSkillSelect();
        }
        else if (IsBlood)
        {
            blood_cool -= Time.deltaTime;
            if (blood_cool <= 0)
            {
                if (blood_level == 0)
                {
                    GameObject b0 = Instantiate(Blood0);
                    b0.transform.position = this.transform.position;
                    blood_cool = blood_coolm; blood_level = 1;
                }
                else if (blood_level == 1)
                {
                    GameObject b1 = Instantiate(Blood1);
                    float Xpos = (float)this.transform.position.x - 0.7f;
                    b1.transform.position = new Vector3(Xpos, this.transform.position.y, 0);
                    blood_cool = blood_coolm; blood_level = 2;
                }
                else if (blood_level == 2)
                {
                    GameObject b2 = Instantiate(Blood2);
                    b2.transform.position = this.transform.position;
                    blood_cool = blood_coolm; blood_level = 3;
                }
                else if (blood_level == 3)
                {
                    GameObject b3 = Instantiate(Blood3);
                    float Xpos = (float)this.transform.position.x + 0.3f;
                    float Ypos = (float)this.transform.position.y + 0.3f;
                    b3.transform.position = new Vector3(Xpos, Ypos, 0);
                    blood_cool = blood_coolm; blood_level = 4;
                }
                else if (blood_level == 4)
                {
                    GameObject b4 = Instantiate(Blood4);
                    b4.transform.position = this.transform.position;
                    blood_cool = 0.2f; blood_level = 5;
                }
                else if (blood_level == 5)
                {
                    IsBlood = false; IsAttacking = false; IsIdle = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastTouchTime + touchInterval)
            {
                Playeractive playerScript = other.GetComponent<Playeractive>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(touchDamage);
                    lastTouchTime = Time.time;
                }
            }
        }
    }

    public void TakeDamage(float rawDamage)
    {
        float damageReduction = (rawDamage * Defense) / (100f + Defense);
        float finalDamage = rawDamage - damageReduction;
        if (finalDamage < 1f) finalDamage = 1f;
        finalDamage = Mathf.Floor(finalDamage);

        CurrentHp -= finalDamage;

        if (bossHPBar != null)
        {
            bossHPBar.UpdateHP(CurrentHp, MaxHp);
        }

        if (damageTextPrefab != null)
        {
            GameObject hud = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            hud.transform.position += new Vector3(0, 1.0f, 0.0f);

            FloatingText ft = hud.GetComponent<FloatingText>();
            ft.SetDamage(finalDamage);

            if (ft.GetComponent<TextMeshPro>() != null)
            {
                ft.GetComponent<TextMeshPro>().color = Color.red;
                ft.GetComponent<TextMeshPro>().fontSize = 5;
            }
        }

        if (CurrentHp <= 0)
        {
            IsAttacking = false;
            IsBlood = false;
            Destroy(gameObject);
            SceneManager.LoadScene("Boss_Ending");
        }
    }

    public void ApplyAtkBuff(float multiplier, float duration)
    {
        isAtkBuffed = true;
        currentBuffMultiplier = multiplier;
        atkBuffTimer = duration;
    }

    public void Heal(float amount)
    {
        CurrentHp += amount;
        if (CurrentHp > MaxHp) CurrentHp = MaxHp;

        if (bossHPBar != null)
        {
            bossHPBar.UpdateHP(CurrentHp, MaxHp);
        }
    }
}