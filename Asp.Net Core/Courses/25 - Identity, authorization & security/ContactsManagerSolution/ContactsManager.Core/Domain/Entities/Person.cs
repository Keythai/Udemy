﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// Person domain class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        [StringLength(40)]
        public string? PersonName { get; set; }
        [StringLength(40)]
        public string? Email {  get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public string? TIN { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }

        // to make sure the seq can print the readable person class details
        public override string ToString()
        {
            return $"PersonId: {PersonId}," +
                $"Email: {Email}," +
                $"Date of Birth: {DateOfBirth?.ToString("MM/dd/yyyy")}," +
                $"Gender: {Gender}," +
                $"CountryId: {CountryId}," +
                $"Country name: {Country}," +
                $"Address: {Address}," +
                $"Receive News Letters: {ReceiveNewsLetters}";
        }
    }
}
