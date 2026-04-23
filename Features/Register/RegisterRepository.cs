using Dapper;
using WriteTogether.Dapper;
namespace WriteTogether.Features.Register
{
    public class RegisterRepository
    {
        public readonly DbConnection _connection;
        public RegisterRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<RegisterModel> RegisterUser(RegisterModel register)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO users (username, email, password, role, status, created_at, created_by)
                VALUES (@UserName, @Email, @Password, @Role, @Status, @CreatedAt, @CreatedBy)
                RETURNING id;
                ";

            var parameters = new
            {
                register.UserName,
                register.Email,
                register.Password,
                Role = "user",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var userId = await connection.ExecuteScalarAsync<int>(sql, parameters);

            register.UserId = userId;

            return register;
        }

    }
}
