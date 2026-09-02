namespace MapleDreams.Models
{
    public class Attribute
    {
        public decimal Level { get; set; }

        public decimal Strength { get; set; }

        public decimal Dexterity { get; set; }

        public decimal Lucky { get; set; }

        public decimal Intelligence { get; set; }

        public decimal AttackDamage { get; set; }

        public decimal AttackPower { get; set; }

        public decimal GetValue(string key)
        {
            if (key == "LV")
            {
                return Level;
            }
            if (key == "STR" || key == "Strength")
            {
                return Strength;
            }
            if (key == "DEX" || key == "Dexterity")
            {
                return Dexterity;
            }
            if (key == "LUK" || key == "Lucky")
            {
                return Lucky;
            }
            if (key == "INT" || key == "Intelligence")
            {
                return Intelligence;
            }
            if (key == "AD" || key == "AttackDamage")
            {
                return AttackDamage;
            }
            if (key == "AP" || key == "AttackPower")
            {
                return AttackPower;
            }
            return -1;
        }

        public void MulWithRetio(decimal ratio, bool withLevel = false, bool withAttack = false)
        {
            if (withLevel)
            {
                Level *= ratio;
            }
            Strength *= ratio;
            Dexterity *= ratio;
            Lucky *= ratio;
            Intelligence *= ratio;
            if (withAttack)
            {
                AttackDamage *= ratio;
                AttackPower *= ratio;
            }
        }
    }
}
