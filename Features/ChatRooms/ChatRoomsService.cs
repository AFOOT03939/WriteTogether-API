using WriteTogether.Features.Fragments;

namespace WriteTogether.Features.ChatRooms
{
    public class ChatRoomsService
    {
        private readonly ChatRoomsRepository _repo;
        public ChatRoomsService(ChatRoomsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ChatRoomsModel>> GetChatRooms(int storyId)
        {
            var result = await _repo.GetChatRooms(storyId);

            return result;
        }

        public async Task<int> CreateChatRoom(int storyId)
        {
            if (storyId <= 0)
                throw new ArgumentException("StoryId inválido");

            return await _repo.CreateChatRoom(storyId);
        }

        public async Task<bool> DeleteChatRoom(int chatRoomId)
        {
            if (chatRoomId <= 0)
                throw new ArgumentException("ChatRoomId inválido");

            return await _repo.DeleteChatRoom(chatRoomId);
        }

        public async Task<int> DeleteChatRoomsByStory(int storyId)
        {
            if (storyId <= 0)
                throw new ArgumentException("StoryId inválido");

            return await _repo.DeleteChatRoomsByStory(storyId);
        }
    }
}
