using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

//this isn't the best method for movement but it's the easiest
//if i have time i'll make a better method that properly uses PositionSync and RotationSync
public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
