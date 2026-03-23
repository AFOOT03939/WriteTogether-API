using Dapper;
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
    }
}
