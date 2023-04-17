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


        // Start is called before the first frame update
        void Start()
        {
            timerValue = timer;
        }

        // Update is called once per frame
        void Update()
        {
            timerValue -= Time.deltaTime;

            if (timerValue <= 0 && Input.GetMouseButtonDown(1))
                shoot();

        }

        void shoot()
        {
            timerValue = timer;

            //CHANGE TO SPAWN FOR NETCODE
            Instantiate(bullet,barrel.transform.position,Quaternion.identity);

            
        }


        private void OnMouseDown()
        {
            rb.gravityScale = 0;
            
            transform.Rotate(0, 0, Mathf.Clamp(Mathf.Atan(Input.mousePosition.y - transform.position.y /
                Input.mousePosition.x - transform.position.x),-.5f * Mathf.PI, .5f * Mathf.PI));

        }

        private void OnMouseUp()
        {
            rb.gravityScale = 1;
        }


    }

}