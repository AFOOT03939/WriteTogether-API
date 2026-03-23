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
                        id AS FragmentId, 
                        story_id AS StoryId, 
                        author_id as UserId, 
                        content as Content, 
                        order_index AS OrderIndex, 
                        image_url AS ImageUrl
                    FROM fragments
                    WHERE story_id = @StoryId;
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
                        id AS FragmentId, 
                        story_id AS StoryId, 
                        author_id as UserId, 
                        content as Content, 
                        order_index AS OrderIndex, 
                        image_url AS ImageUrl
                    FROM fragments
                    WHERE author_id = @UserId;
                ";

            var result = await connection.QueryAsync<FragmentsModel>(sql, new
            {
                UserId = userId
            });

            return result;
        }

        public async Task<int> CreateFragments(FragmentsModel fragment)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                IF EXISTS (
                    SELECT 1 FROM fragments 
                    WHERE story_id = @StoryId AND author_id = @UserId
                )
                BEGIN
                    UPDATE fragments
                    SET content = @Content
                    WHERE story_id = @StoryId AND author_id = @UserId;

                    SELECT id FROM fragments
                    WHERE story_id = @StoryId AND author_id = @UserId;
                END
                ELSE
                BEGIN
                    INSERT INTO fragments (story_id, author_id, content, order_index, image_url, created_at, created_by)
                    OUTPUT INSERTED.id
                    VALUES (@StoryId, @UserId, @Content, @OrderIndex, ImageUrl, @CreatedAt, @CreatedBy);
                END
                ";

            var parameters = new
            {
                fragment.StoryId,
                fragment.AuthorId,
                fragment.Content,
                fragment.ImageUrl,
                fragment.OrderIndex,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var fragmentId = await connection.ExecuteScalarAsync<int>(sql, parameters);

            return fragmentId;
        }

    }
}
