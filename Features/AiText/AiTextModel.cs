namespace WriteTogether.Features.AiText
{
    public class AiTextRequest
    {
        public int FragmentId { get; set; }
        public string Prompt { get; set; } = string.Empty;
    }
}