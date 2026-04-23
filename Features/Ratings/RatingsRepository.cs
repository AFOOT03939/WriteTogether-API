using Dapper;
using WriteTogether.Dapper;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Register;
namespace WriteTogether.Features.Ratings
{
    public class RatingsRepository
    {
        public readonly DbConnection _connection;
        public RatingsRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<RatingsModel>> GetRatingsByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT id AS RatingId, story_id AS StoryId, user_id AS UserId, rating_value AS Rating FROM ratings
                WHERE story_id = @StoryId;
                ";

            //devuelve una lista de los registros de la tabla
            var result = await connection.QueryAsync<RatingsModel>(sql, new {StoryId = storyId});

            return result;

        }

        public async Task<RatingsModel?> GetRatingsByAuthor(int userId, int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT id AS RatingId, story_id AS StoryId, user_id AS UserId, rating_value AS Rating FROM ratings
                WHERE user_id = @UserId
                AND story_id = @StoryId;
                ";

            //devuelve solo un registro
            var result = await connection.QueryFirstOrDefaultAsync<RatingsModel>(sql, 
                new { StoryId = storyId,
                      UserId = userId
                });

            return result;

        }

        public async Task<int> CreateRatingsByAuthor(RatingsModel rating)
        {
            using var connection = _connection.CreateConnection();

            //UPSERT, si existe un rating, hace update, de lo contrario, inserta
            var sql = @"
                INSERT INTO ratings (story_id, user_id, rating_value, created_at, created_by)
                VALUES (@StoryId, @UserId, @Rating, @CreatedAt, @CreatedBy)
                ON CONFLICT (story_id, user_id)
                DO UPDATE SET rating_value = EXCLUDED.rating_value
                RETURNING id;
            ";

            var parameters = new
            {
                rating.StoryId,
                rating.UserId,
                rating.Rating,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var ratingId = await connection.ExecuteScalarAsync<int>(sql, parameters);

            return ratingId;

        }

    }
}
