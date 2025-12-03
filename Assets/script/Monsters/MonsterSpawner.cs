using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab; // 몬스터 프리팹
    public float spawnInterval = 5f; // 웨이브 간격
    public int maxMonsters = 20;     // 최대 수
    public int spawnCountPerWave = 3; // 한 번에 생성할 수

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin = new Vector2(-10f, -5f);
    public Vector2 spawnAreaMax = new Vector2(10f, 5f);

    private int currentCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            for (int i = 0; i < spawnCountPerWave; i++)
            {
                if (currentCount < maxMonsters)
                {
                    SpawnMonster();
                }
                else
                {
                    break;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null) return;

        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        GameObject newMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        newMonster.name = monsterPrefab.name + "_" + currentCount;

        //몬스터에게 스탯 스크립트가 있다면 스포너 정보를 넘겨줌
        MonsterStats stats = newMonster.GetComponent<MonsterStats>();
        if (stats != null)
        {
            stats.Setup(this);
        }

        currentCount++;
    }

    public void DecreaseCount()
    {
        currentCount--;
        if (currentCount < 0) currentCount = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2, (spawnAreaMin.y + spawnAreaMax.y) / 2, 0);
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0);
        Gizmos.DrawWireCube(center, size);
    }
}