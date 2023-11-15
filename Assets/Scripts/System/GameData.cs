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

    public int currentModelStyle = 0;
    public int currentModelLevel = 0;
    public int statOFF = 1;
    public int statDFF = 1;
    public int statSPD = 1;
    public string name;
    
}
