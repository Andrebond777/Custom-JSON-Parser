using DevTrust_Test_Task.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ninject;
using DevTrust_Test_Task.Extensions;
using DevTrust_Test_Task.Entities;
using System.Text.Json;
using DevTrust_Test_Task.Converters;
using System.Text.Json.Serialization;

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
            //IKernel ninjectKernel = new StandardKernel();
            //ninjectKernel.Bind<IPersonRepository>().To<PersonRepository>();
            //repository = ninjectKernel.Get<IPersonRepository>();
        }

        [HttpPut]
        public async Task<long> Save(string input)
        {
            //Person pers = JsonSerializer.Deserialize<Person>(input);

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
