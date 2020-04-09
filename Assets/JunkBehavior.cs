using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkBehavior : MonoBehaviour
{
    public float goal = -10f;
    public Vector3 dv;
    public float d0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += dv * Time.deltaTime;
        this.transform.Rotate(new Vector3(0, 0, d0 * Time.deltaTime));

        if(this.transform.position.x <= goal)
        {
            GameControllerScript.staticInstance.LoseLife();
        }

        if ((this.transform.position.y < -4 && this.dv.y < 0) || (this.transform.position.y > 4 || this.dv.y > 0)) this.dv.y = -this.dv.y;
    }
}
