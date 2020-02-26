using System;

namespace DatingApp.API.Core.Base
{
    public class BaseReturnModel<TData>
    {
        protected BaseReturnModel()
        {

        }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public TData Data { get; set; }

        public static BaseReturnModel<T> CreateSuccessInstance<T>(T Data, string message)
        {
            return new BaseReturnModel<T>()
            {
                Data = Data,
                ErrorMessage = message,
                IsSuccess = true
            };
        }

        public static BaseReturnModel<TData> CreateFailInstance(TData Data, string message)
        {
            return new BaseReturnModel<TData>()
            {
                Data = Data,
                ErrorMessage = message,
                IsSuccess = false
            };
        }
    }
}