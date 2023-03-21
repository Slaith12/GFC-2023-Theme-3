using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCluster : MonoBehaviour
{
    public Transform[] resourceSpawnPositions;
    public GameObject[] resources;
    float[] ResourceTimers;
    [SerializeField] float maxTimer;
    [SerializeField] float minTimer;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < resourceSpawnPositions.Length; i++) 
        {
            Instantiate(resources[i], resourceSpawnPositions[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        resourceTimerLogic();   
    }

    void resourceTimerLogic() 
    {
        for (int i = 0; i < ResourceTimers.Length; i++) 
        {
            ResourceTimers[i] -= Time.fixedDeltaTime;

            if (ResourceTimers[i] <= 0 && !resources[i].activeInHierarchy)
            {
                ResourceTimers[i] = Random.Range(minTimer, maxTimer);
                resources[i].SetActive(true);
            }
        }
    }



}
