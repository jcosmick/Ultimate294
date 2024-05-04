using Org.BouncyCastle.Utilities;
using SCP294.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP294.Classes
{
    public class RarityConfig
    {
        public List<Rarity> rarities { get; set; }

        public void NormalizeRarities(float targetSum)
        {
            float sum = rarities.Sum(obj => obj.Percentage);

            float ratio = targetSum / sum;
            foreach (var rarity in rarities)
            {
                rarity.Percentage = rarity.Percentage * ratio;
            }
        }

        public Rarity GetRandomRarity(int randomNumber)
        {
            float sum = 0;
            foreach (var rarity in rarities)
            {
                if(randomNumber > sum && randomNumber <= rarity.Percentage+sum)
                    return rarity;
                sum += rarity.Percentage;
            }
            return null;
        }
    }
}
