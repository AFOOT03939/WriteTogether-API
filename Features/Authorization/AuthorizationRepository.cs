using Dapper;
using WriteTogether.Dapper;

namespace WriteTogether.Features.Authorization
{
    public class AuthorizationRepository
    {
        public readonly DbConnection _connection;
        public AuthorizationRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<AuthorizationModel?> GetUserByEmail(string email)
        {
            using var connection = _connection.CreateConnection();

            var sql = "SELECT id AS UserId, email as Email, password as Password FROM users WHERE email = @Email";

            return await connection.QueryFirstOrDefaultAsync<AuthorizationModel>(
                sql,
                new { Email = email }
            );
        }
    }
}
