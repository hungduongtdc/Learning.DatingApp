using DatingApp.API.Data;

namespace DatingApp.API.Core.Base {
    public class BaseService : IBaseService {
        protected DataContext _context { get; set; }
        public BaseService (DataContext context) {
            this._context = context;
        }

    }
}