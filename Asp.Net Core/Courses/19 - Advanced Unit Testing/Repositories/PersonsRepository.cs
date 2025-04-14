using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _context;
        public PersonsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _context.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DetelePersonByPersonId(Guid personId)
        {
            _context.RemoveRange(_context.Persons.Where(p => p.PersonId == personId));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _context.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _context.Persons
                .Include("Country")
                .Where(predicate)
                .ToListAsync(); 
        }

        public async Task<Person?> GetPersonByPersonId(Guid personId)
        {
            return await _context.Persons
                .Include("Country")
                .FirstOrDefaultAsync(p => p.PersonId == personId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _context.Persons
                .FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
            if(matchingPerson == null)
            {
                return person;
            }
            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
            await _context.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
