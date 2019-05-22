using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int lastUnlockedLevel = 1;
    public int unspentUpgradePoints = 0;
    public long lastAdViewTimeTicks = 0;
    public Upgrades permanentUpgrades = new Upgrades();

    public DateTime LastAdViewTime
    {
        get
        {
            return new DateTime(lastAdViewTimeTicks);
        }
        set
        {
            lastAdViewTimeTicks = value == null ? 0 : value.Ticks;
        }
    }
}
