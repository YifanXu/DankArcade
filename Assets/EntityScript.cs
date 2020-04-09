using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityScript : MonoBehaviour
{
    public float hp;
    public float hpDecay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameControllerScript.staticInstance.paused) Damage(hpDecay * Time.deltaTime);
    }

    public void Damage (float damageHP)
    {
        this.hp -= damageHP;
        if(this.hp <= 0f)
        {
            // Player needs special screen
            if(this.GetComponent<PlayerScript>() != null)
            {
                this.GetComponent<PlayerScript>().Die();
            }
            else if(this.GetComponent<JunkBehavior>() != null)
            {
                UIScript.staticInstance.AddJunkKill();
            }
            Destroy(this.gameObject);
        }
    }
}
