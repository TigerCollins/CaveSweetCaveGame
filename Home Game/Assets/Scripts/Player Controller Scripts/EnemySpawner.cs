using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float minOffset;
    public float maxOffset;
    public float minDistance;
    public float maxDistance;

    public float spawnIntervalMin;
    public float spawnIntervalMax;
    public float spawnTimer;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.C))
        {
            SpawnEnemy();
        }

        UpdateSpawnTimer();
	}

    void UpdateSpawnTimer()
    {
        if (PlayerController.instance.dayNightControl.currentTime < 0.25f || PlayerController.instance.dayNightControl.currentTime > 0.75f)
        {
            if(PlayerController.instance.inCave == false)
            {
                if (spawnTimer > 0)
                {
                    spawnTimer -= Time.deltaTime;

                    if (spawnTimer < 0)
                    {
                        SpawnEnemy();
                        spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
                    }
                }
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(EnemyPrefab);

        //Debug.Log(this.gameObject.transform.position);
        newEnemy.transform.position = this.gameObject.transform.position - (PlayerController.instance.firstPersonController.HorizontalTurntable.transform.forward * (Random.Range(minDistance, maxDistance)));

        Vector3 enemyPositionOffset = Vector3.zero;

        enemyPositionOffset.x = Random.Range(minOffset, maxOffset);
        if(Random.Range((int)0, (int)2) > 0)
        {
            enemyPositionOffset.x *= -1;
        }
        //enemyPositionOffset.y = Random.Range(minmumDistance, maxDistance);
        enemyPositionOffset.z = Random.Range(minOffset, maxOffset);
        if (Random.Range((int)0, (int)2) > 0)
        {
            enemyPositionOffset.z *= -1;
        }

        newEnemy.transform.position += enemyPositionOffset;


        Debug.Log("enemy Spawned");
    }
}
