using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP294.Classes
{
    public class Rarity
    {
        public Rarity()
        {
        }
        public Rarity(string Name, float Percentage, HashSet<string> Drinks) 
        { 
            this.Name = Name;
            this.Percentage = Percentage;
            this.Drinks = Drinks;
        }
        public string Name { get; set; }
        public float Percentage { get; set; }
        public HashSet<string> Drinks { get; set; }
    }
}
