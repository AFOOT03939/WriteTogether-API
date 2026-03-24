namespace WriteTogether.Features.Users
{
    public class UsersModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int ReputationPoints { get; set; }
        public string? Rol { get; set; }
        public string? ImageUrl { get; set; }
    }
}
