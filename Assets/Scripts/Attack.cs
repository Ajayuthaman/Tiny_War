using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    public string tagName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagName)
        {        

            HealthManager health =  other.gameObject.GetComponent<HealthManager>();
            health.TakeDamage(10);
        }
    }
}
