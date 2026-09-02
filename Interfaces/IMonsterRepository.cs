using MapleDreams.Models;

namespace MapleDreams.Interfaces
{
    public interface IMonsterRepository
    {
        Task<IEnumerable<Monster>> SelectMonstersAsync();
        Task<IEnumerable<Monster>> SelectMonstersByNameAsync(string name, bool byKeywork = false);
        Task<IEnumerable<Monster>> SelectMonstersByLevelAsync(int level);
        Task<IEnumerable<Monster>> SelectMonstersByLocationAsync(string location, bool byKeywork = false);
        Task<IEnumerable<Monster>> SelectMonstersByStoneAsync(string stone, bool byKeywork = false);
        Task<IEnumerable<Monster>> SelectMonstersByCatalystAsync(string catalyst, bool byKeywork = false);
    }
}
