using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevTrust_Test_Task.Entities;
using DevTrust_Test_Task.Extensions;

namespace DevTrust_Test_Task.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersonsAsync();
        IEnumerable<Person> FilterPersons(IEnumerable<Person> personsFromDb, GetAllRequest request);
        Task<IEnumerable<Person>> GetFilteredPersonsAsync(GetAllRequest request);
        Task<Person> GetPersonAsync(long id);
        Task CreatePersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(long id);
    }
}