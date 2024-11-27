using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 50;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            GameObject.Destroy(gameObject);
        }
        this.health = amount;
    }
}
