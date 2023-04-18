using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Combat
{
    public class StandardGun : MonoBehaviour
    {

        [SerializeField] float timer;
        float timerValue;
        [SerializeField] GameObject bullet;
        Rigidbody2D rb;
        [SerializeField] Transform barrel;
        [SerializeField] SKGG.Physics.GrabbableObject g;
        [SerializeField] SKGG.Control.PlayerController p;

        // Start is called before the first frame update
        void Start()
        {
            timerValue = timer;

            g = GetComponent<SKGG.Physics.GrabbableObject>();
            p = GetComponent<SKGG.Control.PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            timerValue -= Time.deltaTime;
            
            if (true) 
            {
                /*transform.Rotate(0, 0, Mathf.Clamp(Mathf.Atan(Input.mousePosition.y - transform.position.y/
                    Input.mousePosition.x - transform.position.x), 0, Mathf.PI));*/


                if (timerValue <= 0 && Input.GetMouseButtonDown(1))
                    shoot();
            }

        }

        void shoot()
        {
            timerValue = timer;

            //CHANGE TO SPAWN FOR NETCODE
            Instantiate(bullet,barrel.transform.position,Quaternion.identity);

            
        }



    }

}