namespace MapleDreams.Models
{
    public class AttackCalculator
    {
        public List<string> Keys { get; set; }

        public Dictionary<string, Attribute> Equipments { get; set; }

        public string Data { get; set; }

        public decimal MaxAttack { get; private set; }

        public decimal MinAttack { get; private set; }

        public int MapleWarriorLevel { get; set; }

        public int WeaponSelect { get; set; }

        public decimal WeaponProficiency { get; set; }

        public List<string> WeaponTypes { get; private set; } = new List<string>()
        {
            "單手劍",
            "單手斧／棍／短杖／長杖（砍）",
            "單手斧／棍／短杖／長杖（戳）",
            "雙手劍",
            "雙手斧／棍 (砍)",
            "雙手斧／棍 (戳)",
            "槍（砍）",
            "槍（戳）",
            "矛（砍）",
            "矛（戳）",
            "短劍（非盜賊）",
            "短劍／飛鏢（盜賊）",
            "弓",
            "弩",
            "指虎",
            "火槍"
        };

        public DateTime DateTime { get; set; }


        public AttackCalculator()
        {
            InitKeys();

            Equipments = [];
            foreach (string key in Keys)
            {
                Equipments[key] = new Attribute();
            }
        }

        public void InitKeys()
        {
            Keys = new();

            foreach (var i in Enum.GetValues(typeof(EquipmentType)))
            {
                foreach (var j in Enum.GetValues(typeof(EquipmentCategory)))
                {
                    switch (j)
                    {
                        case EquipmentCategory.一般:
                            foreach (var k in Enum.GetValues(typeof(EquipmentNormalType)))
                            {
                                Keys.Add($"{i.ToString()}_{j.ToString()}_{k.ToString()}");
                            }
                            break;
                        case EquipmentCategory.飾品:
                            foreach (var k in Enum.GetValues(typeof(EquipmentAccessoriesType)))
                            {
                                Keys.Add($"{i.ToString()}_{j.ToString()}_{k.ToString()}");
                            }
                            break;
                        case EquipmentCategory.其他:
                            foreach (var k in Enum.GetValues(typeof(EquipmentOthersType)))
                            {
                                Keys.Add($"{i.ToString()}_{j.ToString()}_{k.ToString()}");
                            }
                            break;
                        case EquipmentCategory.寵物裝備:
                            for (int k = 0; k < 3; k++)
                            {
                                Keys.Add($"{i.ToString()}_{j.ToString()}_{k.ToString()}");
                            }
                            break;
                    }
                }
            }

            foreach (var i in Enum.GetValues(typeof(EquipmentExtendType)))
            {
                Keys.Add($"{i.ToString()}");
            }

            foreach (var i in Enum.GetValues(typeof(CharacterExtendType)))
            {
                Keys.Add($"{i.ToString()}");
            }

            foreach (var i in Enum.GetValues(typeof(SkillExtendType)))
            {
                Keys.Add($"{i.ToString()}");
            }
        }

