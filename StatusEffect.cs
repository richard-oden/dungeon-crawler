using System.Collections.Generic;
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
        public bool UndoWhenFinished {get; private set;}
        public bool HasCoolDown {get; private set;}
        public string TargetedAbilityScore {get; private set;}
        public StatusEffect(string name, int duration, string targetProp, int valueChange, string targetedAbilityScore = null, bool recurring = false, bool undoWhenFinished = false, bool hasCoolDown = false)
        {
            Name = name;
            Duration = duration;
            TargetProp = targetProp;
            ValueChange = valueChange;
            Recurring = recurring;
            UndoWhenFinished = undoWhenFinished;
            HasCoolDown = hasCoolDown;
            TargetedAbilityScore = targetedAbilityScore;
        }
        public void DecrementDuration()
        {
            if (Duration > 0) Duration -= 1;
        }

        public static List<StatusEffect> ParseStatusEffects(string source)
        {
            var sourceArray = source.Split(' ');
            var output = new List<StatusEffect>();
            foreach(var se in sourceArray)
            {
                PropertyInfo piStatusEffect = typeof(StatusEffects).GetProperty(se);
                if (piStatusEffect.PropertyType == typeof(StatusEffect))
                {
                    output.Add((StatusEffect)piStatusEffect.GetValue(null, null));
                }
                else
                {
                    throw new System.Exception($"{se} is not a valid statusEffect!");
                }
            }
            return output;
        }
    }
}