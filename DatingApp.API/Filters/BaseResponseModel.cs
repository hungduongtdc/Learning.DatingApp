using System.Collections.Generic;

namespace DatingApp.API.Filters
{
    public class BaseResponseModel
    {
        public object Data { get; set; }
        public string Message { get; set; }
    }
}