        public void FromData(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                foreach (string snippet in data.Split(','))
                {
                    string[] parts = snippet.Split('=');
                    string dataKey = (parts[0].Count(x => x == '-') < 1) ? parts[0] : parts[0].Split("-")[0];
                    string dataAttribute = (parts[0].Count(x => x == '-') < 1) ? "" : parts[0].Split("-")[1];
                    string dataValue = parts.Length > 1 ? parts[1].Trim() : "0";

                    if (string.IsNullOrEmpty(dataAttribute))
                    {
                        switch (dataKey)
                        {
                            case "WeaponSelect":
                                WeaponSelect = int.TryParse(dataValue, out int weaponSelect) ? weaponSelect : 0;
                                break;
                            case "WeaponProficiency":
                                WeaponProficiency = decimal.TryParse(dataValue, out decimal proficiency) ? proficiency : 0;
                                break;
                            case "MapleWarrior":
                                MapleWarriorLevel = int.TryParse(dataValue, out int level) ? level : 0;
                                break;
                            default:
                                continue;
                        }
                    }

                    if (decimal.TryParse(dataValue, out decimal value))
                    {
                        if (dataAttribute == "STR")
                        {
                            if (Equipments[dataKey] == null)
                            {
                                Equipments[dataKey] = new Attribute() { Strength = value };
                            }
                            else
                            {
                                Equipments[dataKey].Strength = value;
                            }
                        }
                        if (dataAttribute == "DEX")
                        {
                            if (Equipments[dataKey] == null)
                            {
                                Equipments[dataKey] = new Attribute() { Dexterity = value };
                            }
                            else
                            {
                                Equipments[dataKey].Dexterity = value;
                            }
                        }
                        if (dataAttribute == "INT")
                        {
                            if (Equipments[dataKey] == null)
                            {
                                Equipments[dataKey] = new Attribute() { Intelligence = value };
                            }
                            else
                            {
                                Equipments[dataKey].Intelligence = value;
                            }
                        }
                        if (dataAttribute == "LUK")
                        {
                            if (Equipments[dataKey] == null)
                            {
                                Equipments[dataKey] = new Attribute() { Lucky = value };
                            }
                            else
                            {
                                Equipments[dataKey].Lucky = value;
                            }
                        }
                        if (dataAttribute == "AD")
                        {
                            if (Equipments[dataKey] == null)
                            {
                                Equipments[dataKey] = new Attribute() { AttackDamage = value };
                            }
                            else
                            {
                                Equipments[dataKey].AttackDamage = value;
                            }
                        }
                        if (dataAttribute == "AP")
                        {
                            if (Equipments[dataKey] == null)
                            {
                                Equipments[dataKey] = new Attribute() { AttackPower = value };
                            }
                            else
                            {
                                Equipments[dataKey].AttackPower = value;
                            }
                        }
                    }
                }
            }
        }

        public string ToData() => string.Join(",", Equipments.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        public void Calculate()
        {
            var attribute = new Models.Attribute();

            FromData(Data);
            foreach (string key in Keys)
            {
                if (Equipments.ContainsKey(key))
                {
                    var attrib = Equipments[key];
                    attribute.Strength += attrib.Strength;
                    attribute.Dexterity += attrib.Dexterity;
                    attribute.Lucky += attrib.Lucky;
                    attribute.Intelligence += attrib.Intelligence;
                    attribute.AttackDamage += attrib.AttackDamage;
                    attribute.AttackPower += attrib.AttackPower;
                }
            }
            if (attribute.AttackDamage == 0)
            {
                attribute.AttackDamage = 1;
            }

            if (MapleWarriorLevel > 0)
            {
                decimal ratio = (MapleWarriorLevel + 1) * 0.01m;
                attribute.MulWithRetio(ratio);
            }

            decimal priAttr = 0;
            decimal subAttr = 0;
            switch (WeaponSelect)
            {
                case 0:
                    priAttr = 4.0m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 1:
                    priAttr = 4.4m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 2:
                    priAttr = 3.2m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 3:
                    priAttr = 4.6m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 4:
                    priAttr = 4.8m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 5:
                    priAttr = 3.4m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 6:
                    priAttr = 3.0m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 7:
                    priAttr = 5.0m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 8:
                    priAttr = 5.0m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 9:
                    priAttr = 3.0m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 10:
                    priAttr = 4.0m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 11:
                    priAttr = 3.6m * attribute.Lucky;
                    subAttr = attribute.Strength + attribute.Dexterity;
                    break;
                case 12:
                    priAttr = 3.4m * attribute.Dexterity;
                    subAttr = attribute.Strength;
                    break;
                case 13:
                    priAttr = 3.6m * attribute.Dexterity;
                    subAttr = attribute.Strength;
                    break;
                case 14:
                    priAttr = 4.8m * attribute.Strength;
                    subAttr = attribute.Dexterity;
                    break;
                case 15:
                    priAttr = 3.6m * attribute.Dexterity;
                    subAttr = attribute.Strength;
                    break;
                default:
                    MaxAttack = 0;
                    break;
            }

            MaxAttack = (priAttr + subAttr) * attribute.AttackDamage / 100;
            MinAttack = (priAttr * 0.9m * (WeaponProficiency / 100) + subAttr) * attribute.AttackDamage / 100;
        }
    }

    public enum CharacterExtendType
    {
        角色基礎數值 = 0
    }

    public enum SkillExtendType
    {
        精靈的祝福 = 0,
        楓葉祝福 = 1,
        其他增益1 = 2,
        其他增益2 = 3,
        其他增益3 = 4,
    }
}
