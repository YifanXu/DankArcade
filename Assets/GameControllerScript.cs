using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript staticInstance;

    public int life = 3;
    public float persistTime = 10f;
    public float minJunkSpawnTime = 0.3f;
    public float minDroneSpawnTime = 10f;
    public GameObject JunkPrefab;
    public GameObject DronePrefab;
    public bool paused = false;

    public float timer { get; private set; }

    private Queue<ProjectileBehavior> queue;
    private float lastJunkSpawn = 0f;
    private float lastDroneSpawn = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (staticInstance != null) Destroy(staticInstance.gameObject);
        staticInstance = this;
        queue = new Queue<ProjectileBehavior>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Clear old projectiles
        if(!paused) timer += Time.deltaTime;
        while(queue.Count > 0 && (queue.Peek() == null || queue.Peek().destructTime <= timer))
        {
            var obj = queue.Dequeue();
            if(obj != null) Destroy(obj.gameObject);
        }

        if(timer - lastJunkSpawn > minJunkSpawnTime)
        {
            lastJunkSpawn = timer;
            Spawn(JunkPrefab, 9f, -3, 3, -1f - timer / 20f, 1f, 1f, 150f);
        }

        if (timer - lastDroneSpawn > minDroneSpawnTime)
        {
            lastDroneSpawn = timer;
            Spawn(DronePrefab, 9f, -3, 3, -0.5f - timer / 50f, 0.2f, 0.5f, 0f).transform.Rotate(new Vector3(0,0,180));
        }
    }

    public void Register(ProjectileBehavior projectile)
    {
        projectile.destructTime = timer + persistTime;
        queue.Enqueue(projectile);
    }

    public void LoseLife()
    {
        if (life > 0)
        {
            life--;
            UIScript.staticInstance.ChangeLifeCount(life);
        }
        if (life <= 0) EndGame();
    }

    public void EndGame()
    {
        paused = true;
        UIScript.staticInstance.GameOverPanel.SetActive(true);
    }

    private GameObject Spawn (GameObject prefab, float startX, float minStartY, float maxStartY, float basedX, float varydX, float varydY, float varyd0)
    {
        var newObj = Instantiate(prefab, new Vector3(9, minStartY+ (maxStartY - minStartY) * Random.value), Quaternion.identity);
        newObj.GetComponent<JunkBehavior>().dv = new Vector3(basedX + 2 * varydX * (Random.value - 0.5f), 2 * varydY * (Random.value - 0.5f), 0);
        newObj.GetComponent<JunkBehavior>().d0 = varyd0 * (Random.value - 0.5f) * 2;
        return newObj;
    }
}
