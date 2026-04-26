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
                sm.id AS MessageId,
                sm.story_id AS StoryId,
                sm.user_id AS UserId,
                sm.message AS Message,
                sm.image_url AS ImageUrl,
                sm.created_at AS CreatedAt,
                u.username AS UserName
            FROM story_messages sm
            INNER JOIN users u ON u.id = sm.user_id
            WHERE sm.story_id = @StoryId
            ORDER BY sm.id ASC;
            ";

            return await connection.QueryAsync<StoriesMessagesModel>(sql, new { StoryId = storyId });
        }
        public async Task<int> Create(StoriesMessagesModel message)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO story_messages (story_id, user_id, message, image_url, created_at, created_by)
                VALUES (@StoryId, @UserId, @Message, @ImageUrl, @CreatedAt, @CreatedBy)
                RETURNING id;
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

        public async Task<int> UpdateMessageImage(int messageId, string imageUrl)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    UPDATE story_messages
                    SET image_url = @ImageUrl
                    WHERE id = @MessageId;
                ";

            return await connection.ExecuteAsync(sql, new
            {
                MessageId = messageId,
                ImageUrl = imageUrl
            });
        }
    }
}