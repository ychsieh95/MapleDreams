namespace MapleDreams.Models
{
    public class Stone
    {
        public string Name { get; set; }

        public StoneType Type { get; set; }

        public int SerialNumber { get; set; }

        public string Guid { get; set; }
    }

    public enum StoneType
    {
        水晶 = 0,
        礦石 = 1,
        寶石 = 2
    }
}
