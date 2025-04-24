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
    public class PersonsSorterService : IPersonsSorterService
    {
        //private readonly ApplicationDbContext _context;

        //public PersonService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsSorterService(IPersonsRepository personRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPersons method of PersonService");

            if (string.IsNullOrEmpty(sortBy)) return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Age).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };
            return sortedPersons;
        }

    }
}
