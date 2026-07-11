using MapleDreams.Models;

namespace MapleDreams.Interfaces
{
    public interface ICatalystRepository
    {
        Task<IEnumerable<Catalyst>> SelectCatalystsAsync();
        Task<IEnumerable<Catalyst>> SelectCatalystsByNameAsync(string name, bool byKeywork = false);
        Task<IEnumerable<Catalyst>> SelectCatalystsByTypeAsync(CatalystType type);
        Task<IEnumerable<Catalyst>> SelectCatalystsBySerialNumberAsync(int serialNumber);
    }
}
