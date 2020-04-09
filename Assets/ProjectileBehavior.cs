using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Vector3 dv;
    public float payload = 1f;
    public float destructTime;
    public Color color;
    public GameObject parentCollider;

    // Start is called before the first frame update
    void Start()
    {
        if(GameControllerScript.staticInstance != null) GameControllerScript.staticInstance.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += dv * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == parentCollider) return;
        var targetEntityScript = collision.gameObject.GetComponent<EntityScript>();
        if(targetEntityScript != null)
        {
            targetEntityScript.Damage(payload);
        }
        Destroy(this.gameObject);
    }
}
