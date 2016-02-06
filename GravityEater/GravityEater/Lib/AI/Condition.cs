using System;

namespace GravityEater.Lib.AI
{
    public enum ConditionType
    {
        HealthLower,
        HealthLowerPC,
        HealthHigher,
        HealthHigherPC,
        None
    }

    public enum ConditionTarget
    {
        Self,
        Friend,
        Enemy
    }

    public class Condition
    {
        public int ActionId { get; set; }

        public string Label
        {
            get { return Enum.GetName(typeof (ConditionType), Type); }
        }

        public float Value { get; set; }
        public ConditionType Type { get; set; }
        public ConditionTarget Target { get; set; }
    }
}