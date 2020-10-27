using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public class SentientNpc : SentientCreature, INpc
    {
        public Aggression Aggression {get; protected set;}
        public List<Entity> KnownEntities {get; protected set;}
        public List<Item> KnownItems {get; protected set;}

        public SentientNpc(string name, int level, char gender, Race race, Caste caste, int team, Aggression aggression, int[] abilityScoreValues = null, MapPoint location = null) : 
            base(name, level, gender, race, caste, team, abilityScoreValues, location)
        {
            Aggression = aggression;
        }

        private void addToKnown(List<IMappable> knownObjects)
        {
            foreach (var obj in knownObjects)
            {
                if (obj is Entity) KnownEntities.Add(obj as Entity);
                else if (obj is Item) KnownItems.Add(obj as Item);
            }
        }

        public override bool Search()
        {
            addToKnown(BaseSearch());
            return true;
        }

        public void AttackedBy(Entity enemy)
        {
            if (KnownEntities.Contains(enemy))
            {
                KnownEntities.MoveElement(enemy, -1);
            }
        }

        public void DamagedBy(Entity enemy)
        {
            if (KnownEntities.Contains(enemy))
            {
                KnownEntities.MoveElement(enemy, -2);
            }
        }

        public void Prioritize()
        {
            Search();
            KnownItems.OrderBy(i => i.Location.DistanceTo(this.Location))
                      .ThenByDescending(i => i is Consumable);

            KnownEntities.OrderBy(e => e.Team == this.Team) // nonsentient npcs are only interested in enemies
                         .ThenBy(e => e.Location.DistanceTo(this.Location)) // especially ones that are closest
                         .ThenBy(e => e.CurrentHp.Value) // and have less hp remaining
                         .ThenByDescending(e => hasLineOfSightTo(e)); // and are in line of sight
            var enemies = KnownEntities.Where(e => e.Team != Team);
        }
    }
}