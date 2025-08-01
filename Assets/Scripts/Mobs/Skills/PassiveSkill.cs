namespace Mobs.Skills
{
    public abstract class PassiveSkill : Skill
    {
        public int Duration { get; protected set; }

        public virtual void Cleanup()
        {
            
        }
    }
}
