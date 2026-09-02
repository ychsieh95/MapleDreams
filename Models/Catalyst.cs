namespace MapleDreams.Models
{
    public class Catalyst
    {
        public string Name { get; set; }

        public CatalystType Type { get; set; }

        public int SerialNumber { get; set; }

        public string Guid { get; set; }
    }

    public enum CatalystType
    {
        防具 = 0,
        武器 = 1
    }
}
