﻿using Entities;
using ServiceContracts.Enums;
using System;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents a DTO class that is used as return type of most methods of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country {  get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare</param>
        /// <returns>True or false, indicating whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse person = (PersonResponse)obj;
            return PersonId == person.PersonId && 
                PersonName == person.PersonName && 
                Email == person.Email && 
                DateOfBirth == person.DateOfBirth && 
                Gender == person.Gender &&
                CountryId == person.CountryId && 
                Address == person.Address && 
                ReceiveNewsLetters == person.ReceiveNewsLetters;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person Id: {PersonId}, " +
                $"Person Name: {PersonName}, " +
                $"Email: {Email}, " +
                $"Date Of Birth: {DateOfBirth}, " +
                $"Gender: {Gender}, " +
                $"Country Id: {CountryId}, " +
                $"Country Name: {Country}, " +
                $"Address: {Address}, " +
                $"Received News Letters: {ReceiveNewsLetters}, " +
                $"Age: {Age}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Address = Address,
                CountryId = CountryId,
                ReceiveNewsLetters = ReceiveNewsLetters,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
            };
        }
    }

    public static class PersonExtension
    {
        /// <summary>
        /// An extension method to convert an object of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">The person object to convert</param>
        /// <returns>Returns the converted PersonResponse object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName,
            };
        }
    }
}
