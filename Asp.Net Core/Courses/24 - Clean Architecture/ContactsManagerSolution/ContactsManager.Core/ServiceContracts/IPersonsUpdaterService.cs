using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Person entity
    /// </summary>
    public interface IPersonsUpdaterService
    {
         /// <summary>
        /// Update the specified person details based on the given person update request
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update including person id</param>
        /// <returns>Returns the person response object after updating</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        
    }
}
