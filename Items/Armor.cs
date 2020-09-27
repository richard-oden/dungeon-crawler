using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Armor : Item
    {
        public int ArmorClassBonus {get; private set;}
        public string Slot {get; private set;}
    }
}