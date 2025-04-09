using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add as PersonAddRequest</param>
        /// <returns>Returns the same person details with newly generated PersonID, as PersonResponse</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();
        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personId">Person id to search</param>
        /// <returns>Returns the matching person object as PersonResponse</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);
        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Represents variable name to search</param>
        /// <param name="searchString">Represents actual value to search</param>
        /// <returns>Returns all matching persons as list of person response</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString);
        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        /// <summary>
        /// Update the specified person details based on the given person update request
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update including person id</param>
        /// <returns>Returns the person response object after updating</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        /// <summary>
        /// Deletes the person based on the given person id
        /// </summary>
        /// <param name="personId">Person Id to delete</param>
        /// <returns>Returns true if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Guid? personId);
        /// <summary>
        /// Returns persons as CSV file
        /// </summary>
        /// <returns>Returns the memory stream with CSV data</returns>
        Task<MemoryStream> GetPersonsCSV();
        /// <summary>
        /// Returns persons as Excel file
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of persons</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
