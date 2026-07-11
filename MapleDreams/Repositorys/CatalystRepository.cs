using Dapper;
using Microsoft.Data.Sqlite;
using MapleDreams.Models;
using MapleDreams.Interfaces;

namespace Mystina.Repositorys
{
    public class CatalystRepository : ICatalystRepository
    {
        private readonly string connectionString;

        public CatalystRepository(string connectionString) =>
            this.connectionString = connectionString;

        public async Task<IEnumerable<Catalyst>> SelectCatalystsAsync()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Catalyst>("SELECT * FROM Catalysts WHERE 1=1;");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Catalyst>> SelectCatalystsByNameAsync(string name, bool byKeywork = false)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (byKeywork)
                        return await conn.QueryAsync<Catalyst>("SELECT * FROM Catalysts WHERE Name LIKE @Name;", new Catalyst() { Name = $"%{name}%" });
                    else
                        return await conn.QueryAsync<Catalyst>("SELECT * FROM Catalysts WHERE Name=@Name;", new Catalyst() { Name = name });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Catalyst>> SelectCatalystsByTypeAsync(CatalystType type)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Catalyst>("SELECT * FROM Catalysts WHERE Name=@Name;", new Catalyst() { Type = type });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Catalyst>> SelectCatalystsBySerialNumberAsync(int serialNumber)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Catalyst>("SELECT * FROM Catalysts WHERE Name=@Name;", new Catalyst() { SerialNumber = serialNumber });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
