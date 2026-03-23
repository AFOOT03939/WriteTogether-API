using Dapper;
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

    }
}
