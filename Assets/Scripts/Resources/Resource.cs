using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] GameObject resourceType;
    public int resourceAmount;
    

    // Start is called before the first frame update

    ResourceManager r;
    HealthPoints healthPoints;

    void Start()
    {
        r = GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoints.hp <= 0) 
        {
            this.enabled = false;
        }
    }

    private void OnDisable()
    {
        Instantiate(resourceType, this.transform);    
    }
}
