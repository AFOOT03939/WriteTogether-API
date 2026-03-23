using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using WriteTogether.Features.Register;

namespace WriteTogether.Features.Register
{
    public class RegisterService
    {
        private readonly RegisterRepository _repo;
        public RegisterService(RegisterRepository repo)
        {
            _repo = repo;
        }
        public async Task<RegisterModel> RegisterUser(RegisterModel register)
        {
            var user = await _repo.RegisterUser(register);

            return user;
        }
    }
}
