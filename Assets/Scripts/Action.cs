public enum ActionType
{
    SkipTurn,
    Defense,
    Attack1,
    Attack2
}


public class Action
{
    public ActionType MobAction { get; set; }
    public Mob MobInstance { get; set; }
    public Mob TargetInstance { get; set; }
}
