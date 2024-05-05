using SCP294.Types.Config;
using System;
using System.Linq;

namespace SCP294.Classes
{
    public class RarityManager
    {
        public RarityConfig Config { get; set; }

        public void NormalizeRarities(float targetSum)
        {
            float sum = Config.rarities.Sum(obj => obj.Percentage);

            float ratio = targetSum / sum;
            foreach (var rarity in Config.rarities)
            {
                rarity.Percentage = rarity.Percentage * ratio;
            }
        }

        public Rarity GetRandomRarity(int randomNumber)
        {
            float sum = 0;
            foreach (var rarity in Config.rarities)
            {
                if (randomNumber > sum && randomNumber <= rarity.Percentage + sum)
                    return rarity;
                sum += rarity.Percentage;
            }
            throw new InvalidOperationException("No Rarity found. Check rarity values in configuration file.");
        }

        public Rarity GetRarityFromDrink(string drinkName)
        {
            return SCP294.Instance.Config.RarityConfigs.rarities.FirstOrDefault(rarity => rarity.Drinks.Contains(drinkName));
        }

        public Rarity GetRarityFromDrink(CustomDrink drink)
        {
            return SCP294.Instance.Config.RarityConfigs.rarities.FirstOrDefault(rarity => rarity.Drinks.Intersect(drink.DrinkNames).Any());
        }
    }
}
