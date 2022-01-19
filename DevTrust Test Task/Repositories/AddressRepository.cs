using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DevTrust_Test_Task.DataAccess;
using DevTrust_Test_Task.Entities;

namespace DevTrust_Test_Task.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        ApplicationDbContext db = new ();

        public async Task<IEnumerable<Address>> GetAddressesAsync()
        {
            return await db.Addresses.ToListAsync();
        }

        public async Task<Address> GetAddressAsync(long id)
        {
            return await db.Addresses.SingleOrDefaultAsync(address => address.Id == id);
        }

        public async Task CreateAddressAsync(Address address)
        {
            await db.Addresses.AddAsync(address);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Address address)
        {
            db.Addresses.Update(address);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(long id)
        {
            var address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
            await db.SaveChangesAsync();
        }
    }
}
