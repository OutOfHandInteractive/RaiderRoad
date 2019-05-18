using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiscHittable : MonoBehaviour
{
    //MisHittable is a generic object a player can attack and it will do a thing (animation, interact with other script, destroy and drop walls, etc)

    public abstract void RegisterHit(); //generic function for player to call

}
