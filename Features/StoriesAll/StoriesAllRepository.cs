using Dapper;
using Microsoft.AspNetCore.Mvc;
using WriteTogether.Dapper;
using WriteTogether.Features.Fragments;
namespace WriteTogether.Features.StoriesAll
{
    public class StoriesAllRepository
    {
        public readonly DbConnection _connection;
        public StoriesAllRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task <IEnumerable<StoriesAllModel>> GetFullStoriesByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    SELECT 
                        id AS Id, 
                        story_id AS StoryId, 
                        summary_text AS SummaryText,
                        version AS Version
                    FROM story_summaries
                    WHERE story_id = @StoryId
                    AND version = (
                        SELECT MAX(version)
                        FROM story_summaries
                        WHERE story_id = @StoryId);
                    ";

            var result = await connection.QueryAsync<StoriesAllModel>(sql, new
            {
                StoryId = storyId
            });

            return result;
        }

        public async Task<int> CreateFullStoryByStory(int storyId, StoriesAllModelDto story)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                BEGIN TRAN;

                DECLARE @NextVersion INT;

                SELECT @NextVersion = ISNULL(MAX(version), 0) + 1
                FROM story_summaries WITH (UPDLOCK, HOLDLOCK)
                WHERE story_id = @StoryId;

                INSERT INTO story_summaries 
                    (story_id, summary_text, generated_at, version)
                OUTPUT INSERTED.id
                VALUES 
                    (@StoryId, @SummaryText, @CreatedAt, @NextVersion);

                COMMIT;
                ";

            var parameters = new
            {
                StoryId = storyId,
                story.SummaryText,
                CreatedAt = DateTime.UtcNow,
                story.Version
            };

            var result = await connection.ExecuteScalarAsync<int>(sql, parameters);

            return result;

        }
    }
}
