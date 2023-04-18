using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SKGG.Combat
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float bulletForce;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] float destroyTimer;

        // Start is called before the first frame update
        void Start()
        {
            rb.AddForce(bulletForce * new Vector2(Input.mousePosition.x - transform.position.x,
                Input.mousePosition.y - transform.position.y).normalized);
            Destroy(this.gameObject, destroyTimer);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.IsTouchingLayers(7))
                Destroy(this.gameObject);
        }
        
    }

}
