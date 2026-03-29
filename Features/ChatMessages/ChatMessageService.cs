namespace WriteTogether.Features.ChatMessages
{
    public class ChatMessageService
    {
        private readonly ChatMessageRepository _repo;

        public ChatMessageService(ChatMessageRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ChatMessageModel>> GetMessagesByRoom(int roomId)
        {
            if (roomId <= 0)
                throw new ArgumentException("invalide RoomId");

            return await _repo.GetMessagesByRoom(roomId);
        }
        public async Task<ChatMessageModel?> GetMessageById(int chatMessageId)
        {
            if (chatMessageId <= 0)
                throw new ArgumentException("invalide ChatMessageId");

            return await _repo.GetMessageById(chatMessageId);
        }

        public async Task<int> CreateMessage(ChatMessageModel model)
        {
            if (model.RoomId <= 0)
                throw new ArgumentException("invalide RoomId");

            if (model.UserId <= 0)
                throw new ArgumentException("invalide UserId");

            if (string.IsNullOrWhiteSpace(model.Message) && string.IsNullOrWhiteSpace(model.ImageUrl))
                throw new ArgumentException("El mensaje o la imagen son requeridos");

            return await _repo.CreateMessage(model);
        }

        public async Task<bool> UpdateMessage(ChatMessageModel model)
        {
            if (model.ChatMessageId <= 0)
                throw new ArgumentException("invalide ChatMessageId");

            if (model.RoomId <= 0)
                throw new ArgumentException("invalide RoomId");

            if (model.UserId <= 0)
                throw new ArgumentException("Invalide UserId");

            return await _repo.UpdateMessage(model);
        }

        public async Task<bool> DeleteMessage(int chatMessageId)
        {
            if (chatMessageId <= 0)
                throw new ArgumentException("invalide ChatMessageId");

            return await _repo.DeleteMessage(chatMessageId);
        }

        public async Task<int> DeleteMessagesByRoom(int roomId)
        {
            if (roomId <= 0)
                throw new ArgumentException("invalide RoomId");

            return await _repo.DeleteMessagesByRoom(roomId);
        }
    }
    
}
