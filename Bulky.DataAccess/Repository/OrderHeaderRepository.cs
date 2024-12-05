using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }



        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var objFromDb = _db.OrderHeaders.FirstOrDefault(s => s.Id == id);
            if (objFromDb != null)
            {
                objFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    objFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntId)
        {
            var objFromDb = _db.OrderHeaders.FirstOrDefault(s => s.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                objFromDb.SessionId = sessionId;
              
            }
            if (!string.IsNullOrEmpty(paymentIntId))
            {
                objFromDb.PaymentIntentId = paymentIntId;
                objFromDb.PaymentDate = System.DateTime.Now;

            }
        }
    }
}
