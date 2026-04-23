using Dapper;
using WriteTogether.Dapper;
using WriteTogether.Features.StoriesCollaborators;

namespace WriteTogether.Features.Stories
{
    public class StoriesCollaboratorsRepository
    {
        private readonly DbConnection _connection;

        public StoriesCollaboratorsRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> AddCollaborator(int storyId, int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO story_collaborators (story_id, user_id, created_at)
                VALUES (@StoryId, @UserId, NOW());
            ";

            return await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId,
                UserId = userId
            });
        }

        public async Task<int> RemoveCollaborator(int storyId, int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM story_collaborators
                WHERE story_id = @StoryId AND user_id = @UserId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId,
                UserId = userId
            });
        }
        public async Task<bool> IsCollaborator(int storyId, int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT COUNT(1)
                FROM story_collaborators
                WHERE story_id = @StoryId AND user_id = @UserId;
            ";

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                StoryId = storyId,
                UserId = userId
            });

            return count > 0;
        }

        public async Task<StoriesCollaboratorsModel?> GetCollaborator(int storyId, int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    sc.story_id AS StoryId,
                    sc.user_id    AS UserId,
                    sc.created_at AS CreatedAt,
                    u.username    AS UserName
                FROM story_collaborators sc
                INNER JOIN users u
                ON u.id = sc.user_id
                WHERE story_id = @StoryId AND user_id = @UserId;
            ";

            return await connection.QueryFirstOrDefaultAsync<StoriesCollaboratorsModel>(sql, new
            {
                StoryId = storyId,
                UserId = userId
            });
        }

        public async Task<IEnumerable<StoriesCollaboratorsModel>> GetCollaborators(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    sc.story_id AS StoryId,
                    sc.user_id  AS UserId,
                    sc.created_at AS CreatedAt,
                    u.username  AS UserName
                FROM story_collaborators sc
                INNER JOIN users u ON u.id = sc.user_id
                WHERE sc.story_id = @StoryId;
            ";

            return await connection.QueryAsync<StoriesCollaboratorsModel>(sql, new
            {
                StoryId = storyId
            });
        }
    }
}