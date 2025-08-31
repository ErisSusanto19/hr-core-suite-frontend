namespace HRCoreSuite.Frontend.ViewModels.Common
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess => ErrorMessages.Count == 0;
        public List<string> ErrorMessages { get; set; } = new();
    }
}