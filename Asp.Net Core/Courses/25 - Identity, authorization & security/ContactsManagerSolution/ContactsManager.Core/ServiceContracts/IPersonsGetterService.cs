﻿using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Person entity
    /// </summary>
    public interface IPersonsGetterService
    {
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
