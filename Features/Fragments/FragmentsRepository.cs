using Dapper;
using WriteTogether.Dapper;
using WriteTogether.Features.Authorization;
using WriteTogether.Features.Categories;
namespace WriteTogether.Features.Fragments
{
    public class FragmentsRepository
    {
        public readonly DbConnection _connection;
        public FragmentsRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<FragmentsModel>> GetFragmentsByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    f.id AS FragmentId, 
                    f.story_id AS StoryId, 
                    f.author_id AS UserId, 
                    f.content AS Content, 
                    f.order_index AS OrderIndex, 
                    f.image_url AS ImageUrl,
                    f.created_at AS CreatedAt,
                    u.username AS UserName
                FROM fragments f
                INNER JOIN users u 
                ON u.id = f.author_id
                WHERE f.story_id = @StoryId
                ORDER BY f.order_index;
            ";

            var result = await connection.QueryAsync<FragmentsModel>(sql, new
            {
                StoryId = storyId
            });

            return result;
        }

        public async Task<IEnumerable<FragmentsModel>> GetFragmentsByUser(int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    f.id AS FragmentId, 
                    f.story_id AS StoryId, 
                    f.author_id AS UserId, 
                    f.content AS Content, 
                    f.order_index AS OrderIndex, 
                    f.image_url AS ImageUrl,
                    f.created_at AS CreatedAt,
                    u.username AS UserName
                FROM fragments f
                INNER JOIN users u 
                ON u.id = f.author_id
                WHERE f.author_id = @UserId
                ORDER BY f.created_at DESC;
            ";

            var result = await connection.QueryAsync<FragmentsModel>(sql, new
            {
                UserId = userId
            });

            return result;
        }

        public async Task<int> CreateFragment(FragmentsModel fragment)
        {
            using var connection = _connection.CreateConnection();

            // 🔥 calcular orden
            var orderSql = @"
        SELECT ISNULL(MAX(order_index), 0) + 1
        FROM fragments
        WHERE story_id = @StoryId;
    ";

            var orderIndex = await connection.ExecuteScalarAsync<int>(orderSql, new
            {
                fragment.StoryId
            });

            var sql = @"
                INSERT INTO fragments (
                    story_id,
                    author_id,
                    content,
                    order_index,
                    image_url,
                    created_at,
                    created_by
                )
                OUTPUT INSERTED.id
                VALUES (
                    @StoryId,
                    @UserId,
                    @Content,
                    @OrderIndex,
                    @ImageUrl,
                    @CreatedAt,
                    @CreatedBy
                );
            ";

            var parameters = new
            {
                fragment.StoryId,
                fragment.UserId,
                fragment.Content,
                OrderIndex = orderIndex,
                fragment.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            return await connection.ExecuteScalarAsync<int>(sql, parameters);
        }

        public async Task<int> UpdateFragment(int fragmentId, string content)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE fragments
                SET 
                    content = @Content,
                    updated_at = SYSDATETIME()
                WHERE id = @FragmentId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                FragmentId = fragmentId,
                Content = content
            });
        }

        public async Task<int> DeleteFragments(int fragmentId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    DELETE FROM fragments
                    WHERE id = @FragmentId
                ";

            var result = await connection.ExecuteAsync(sql, new
            {
                FragmentId = fragmentId
            });

            return result;
        }

    }
}
