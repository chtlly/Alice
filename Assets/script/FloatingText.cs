using UnityEngine;
using TMPro; // TMP 필수

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float destroyTime = 1.0f;
    TextMeshPro textMesh;

    void Awake() // Start보다 빨리 실행됨
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        Destroy(gameObject, destroyTime); // 시간 지나면 삭제
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); // 위로 둥둥
    }

    public void SetDamage(float damage)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
        textMesh.text = damage.ToString(); // 숫자를 글자로 변환
    }
}