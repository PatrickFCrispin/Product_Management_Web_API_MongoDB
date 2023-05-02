namespace ProductManagement.API.Responses
{
    public class ServiceResponse<T> : GenericResponse
    {
        public T? Data { get; set; }
    }

    public class ServiceCollectionResponse<T> : GenericResponse
    {
        public IEnumerable<T>? Data { get; set; }
    }

    public class GenericResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}