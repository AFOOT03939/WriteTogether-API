using Dapper;
using Microsoft.Win32;
using WriteTogether.Dapper;
using WriteTogether.Features.Ratings;

namespace WriteTogether.Features.Stories
{
    public class StoriesRepository
    {
        public readonly DbConnection _connection;
        public StoriesRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<StoriesModel>> GetAllStories()
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS StoryId,
                    title AS Title,
                    description AS Description,
                    creator_user_id AS UserId,
                    status AS Status,
                    visibility AS Visibility,
                    image_url AS ImageUrl
                FROM stories;
                ";

            var result = await connection.QueryAsync<StoriesModel>(sql);

            return result;

        }

        public async Task<bool> CreateStoryTag(int storyId, int tagId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO story_tags (story_id, tag_id)
                VALUES (@StoryId, @TagId);
                ";

            var parameters = new
            {
                StoryId = storyId,
                TagId = tagId
            };

            var result = await connection.ExecuteScalarAsync<int>(sql, parameters);

            return result > 0;

        }

        public async Task<int> DeleteStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM stories
                WHERE id = @StoryId
            ";

            var result = await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId
            });

            return result;
        }

    }
}
