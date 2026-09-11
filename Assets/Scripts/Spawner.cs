using System.Threading;
using Unity.Cinemachine;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Spawn settings
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private float spawnPadding = 8f;
    private float spawnTimer = 0f;

    CinemachineCamera cinemachineCamera;
    private Player player;

    //Awake is called when the script instance is being loaded
    void Awake()
    {
        cinemachineCamera = FindObjectOfType<CinemachineCamera>();
        player = FindObjectOfType<Player>();
        InvokeRepeating("SpawnObstacle", 0f, spawnInterval);
        cinemachineCamera.Follow = player.transform;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnInterval)
        {
            SpawnObstacle();
            spawnTimer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (player == null)
        {
            return; // Exit if player is not found
        }
        Vector3 cameraPos = GetRandomPositionOutsideCameraBounds();

        Vector3 spawnPosition = new Vector3(cameraPos.x, cameraPos.y, 0f);
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstaclePrefab = obstaclePrefabs[randomIndex];
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    private Vector2 GetRandomPositionOutsideCameraBounds()
    {
        Vector3 cameraPosition = cinemachineCamera.transform.position;
        float cameraHeight = cinemachineCamera.Lens.OrthographicSize * 2f;
        float cameraWidth = cameraHeight * cinemachineCamera.Lens.Aspect;
        float spawnX = Random.Range(cameraPosition.x - cameraWidth, cameraPosition.x + cameraWidth);
        float spawnY = Random.Range(cameraPosition.y - cameraHeight, cameraPosition.y + cameraHeight);

        //Padding to ensure the spawn position is outside the camera bounds
        if (spawnX > cameraPosition.x - spawnPadding && spawnX < cameraPosition.x + spawnPadding)
        {
            spawnX = (spawnX < cameraPosition.x) ? cameraPosition.x - spawnPadding : cameraPosition.x + spawnPadding;
        }
        if(spawnY > cameraPosition.y - spawnPadding && spawnY < cameraPosition.y + spawnPadding)
        {
            spawnY = (spawnY < cameraPosition.y) ? cameraPosition.y - spawnPadding : cameraPosition.y + spawnPadding;
        }

        return new Vector3(spawnX, spawnY, 0f);
    }
}
