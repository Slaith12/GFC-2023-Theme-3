using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SKGG.Crafting
{
    public class Crafting : MonoBehaviour
    {
        public List<Resource> ingredients;

        public Dictionary<List<Resource>, GameObject> weapons;

        [SerializeField] GameObject crabHammer;
        [SerializeField] List<Resource> crabIngr;

        // Start is called before the first frame update
        void Start()
        {
            weapons.Add(crabIngr, crabHammer);   
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Resource>() != null) 
            {
                ingredients.Add(collision.GetComponent<Resource>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Resource>() != null)
            {
                ingredients.Remove(collision.GetComponent<Resource>());
            }
        }

        private void OnMouseDown()
        {
            GameObject crafted = weapons[ingredients];

            //CHANGE TO SPAWN
            Instantiate(crafted);
            foreach (var ingr in ingredients)
            {
                Destroy(ingr);
            }
        }
    }

}
