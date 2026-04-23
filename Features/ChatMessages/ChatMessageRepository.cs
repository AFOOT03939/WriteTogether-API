using Dapper;
using WriteTogether.Dapper;

namespace WriteTogether.Features.ChatMessages
{
    public class ChatMessageRepository
    {
        public readonly DbConnection _connection;
        public ChatMessageRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<ChatMessageModel>> GetMessagesByRoom(int roomId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS ChatMessageId,
                    room_id AS RoomId,
                    user_id AS UserId,
                    message AS Message,
                    image_url AS ImageUrl
                FROM chat_messages
                WHERE room_id = @RoomId
                ORDER BY id ASC;
            ";

            return await connection.QueryAsync<ChatMessageModel>(sql, new
            {
                RoomId = roomId
            });
        }

        // 🔍 GET por ID
        public async Task<ChatMessageModel?> GetMessageById(int chatMessageId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS ChatMessageId,
                    room_id AS RoomId,
                    user_id AS UserId,
                    message AS Message,
                    image_url AS ImageUrl
                FROM chat_messages
                WHERE id = @ChatMessageId;
            ";

            return await connection.QueryFirstOrDefaultAsync<ChatMessageModel>(sql, new
            {
                ChatMessageId = chatMessageId
            });
        }
        public async Task<int> CreateMessage(ChatMessageModel model)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO chat_messages (room_id, user_id, message, image_url)
                VALUES (@RoomId, @UserId, @Message, @ImageUrl);

                RETURNING id;
            ";

            var id = await connection.ExecuteScalarAsync<int>(sql, model);
            return id;
        }

        public async Task<bool> UpdateMessage(ChatMessageModel model)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE chat_messages
                SET 
                    room_id = @RoomId,
                    user_id = @UserId,
                    message = @Message,
                    image_url = @ImageUrl
                WHERE id = @ChatMessageId;
            ";

            var rowsAffected = await connection.ExecuteAsync(sql, model);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteMessage(int chatMessageId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM chat_messages
                WHERE id = @ChatMessageId;
            ";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                ChatMessageId = chatMessageId
            });

            return rowsAffected > 0;
        }

        public async Task<int> DeleteMessagesByRoom(int roomId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM chat_messages
                WHERE room_id = @RoomId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                RoomId = roomId
            });
        }
    }
}
