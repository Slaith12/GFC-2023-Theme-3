using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Crafting
{
    public class ResourceCluster : MonoBehaviour
    {

        public int resourceAmount;
        public SKGG.Combat.Health h;


        void Start()
        {
            h = GetComponent<SKGG.Combat.Health>();
        }

        // Update is called once per frame
        void Update()
        {
            this.enabled = !h.dead;
        }

        private void OnDisable()
        {

        }



    }

}
