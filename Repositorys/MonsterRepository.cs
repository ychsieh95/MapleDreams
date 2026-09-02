using Dapper;
using Microsoft.Data.Sqlite;
using MapleDreams.Models;
using MapleDreams.Interfaces;

namespace Mystina.Repositorys
{
    public class MonsterRepository : IMonsterRepository
    {
        private readonly string connectionString;

        public MonsterRepository(string connectionString) =>
            this.connectionString = connectionString;

        public async Task<IEnumerable<Monster>> SelectMonstersAsync()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE 1=1;");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Monster>> SelectMonstersByNameAsync(string name, bool byKeywork = false)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (byKeywork)
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Name LIKE @Name;", new Monster() { Name = $"%{name}%" });
                    else
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Name=@Name;", new Monster() { Name = name });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Monster>> SelectMonstersByLevelAsync(int level)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Level=@Level;", new Monster() { Level = level });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Monster>> SelectMonstersByLocationAsync(string location, bool byKeywork = false)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (byKeywork)
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Location LIKE @Location;", new Monster() { Location = $"%{location}%" });
                    else
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Location=@Location;", new Monster() { Location = location });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Monster>> SelectMonstersByStoneAsync(string stone, bool byKeywork = false)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (byKeywork)
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Stone LIKE @Stone;", new Monster() { Stone = $"%{stone}%" });
                    else
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Stone=@Stone;", new Monster() { Stone = stone });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Monster>> SelectMonstersByCatalystAsync(string catalyst, bool byKeywork = false)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (byKeywork)
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Catalyst LIKE @Catalyst;", new Monster() { Catalyst = $"%{catalyst}%" });
                    else
                        return await conn.QueryAsync<Monster>("SELECT * FROM Monsters WHERE Catalyst=@Catalyst;", new Monster() { Catalyst = catalyst });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
