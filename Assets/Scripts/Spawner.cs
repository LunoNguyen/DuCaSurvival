using Unity.Cinemachine;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnBuffer = 2f;

    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Player player;

    private float spawnTimer = 0f;

    void Awake()
    {
        if (cinemachineCamera == null)
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        if (player == null)
            player = FindFirstObjectByType<Player>();

        if (cinemachineCamera != null && player != null)
        {
            cinemachineCamera.Follow = player.transform;
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnObstacle();
            spawnTimer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;
        if (cinemachineCamera == null) return;

        Vector2 spawnPos = GetRandomPositionOutsideCameraBounds();
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomIndex];

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        }
    }

    private Vector2 GetRandomPositionOutsideCameraBounds()
    {
        Camera mainCam = Camera.main;
        Vector3 camCenter = cinemachineCamera.transform.position;

        // Calculate visible half-bounds in world units
        float halfHeight = cinemachineCamera.Lens.OrthographicSize;
        float aspect = mainCam != null ? mainCam.aspect : (16f / 9f);
        float halfWidth = halfHeight * aspect;

        // Choose one of the 4 borders: 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
        int edge = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        switch (edge)
        {
            case 0: // Top
                spawnPos.x = Random.Range(camCenter.x - halfWidth - spawnBuffer, camCenter.x + halfWidth + spawnBuffer);
                spawnPos.y = camCenter.y + halfHeight + spawnBuffer;
                break;

            case 1: // Bottom
                spawnPos.x = Random.Range(camCenter.x - halfWidth - spawnBuffer, camCenter.x + halfWidth + spawnBuffer);
                spawnPos.y = camCenter.y - halfHeight - spawnBuffer;
                break;

            case 2: // Left
                spawnPos.x = camCenter.x - halfWidth - spawnBuffer;
                spawnPos.y = Random.Range(camCenter.y - halfHeight - spawnBuffer, camCenter.y + halfHeight + spawnBuffer);
                break;

            case 3: // Right
                spawnPos.x = camCenter.x + halfWidth - spawnBuffer;
                spawnPos.y = Random.Range(camCenter.y - halfHeight - spawnBuffer, camCenter.y + halfHeight + spawnBuffer);
                break;
        }

        return spawnPos;
    }
}