using MapleDreams.Models;

namespace MapleDreams.Interfaces
{
    public interface IStoneRepository
    {
        Task<IEnumerable<Stone>> SelectStonesAsync();
        Task<IEnumerable<Stone>> SelectStonesByNameAsync(string name, bool byKeywork = false);
        Task<IEnumerable<Stone>> SelectStonesByTypeAsync(StoneType type);
        Task<IEnumerable<Stone>> SelectStonesBySerialNumberAsync(int serialNumber);
    }
}
