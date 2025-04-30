using CsvHelper;
using CsvHelper.Configuration;
using Entities;

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
    public class PersonsDeleterService : IPersonsDeleterService
    {
        //private readonly ApplicationDbContext _context;

        //public PersonService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsDeleterService(IPersonsRepository personRepository, ILogger<PersonsDeleterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException(nameof(personId));
            //Person? matchingPerson = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);
            //if (matchingPerson == null) return false;
            //_context.Persons.Remove(matchingPerson);
            //await _context.SaveChangesAsync();

            Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personId.Value);
            if (matchingPerson == null) return false;
            await _personsRepository.DetelePersonByPersonId(personId.Value);
            return true;
        }
    }
}
