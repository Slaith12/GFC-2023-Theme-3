using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    /*keeps track of the resources that all players have.
    Collective pool for all players rather than individual players having individual resources
    so getting resources for the betterment of group is incentivized
    */

    //main collected resources
    public int stoneCount = 0;
    public int gemCount = 0;
    public int metalCount = 0;
    public int gearCount = 0;
    public int slimeCount = 0;
    public int sunCoreCount = 0;
    public int moonCoreCount = 0;


    public int gunPartCount = 0;
    public int statuetteShardCount = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
