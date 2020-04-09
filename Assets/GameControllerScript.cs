using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript staticInstance;

    public int life = 3;
    public float persistTime = 10f;
    public float minJunkSpawnTime = 0.3f;
    public GameObject JunkPrefab;
    public bool paused = false;

    public float timer { get; private set; }

    private Queue<ProjectileBehavior> queue;
    private float lastJunkSpawn = 0.3f;

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
            var newObj = Instantiate(JunkPrefab, new Vector3(9, Random.value * 8 - 4, 0), Quaternion.identity);
            newObj.GetComponent<JunkBehavior>().dv = new Vector3(-Random.value - timer / 20f, Random.value * 0.6f - 0.3f, 0);
            newObj.GetComponent<JunkBehavior>().d0 = Random.value * 150f - 50f;
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
}
