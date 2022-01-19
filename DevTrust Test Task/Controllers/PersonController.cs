using DevTrust_Test_Task.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevTrust_Test_Task.Extensions;
using DevTrust_Test_Task.Entities;
using DevTrust_Test_Task.Converters;

namespace DevTrust_Test_Task.Controllers
{
    [ApiController]
    [Route("persons")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        [HttpPut]
        public async Task<long> Save(string input)
        {
            var person = CustomJsonConverter.Deserialize<Person>(input);

            if (await personRepository.GetPersonAsync(person.Id) is null)
                await personRepository.CreatePersonAsync(person);
            else
                await personRepository.UpdatePersonAsync(person);

            return person.Id;
        }

        //GET /persons
        [HttpGet]
        public async Task<string> GetAll(GetAllRequest request)
        {
            var personsFromDb = await personRepository.GetFilteredPersonsAsync(request);

            string result = CustomJsonConverter.Serialize(personsFromDb);

            return result;
        }
    }
}
