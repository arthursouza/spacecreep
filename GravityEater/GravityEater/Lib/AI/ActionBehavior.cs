using System;

namespace GravityEater.Lib.AI
{
    public enum ActionType
    {
        Attack,
        Skill,
        Behavior
    }

    public class ActionBehavior
    {
        public ActionBehavior()
        {
            Condition = new Condition();
            ActionType = ActionType.Skill;
        }

        public int Id { get; set; }

        public Condition Condition { get; set; }
        public ActionType ActionType { get; set; }
        public BehaviorType NewBehavior { get; set; }
        public int SkillId { get; set; }
    }
}