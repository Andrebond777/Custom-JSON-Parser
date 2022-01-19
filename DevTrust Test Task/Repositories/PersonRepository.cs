using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTrust_Test_Task.DataAccess;
using DevTrust_Test_Task.Entities;
using DevTrust_Test_Task.Extensions;

namespace DevTrust_Test_Task.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        ApplicationDbContext db = new ();

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            return await db.Persons.Include(u => u.Address).ToListAsync();
        }

        public IEnumerable<Person> FilterPersons(IEnumerable<Person> personsFromDb, GetAllRequest request)
        {
            personsFromDb = personsFromDb
                .Where(p => request.FirstName == p.FirstName || string.IsNullOrEmpty(request.FirstName))
                .Where(p => request.LastName == p.LastName || string.IsNullOrEmpty(request.LastName))
                .Where(p => request.City == p.Address.City || string.IsNullOrEmpty(request.City))
                .ToList();

            return personsFromDb;
        }

        public async Task<IEnumerable<Person>> GetFilteredPersonsAsync(GetAllRequest request)
        {
            var personsFromDb = await GetPersonsAsync();

            personsFromDb = FilterPersons(personsFromDb, request);

            return personsFromDb;
        }

        public async Task<Person> GetPersonAsync(long id)
        {
            return await db.Persons.SingleOrDefaultAsync(person => person.Id == id);
        }

        public async Task CreatePersonAsync(Person person)
        {
            await db.Persons.AddAsync(person);
            await db.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            //db.Entry(person).State = EntityState.Detached;
            db.Persons.Update(person);
            await db.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(long id)
        {
            var person = db.Persons.Find(id);
            db.Persons.Remove(person);
            await db.SaveChangesAsync();
        }
    }
}
