using SCP294.Classes;
using SCP294.Types.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP294.Utils
{
    public class RarityUtils
    {
        public static Rarity GetRarityFromDrink(string drinkName)
        {
            return SCP294.Instance.Config.RarityConfigs.rarities.FirstOrDefault(rarity => rarity.Drinks.Contains(drinkName));
        }

        public static Rarity GetRarityFromDrink(CustomDrink drink)
        {
            return SCP294.Instance.Config.RarityConfigs.rarities.FirstOrDefault(rarity => rarity.Drinks.Intersect(drink.DrinkNames).Any());
        }

    }
}
