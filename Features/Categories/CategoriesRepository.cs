using Dapper;
using WriteTogether.Dapper;
using WriteTogether.Features.Stories;
namespace WriteTogether.Features.Categories
{
    public class CategoriesRepository
    {
        public readonly DbConnection _connection;
        public CategoriesRepository(DbConnection connection)
        {
            _connection = connection;
        }
           
        public async Task<IEnumerable<CategoriesModel>> Categories()
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT id AS CategoryId, name AS Name, description as Description FROM categories;
                ";
            //devuelve una lista de los registros de la tabla
            var result = await connection.QueryAsync<CategoriesModel>(sql);

            return result;
        }

        public async Task<IEnumerable<StoriesModel>> GetStoriesByCategory(int? categoryId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    s.id AS StoryId,
                    s.title AS Title,
                    s.description AS Description,
                    s.creator_user_id AS UserId,
                    s.status AS Status,
                    s.visibility AS Visibility,
                    s.image_url AS ImageUrl
                FROM story_categories sc
                INNER JOIN stories s ON s.id = sc.story_id
                WHERE sc.category_id = @CategoryId;
                ";

            //devuelve una lista de los registros de la tabla
            var result = await connection.QueryAsync<StoriesModel>(sql, new
            {
                CategoryId = categoryId
            });

            return result;
        }
    }
}
