using GameCatalog.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Repositories
{
    public class GameSqlRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;

        public GameSqlRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Game>> Get(int page, int quantity)
        {
            var games = new List<Game>();

            var command = $"select * from Games order by id offset {((page - 1) * quantity)} rows fetch next {quantity} rows only";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Description = (string)sqlDataReader["Description"],
                    Developer = (string)sqlDataReader["Developer"],
                    Publisher = (string)sqlDataReader["Publisher"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task<Game> Get(Guid id)
        {
            Game game = null;
            
            var command = $"select * from Games where Id = '{id}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Description = (string)sqlDataReader["Description"],
                    Developer = (string)sqlDataReader["Developer"],
                    Publisher = (string)sqlDataReader["Publisher"],
                    Price = (double)sqlDataReader["Price"]
                };
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> Get(string name, string developer)
        {
            var game = new List<Game>();

            var command = $"select * from Games where Name = '{name}' and Developer = '{developer}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Description = (string)sqlDataReader["Description"],
                    Developer = (string)sqlDataReader["Developer"],
                    Publisher = (string)sqlDataReader["Publisher"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task Insert(Game game)
        {
            var command = $"insert Games (Id, Name, Description, Developer, Publisher, Price) Values ('{game.Id}', '{game.Name}', '{game.Description}', '{game.Developer}', '{game.Publisher}', '{game.Price.ToString()}')";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Remove(Guid id)
        {
            var command = $"delete from Games where Id = '{id}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }

        public async Task Update(Game game)
        {
            var command = $"update Games set Name = '{game.Name}', Developer = '{game.Developer}', Description = '{game.Description}' Publisher = '{game.Publisher}', Price = '{game.Price.ToString()}' where id = '{game.Id}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
