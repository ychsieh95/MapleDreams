using Dapper;
using Microsoft.Data.Sqlite;
using MapleDreams.Models;
using MapleDreams.Interfaces;

namespace Mystina.Repositorys
{
    public class StoneRepository : IStoneRepository
    {
        private readonly string connectionString;

        public StoneRepository(string connectionString) =>
            this.connectionString = connectionString;

        public async Task<IEnumerable<Stone>> SelectStonesAsync()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Stone>("SELECT * FROM Stones WHERE 1=1;");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Stone>> SelectStonesByNameAsync(string name, bool byKeywork = false)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (byKeywork)
                        return await conn.QueryAsync<Stone>("SELECT * FROM Stones WHERE Name LIKE @Name;", new Stone() { Name = $"%{name}%" });
                    else
                        return await conn.QueryAsync<Stone>("SELECT * FROM Stones WHERE Name=@Name;", new Stone() { Name = name });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Stone>> SelectStonesByTypeAsync(StoneType type)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Stone>("SELECT * FROM Stones WHERE Name=@Name;", new Stone() { Type = type });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Stone>> SelectStonesBySerialNumberAsync(int serialNumber)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Stone>("SELECT * FROM Stones WHERE Name=@Name;", new Stone() { SerialNumber = serialNumber });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
