using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entities.
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a new Person to the database.
        /// </summary>
        /// <param name="person">Person object to add</param>
        /// <returns>Return the person object after adding it to the database</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Retrieves a Person by its ID from the database.
        /// </summary>
        /// <returns>List of person objects from the database</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Retrieves a Person by its ID from the database.
        /// </summary>
        /// <param name="personId">PersonId to search</param>
        /// <returns>Matching person object or null</returns>
        Task<Person?> GetPersonByPersonId(Guid personId);

        /// <summary>
        /// Returns all persons based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching persons with the given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a Person by its ID from the database.
        /// </summary>
        /// <param name="personId">Person Id to search</param>
        /// <returns>Returns true if deletion is successful, otherwise false</returns>
        Task<bool> DetelePersonByPersonId(Guid personId);

        /// <summary>
        /// Updates a Person in the database.
        /// </summary>
        /// <param name="person">Person object to update</param>
        /// <returns>Returns the updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
