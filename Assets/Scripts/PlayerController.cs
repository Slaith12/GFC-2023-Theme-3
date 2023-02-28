using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs inputs;

    void Awake()
    {
        inputs = new PlayerInputs(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
