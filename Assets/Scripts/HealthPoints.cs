using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    public float hp;


    public void takeDamage(float damageVal) 
    {
        hp -= damageVal;
    }
}
