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
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        //private readonly ApplicationDbContext _context;

        //public PersonService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsUpdaterService(IPersonsRepository personRepository, ILogger<PersonsUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching matchingPerson object to update
            //Person? matchingPerson = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == personUpdateRequest.PersonId);
            Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (matchingPerson == null) throw new InvalidPersonIdException("Given id does not exist");

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            //await _context.SaveChangesAsync();
            await _personsRepository.UpdatePerson(matchingPerson);

            return matchingPerson.ToPersonResponse();
        }

    }
}
