using Dapper;
using WriteTogether.Dapper;

namespace WriteTogether.Features.StoriesMessages
{
    public class StoriesMessagesRepository
    {
        private readonly DbConnection _connection;

        public StoriesMessagesRepository(DbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<StoriesMessagesModel>> GetByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS MessageId,
                    story_id AS StoryId,
                    user_id AS UserId,
                    message AS Message,
                    image_url AS ImageUrl
                FROM story_messages
                WHERE story_id = @StoryId
                ORDER BY id ASC;
            ";

            return await connection.QueryAsync<StoriesMessagesModel>(sql, new { StoryId = storyId });
        }
        public async Task<int> Create(StoriesMessagesModel message)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO story_messages (story_id, user_id, message, image_url, created_at, created_by)
                OUTPUT INSERTED.id
                VALUES (@StoryId, @UserId, @Message, @ImageUrl, @CreatedAt, @CreatedBy);
            ";

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                message.StoryId,
                message.UserId,
                message.Message,
                message.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });
        }

        public async Task<int> Update(StoriesMessagesModel message)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE story_messages
                SET message = @Message,
                    image_url = @ImageUrl,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy
                WHERE id = @MessageId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                message.MessageId,
                message.Message,
                message.ImageUrl,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "system"
            });
        }
        public async Task<int> Delete(int messageId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM story_messages
                WHERE id = @MessageId
            ";

            return await connection.ExecuteAsync(sql, new { MessageId = messageId });
        }
    }
}