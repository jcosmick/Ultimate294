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
        public static Rarity GetRarityFromDrink(CustomDrink drink)
        {
            foreach (var rarity in SCP294.Instance.Config.RarirtyConfigs.rarities)
            {
                if (rarity.Drinks.Intersect(drink.DrinkNames).Any())
                    return rarity;
            }
            return null;
        }
    }
}
