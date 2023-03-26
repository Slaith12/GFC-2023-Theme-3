using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//if anything needs to be done when the game first starts up, do it in this script
public class Initializer : MonoBehaviour
{
    void Start()
    {
        //right now initialization is only here to instantiate the network manager, so it can finish immediately
        FinishInitialization();
    }

    public void FinishInitialization()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
