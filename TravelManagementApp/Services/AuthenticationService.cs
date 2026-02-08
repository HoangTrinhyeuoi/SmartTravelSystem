using TravelDataAccess.Data;
using TravelDataAccess.Models;

namespace TravelManagementApp.Services
{
    public class AuthenticationService
    {
        private readonly TravelDbContext _context;

        public AuthenticationService()
        {
            _context = new TravelDbContext();
        }

        public Customer? Login(string code, string password)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Code == code && c.Password == password);
            return customer;
        }
    }
}