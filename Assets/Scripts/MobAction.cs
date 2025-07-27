using Mobs;
using System.Collections.Generic;

public enum ActionType
{
    SkipTurn,
    Defense,
    Attack,
    ActiveSkill,
    PassiveSkill
}

public class MobAction
{
    public ActionType MobActionType { get; set; }
    public Mob MobInstance { get; set; }
    public Mob TargetInstance { get; set; }
    public List<Mob> Targets { get; set; } = new ();
    
    public MobAction()
    {
        
    }
    
    public MobAction(ActionType actionType, Mob mobInstance, Mob targetInstances)
    {
        MobActionType = actionType;
        MobInstance = mobInstance;
        TargetInstance = targetInstances;
    }
}
