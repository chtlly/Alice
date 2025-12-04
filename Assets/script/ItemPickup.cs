using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Gold, Exp }

    [Header("아이템 설정")]
    public ItemType type;
    public int amount = 10;

    [Header("자석 효과")]
    public float magnetSpeed = 5.0f;
    public float magnetRange = 3.0f; // [추가] 이 거리 안에 있어야 빨려옴

    private Transform playerTrans;
    private bool isMagnetReady = false; // 0.5초 대기 후 true가 됨

    void Start()
    {
        // 0.5초 뒤부터 자석 기능 활성화 준비 (생성되자마자 먹어지는 것 방지)
        Invoke("EnableMagnet", 0.5f);
        Destroy(gameObject, 10.0f);
    }

    void EnableMagnet()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTrans = player.transform;
            isMagnetReady = true; // 이제부터 거리 체크 시작
        }
    }

    void Update()
    {
        // 1. 준비가 되었고 플레이어가 존재할 때
        if (isMagnetReady && playerTrans != null)
        {
            // 2. 플레이어와의 거리 계산
            float distance = Vector3.Distance(transform.position, playerTrans.position);

            // 3. 거리가 설정한 범위(magnetRange)보다 작으면 빨려감
            if (distance <= magnetRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTrans.position, magnetSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Playeractive player = other.GetComponent<Playeractive>();
            if (player != null)
            {
                switch (type)
                {
                    case ItemType.Gold:
                        player.Money += amount;
                        Debug.Log($"[ITEM] 골드 획득! (+{amount})");
                        break;
                    case ItemType.Exp:
                        player.GainExp(amount);
                        Debug.Log($"[ITEM] 경험치 획득! (+{amount})");
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}