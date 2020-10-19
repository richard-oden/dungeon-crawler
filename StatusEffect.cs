using System.Reflection;

namespace DungeonCrawler
{
    public class StatusEffect
    {
        public string Name {get; private set;}
        public int Duration {get; private set;}
        public string TargetProp {get; private set;}
        public int ValueChange {get; private set;}
        public bool Recurring {get; private set;}
        public bool HasCoolDown {get; private set;}
        public StatusEffect(string name, int duration, string targetProp, int valueChange, bool recurring, bool hasCoolDown)
        {
            Name = name;
            Duration = duration;
            TargetProp = targetProp;
            ValueChange = valueChange;
            Recurring = recurring;
            HasCoolDown = hasCoolDown;
        }
        public void DecrementDuration()
        {
            if (Duration > 0) Duration -= 1;
        }
    }
}