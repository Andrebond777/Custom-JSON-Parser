using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevTrust_Test_Task.Entities;

namespace DevTrust_Test_Task.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAddressesAsync();
        Task<Address> GetAddressAsync(long id);
        Task CreateAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(long id);
    }
}