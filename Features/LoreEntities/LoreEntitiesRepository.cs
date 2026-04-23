using Dapper;
using WriteTogether.Dapper;

namespace WriteTogether.Features.LoreEntities
{
    public class LoreEntitiesRepository
    {
        public readonly DbConnection _connection;
        public LoreEntitiesRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<LoreEntitiesModel>> GetByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT
                    id AS Id,
                    story_id AS StoryId,
                    name AS Name,
                    type AS Type,
                    description AS Description,
                    importance AS Importance,
                    first_fragment_id AS FirstFragmentId
                FROM lore_entities
                WHERE story_id = @StoryId
                ORDER BY id ASC;
            ";

            return await connection.QueryAsync<LoreEntitiesModel>(sql, new
            {
                StoryId = storyId
            });
        }

        public async Task<int> Create(LoreEntitiesModel model)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO lore_entities
                    (story_id, name, type, description, importance, first_fragment_id)
                VALUES
                    (@StoryId, @Name, @Type, @Description, @Importance, @FirstFragmentId);

                RETURNING id;
            ";

            return await connection.ExecuteScalarAsync<int>(sql, model);
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM lore_entities
                WHERE id = @Id;
            ";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                Id = id
            });

            return rowsAffected > 0;
        }

    }
}
