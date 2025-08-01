using Mobs;
using UnityEngine;

public class SEEarth : SEElemental
{
    public SEEarth()
    {
        IsNegative = true;
    }
    

    public override void ApplyEffect(Mob parentMob)
    {
        parentMob.UI.ShowText("ОПУТАН ЛОЗАМИ!", Color.blue);
    }
}
