using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is here so that if we want anything other than the player to grab objects it will still work well.
public interface IGrabber
{
    public Vector2 targetLocation { get; }
    public float rotationOffset { get; }
    public float followStrength { get; }
    public float torqueStrength { get; }
    public float lookAheadTime { get; }
}
