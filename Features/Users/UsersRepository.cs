using Dapper;
using WriteTogether.Dapper;
namespace WriteTogether.Features.Users
{
    public class UsersRepository
    {
        private readonly DbConnection _connection;

        public UsersRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<UsersModel?> GetUserById(int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS UserId,
                    username AS Username,
                    image_url AS ImageUrl,
                    reputation_points AS ReputationPoints
                FROM users
                WHERE id = @UserId;
            ";

            return await connection.QueryFirstOrDefaultAsync<UsersModel>(sql, new
            {
                UserId = userId
            });
        }

        public async Task<int> UpdateUser(int userId, string username, string? imageUrl)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE users
                SET 
                    username = @Username,
                    image_url = COALESCE(@ImageUrl, image_url)
                WHERE id = @UserId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                Username = username,
                ImageUrl = imageUrl
            });
        }
        public async Task<int> UpdateUserImage(int userId, string imageUrl)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE users
                SET image_url = @ImageUrl
                WHERE id = @UserId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                ImageUrl = imageUrl
            });
        }
    }
}
