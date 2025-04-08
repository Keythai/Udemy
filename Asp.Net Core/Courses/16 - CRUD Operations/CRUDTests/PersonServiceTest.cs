using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService();
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region  AddPerson

        //when supplying null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }

        //when supplying null value as PersonName, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }
        //when supplying proper person details, it should insert the person into person list;
        //and it should return an object of PersonResponse, which includes with the newly generated PersonId
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { 
                PersonName = "Person name...",
                Email = "person@example.com",
                Address = "sample address",
                CountryId = Guid.NewGuid(),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,
            };

            //Act
            PersonResponse person_response_from_add =_personService.AddPerson(personAddRequest);
            List<PersonResponse> person_list = _personService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonId !=  Guid.Empty);
            Assert.Contains(person_response_from_add, person_list);
        }
        #endregion

        #region GetPersonByPersonId

        //if we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(person_response_from_get);
        }

        //if we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonById_ValidPersonId()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse country_response = _countriesService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "person name",
                Address = "address",
                CountryId = country_response.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "person@example.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonResponse person_response = _personService.AddPerson(person_request);
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonId(person_response.PersonId);

            //Assert
            Assert.Equal(person_response_from_get, person_response);
        }

        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

            //Assert
            Assert.Empty(persons_from_get);
        }

        //First, we add three persons into the list, then we call GetAllPersons, it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "India"
            };
            CountryResponse countryResponse1 = _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Address = "address of smith",
                DateOfBirth = DateTime.Parse("2002-05-06"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse1.CountryId
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Address = "address of mary",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false,
                CountryId = countryResponse2.CountryId
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Address = "address of rahman",
                DateOfBirth = DateTime.Parse("1999-03-03"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse2.CountryId
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_get = _personService.GetAllPersons();

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_get)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, person_list_from_get);
            }
        }
        #endregion

        #region GetFilteredPersons

        //if the serach text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "India"
            };
            CountryResponse countryResponse1 = _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Address = "address of smith",
                DateOfBirth = DateTime.Parse("2002-05-06"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse1.CountryId
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Address = "address of mary",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false,
                CountryId = countryResponse2.CountryId
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Address = "address of rahman",
                DateOfBirth = DateTime.Parse("1999-03-03"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse2.CountryId
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, person_list_from_search);
            }
        }

        //First we add a few persons, then we search based on the person name and search string. It should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "India"
            };
            CountryResponse countryResponse1 = _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Address = "address of smith",
                DateOfBirth = DateTime.Parse("2002-05-06"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse1.CountryId
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Address = "address of mary",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false,
                CountryId = countryResponse2.CountryId
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Address = "address of rahman",
                DateOfBirth = DateTime.Parse("1999-03-03"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse2.CountryId
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                if (person_response_from_add.PersonName != null)
                {
                    if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, person_list_from_search);
                    }
                }
            }
        }

        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return person list in descending order on PersonName
        [Fact]
        public void GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "India"
            };
            CountryResponse countryResponse1 = _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Address = "address of smith",
                DateOfBirth = DateTime.Parse("2002-05-06"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse1.CountryId
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Address = "address of mary",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false,
                CountryId = countryResponse2.CountryId
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Address = "address of rahman",
                DateOfBirth = DateTime.Parse("1999-03-03"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = countryResponse2.CountryId
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> allPersons = _personService.GetAllPersons();
            List<PersonResponse> person_list_from_sort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            for(int i = 0; i < person_response_list_from_add.Count(); i++)
            {
                Assert.Equal(person_list_from_sort[i], person_response_list_from_add[i]);
            }
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid()
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //When person name is null, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse country_response = _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = country_response.CountryId,
                Email = "john@example.com",
                Address = "address",
                Gender = GenderOptions.Male
            };
            PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);

            PersonUpdateRequest? personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //First, add a new person, then try to update the person name and email
        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse country_response = _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = country_response.CountryId,
                Address = "address",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "abc@example.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);

            PersonUpdateRequest? personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonId(person_response_from_update.PersonId);

            //Assert
            Assert.Equal(person_response_from_update, person_response_from_get);
        }

        #endregion

        #region DeletePerson

        //If we supply valid PersonId, it should return true
        [Fact]
        public void DeletePerson_ValidPersonId()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse country_respoonse = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "Jones",
                Address = "address",
                CountryId = country_respoonse.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "jones@example.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);

            //Act
            bool isDeleted = _personService.DeletePerson(person_response_from_add.PersonId);

            //Assert
            Assert.True(isDeleted);
        }

        //If we supply invalid PersonId, it should return false
        [Fact]
        public void DeletePerson_InvalidPersonId()
        {
            //Act
            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}
