namespace ToDoApp.Configurations
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public bool Status { get; set; }
        public List<string> Error { get; set; }

    }
}
