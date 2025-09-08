namespace DTO;
 public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public bool Success { get; set; }
    }