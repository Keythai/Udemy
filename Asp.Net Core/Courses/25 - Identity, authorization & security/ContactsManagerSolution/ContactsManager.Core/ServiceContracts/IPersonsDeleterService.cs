using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Person entity
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes the person based on the given person id
        /// </summary>
        /// <param name="personId">Person Id to delete</param>
        /// <returns>Returns true if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Guid? personId);
        
    }
}
