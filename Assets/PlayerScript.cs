﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //private readonly Bounds screenBounds = new Bounds(new Vector3(), new Vector3(16,8));
    private const float leftBound = -8;
    private const float rightBound = 8;
    private const float botBound = -4;
    private const float topBound = 4;

    public float speed = 10f;
    public float standardProjCD = 0.5f;
    public GameObject projectileTemplate;
    public GameObject droneTemplate;
    public Vector3 projectileDV;
    public Vector3 projectileOffset;
    public Color projectileColor;
    public float droneCharge = 0f;
    public float maxDroneCharge = 4f;
    public float droneChargePerSecond = 0.1f;
    public float droneDeployTime = 1f;

    private Collider2D collider;
    private Vector3 colliderSize;
    public float lastShot = -1000f;

    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<Collider2D>();
        colliderSize = collider.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement 
        Vector3 movementDelta = new Vector3();
        if(Input.GetKey(KeyCode.UpArrow))
        {
            movementDelta.y += speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDelta.y -= speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDelta.x -= speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDelta.x += speed;
        }
        movementDelta *= Time.deltaTime;
        Vector3 endPos = this.transform.position + movementDelta;
        endPos.x = Mathf.Clamp(endPos.x, leftBound - collider.offset.x + collider.bounds.size.x / 2 , rightBound - collider.offset.x - collider.bounds.size.x / 2);
        endPos.y = Mathf.Clamp(endPos.y, botBound - collider.offset.y + collider.bounds.size.y / 2, topBound - collider.offset.y - collider.bounds.size.y / 2);
        if(!GameControllerScript.staticInstance.paused) this.transform.position = endPos;

        // Shoot Stuff
        if(Input.GetKey(KeyCode.Space))
        {
            float currentTime = GameControllerScript.staticInstance.timer;
            if(currentTime - lastShot > standardProjCD)
            {
                Debug.Log("Pew!");
                lastShot = currentTime;
                //Shoot Projectile
                var newProjectile = Instantiate(projectileTemplate, this.transform.position + this.projectileOffset, Quaternion.identity);
                newProjectile.GetComponent<ProjectileBehavior>().dv = this.projectileDV + movementDelta;
                newProjectile.GetComponent<ProjectileBehavior>().parentCollider = this.gameObject;
                newProjectile.GetComponent<SpriteRenderer>().color = this.projectileColor;
            }
        }

        //Charge Drone
        if (!GameControllerScript.staticInstance.paused && this.droneCharge < this.maxDroneCharge)
        {
            this.droneCharge += this.droneChargePerSecond * Time.deltaTime;
            UIScript.staticInstance.ChangeDroneCount(droneCharge);
        }

        // Deploy Drones
        if(Input.GetKeyDown(KeyCode.X))
        {
            if(this.droneCharge > 1f)
            {
                droneCharge -= 1f;
                UIScript.staticInstance.ChangeDroneCount(droneCharge);
                var newDrone = Instantiate(droneTemplate, this.transform.position, Quaternion.identity);
                newDrone.GetComponent<DroneScript>().deployTime = GameControllerScript.staticInstance.timer + droneDeployTime;
            }
        }
    }

    public void Die()
    {
        GameControllerScript.staticInstance.EndGame();
    }
}
