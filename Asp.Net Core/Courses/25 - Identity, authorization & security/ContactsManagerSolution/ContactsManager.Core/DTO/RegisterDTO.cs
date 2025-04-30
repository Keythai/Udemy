﻿using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name cannot be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already in use")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone cannot be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone should contain numbers only")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm Password cannot be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
