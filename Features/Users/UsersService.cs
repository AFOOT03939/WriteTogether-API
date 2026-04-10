namespace WriteTogether.Features.Users
{
    public class UsersService
    {
        private readonly UsersRepository _repo;

        public UsersService(UsersRepository repo)
        {
            _repo = repo;
        }


        public async Task<UsersModel?> GetUserById(int userId)
        {
            return await _repo.GetUserById(userId);
        }

        public async Task<bool> UpdateUser(int userId, string userName)
        {
            var result = await _repo.UpdateUser(userId, userName);

            return result > 0;
        }

        public async Task<bool> UpdateUserImage(int userId, string imageUrl)
        {
            var result = await _repo.UpdateUserImage(userId, imageUrl);

            return result > 0;
        }
    }
}

