using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Person entity
    /// </summary>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add as PersonAddRequest</param>
        /// <returns>Returns the same person details with newly generated PersonID, as PersonResponse</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    }
}
