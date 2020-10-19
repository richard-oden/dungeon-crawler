using System.Collections.Generic;

namespace DungeonCrawler
{
    public class InitiativeComparer : IComparer<Entity>
    {
        public int Compare(Entity x, Entity y)
        {
            // Sort by initiative:
            if (y.CurrentInitiative < x.CurrentInitiative)
            {
                return -1;
            }
            else if (y.CurrentInitiative > x.CurrentInitiative)
            {
                return 1;
            }
            // If initiatives are equal, sort by DEX score:
            else
            {
                if (y.AbilityScores.TotalScores["DEX"] < x.AbilityScores.TotalScores["DEX"])
                {
                    return -1;
                }
                else if (y.AbilityScores.TotalScores["DEX"] > x.AbilityScores.TotalScores["DEX"])
                {
                    return 1;
                }
                // If DEX scores are equal, roll off until winner:
                else
                {
                    while (true)
                    {
                        int yRoll = Dice.D20.Roll();
                        int xRoll = Dice.D20.Roll();
                        if (yRoll < xRoll)
                        {
                            return -1;
                        }
                        else if (yRoll > xRoll)
                        {
                            return 1;
                        }
                    }
                }
            }
        }
    }
}