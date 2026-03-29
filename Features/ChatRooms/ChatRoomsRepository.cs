using Dapper;
using WriteTogether.Dapper;
using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.ChatRooms
{
    public class ChatRoomsRepository
    {
        public readonly DbConnection _connection;
        public ChatRoomsRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<ChatRoomsModel>> GetChatRooms(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    SELECT 
                        id AS ChatRoomId,
                        story_id AS StoryId
                    FROM chat_rooms
                    WHERE story_id = @StoryId;
                ";

            var result = await connection.QueryAsync<ChatRoomsModel>(sql, new
            {
                StoryId = storyId
            });

            return result;
        }

        public async Task<int> CreateChatRoom(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO chat_rooms (story_id)
                VALUES (@StoryId);
        
                SELECT LAST_INSERT_ID();
            ";

            var id = await connection.ExecuteScalarAsync<int>(sql, new
            {
                StoryId = storyId
            });

            return id;
        }

        public async Task<bool> DeleteChatRoom(int chatRoomId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM chat_rooms
                WHERE id = @ChatRoomId;
            ";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                ChatRoomId = chatRoomId
            });

            return rowsAffected > 0;
        }

        public async Task<int> DeleteChatRoomsByStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM chat_rooms
                WHERE story_id = @StoryId;
            ";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId
            });

            return rowsAffected;
        }

    }
}
