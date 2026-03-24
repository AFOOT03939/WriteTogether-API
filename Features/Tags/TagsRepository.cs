using Dapper;
using Microsoft.Win32;
using WriteTogether.Dapper;
using WriteTogether.Features.Stories;
namespace WriteTogether.Features.Tags
{
    public class TagsRepository
    {
        public readonly DbConnection _connection;
        public TagsRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<TagsModel>> GetTagsByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    SELECT 
                    t.id AS TagId, 
                    t.name AS Name 
                    FROM tags t
                    INNER JOIN story_tags st
                    ON t.id = st.tag_id
                    WHERE st.story_id = @StoryId;
                    ";

            var result = await connection.QueryAsync<TagsModel>(sql, new
            {
                StoryId = storyId
            });

            return result;
        }

        public async Task<int> GetTagsByName(string name)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    SELECT id FROM tags
                    WHERE LOWER(name) = LOWER(@Name);
                    ";

            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new
            {
                Name = name
            });

            return result;
        }

        public async Task<int> CreateTags(string name)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO tags (name, created_at, created_by)
                OUTPUT INSERTED.id
                VALUES (@Name, @CreatedAt,@CreatedBy);
            ";

            var parameters = new
            {
                Name = name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var tagId = await connection.ExecuteScalarAsync<int>(sql, parameters);

            return tagId;
        }
    }
}
