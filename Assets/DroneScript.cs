using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    public Vector3 dv;
    public Vector3 projectileOffset = new Vector3();
    public Vector3 projectileDV;
    public Color projectileColor;
    public float shotInterval = 0.5f;
    public float deployTime;
    public GameObject projectilePrefab;

    public bool deployed = false;

    private float lastShot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = GameControllerScript.staticInstance.timer;
        if (!deployed && deployTime <= currentTime)
        {
            this.deployed = true;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (deployed && currentTime - lastShot > shotInterval)
        {
            lastShot = currentTime;
            //Shoot Projectile
            var newProjectile = Instantiate(projectilePrefab, this.transform.position + this.projectileOffset, Quaternion.identity);
            newProjectile.GetComponent<ProjectileBehavior>().dv = this.projectileDV;
            newProjectile.GetComponent<ProjectileBehavior>().parentCollider = this.gameObject;
            newProjectile.GetComponent<SpriteRenderer>().color = this.projectileColor;
        }
    }
}
