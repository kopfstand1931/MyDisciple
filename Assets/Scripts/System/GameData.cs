using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    // 'Style', 'Model' of Battle Model, and '3 Stats'

    // 'Style' is the personality of the Battle Model.
    // 0 : 'Melee'
    // 1 : 'Ranged'
    // 2 : 'Balanced'

    // 'Model Level' is the level of the Battle Model.
    // 1 : 'Low'
    // 2 : 'Medium'
    // 3 : 'High'
    // Initailize as 0 for checking if the player has created a model through New Start properly.

    public int currentModelStyle = 0;
    public int currentModelLevel = 0;

    public int statOFF = 1;
    public int statDFF = 1;
    public int statSPD = 1;
    
    public string name;
    
    public int turnElapsed = 0;

    // 'EXP' is the experience for Model Level Up.
    // if Model Level is 1, needed EXP for Level Up is 2.
    // if Model Level is 2, needed EXP for Level Up is 4.
    public int currentEXP = 0;
    
}
