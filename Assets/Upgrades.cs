using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Upgrades
{
    public int moveSpeedIncrease = 0; // increases ms multiplier by 1
    public int beaconCount = 0; // red, green, blue, gold
    public int compassVisibilityRadius = 0; // visibility and thickness changes according to how close the player is to the exit point
    public int extraTime = 0; // each point adds a minute
}
