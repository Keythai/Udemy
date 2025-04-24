using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonsAdderService : IPersonsAdderService
    {
        //private readonly ApplicationDbContext _context;

        //public PersonService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsAdderService(IPersonsRepository personRepository, ILogger<PersonsAdderService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));

            //Model Validations
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonId
            person.PersonId = Guid.NewGuid();

            //add matchingPerson object into persons list
            //_context.Persons.Add(person);
            //await _context.SaveChangesAsync();

            await _personsRepository.AddPerson(person);

            return person.ToPersonResponse();
        }
    }
}
