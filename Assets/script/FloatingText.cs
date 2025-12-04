using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float destroyTime = 1.0f;
    public TextMeshPro textMesh;

    void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    // 데미지용 (숫자)
    public void SetDamage(float damage)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            textMesh.text = damage.ToString();
            textMesh.color = Color.red;
            textMesh.fontSize = 4;
        }
    }

    // [추가됨] 레벨업용 (글자 + 색상)
    public void SetText(string message, Color color)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            textMesh.text = message;
            textMesh.color = color;
            textMesh.fontSize = 5; // 좀 더 크게
        }
    }
}