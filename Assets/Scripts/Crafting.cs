using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SKGG.Combat
{
    public class Crafting : MonoBehaviour
    {
        public Resource[] ingredients;

        public Dictionary<Resource[], Weapon> weapons;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            ingredients = GetComponent<Resource[]>();
        }

        private void OnMouseDown()
        {
            Weapon crafted = weapons[ingredients];

            Instantiate(crafted);
            foreach (var ingr in ingredients)
            {
                Destroy(ingr);
            }
        }
    }

}
