namespace Livros.Domain.Core.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }

        public ServiceResult(bool isSuccess, T value)
        {
            IsSuccess = isSuccess;
            Value = value;
        }

        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T>(true, value);
        }

        public static ServiceResult<T> Error()
        {
            return new ServiceResult<T>(false, default(T));
        }
    }
}