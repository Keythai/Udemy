using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions; // More readable assertions
using Microsoft.EntityFrameworkCore;
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
        private readonly IFixture _fixture;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            // Initialize the AutoFixture, to create dummy objects
            _fixture = new Fixture();

            // Initialize the mock database, to create dummy database
            var personsInitialData = new List<Person>() { };
            var countriesInitialData = new List<Country>() { };
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            // Create DbSet<Person> and DbSet<Country> for the mock database
            dbContextMock.CreateDbSetMock<Person>(c => c.Persons, personsInitialData);
            dbContextMock.CreateDbSetMock<Country>(c => c.Countries, countriesInitialData);
            ApplicationDbContext dbContext = dbContextMock.Object;

            _countriesService = new CountriesService(dbContext);
            _personService = new PersonService(dbContext);
            _testOutputHelper = testOutputHelper;
        }

        #region  AddPerson

        //when supplying null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            Func<Task> action = async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //when supplying null value as PersonName, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(p => p.PersonName, null as string)
                .Create();

            Func<Task> action = async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
        //when supplying proper person details, it should insert the person into person list;
        //and it should return an object of PersonResponse, which includes with the newly generated PersonId
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone@example.com") // to replace the randomized email which failed the test
                .Create() ;

            //Act
            PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);
            List<PersonResponse> person_list = await _personService.GetAllPersons();

            //Assert
            person_response_from_add.PersonId.Should().NotBe(Guid.Empty);
            person_list.Should().Contain(person_response_from_add);
        }
        #endregion

        #region GetPersonByPersonId

        //if we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonId(personId);

            //Assert
            person_response_from_get.Should().BeNull();
        }

        //if we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonById_ValidPersonId()
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response = await _countriesService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest person_request = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone@example.com")
                .Create();
            PersonResponse person_response = await _personService.AddPerson(person_request);
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonId(person_response.PersonId);

            //Assert
            person_response.Should().Be(person_response_from_get);
        }

        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

            //Assert
            persons_from_get.Should().BeEmpty();
        }

        //First, we add three persons into the list, then we call GetAllPersons, it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone1@example.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone2@example.com")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone3@example.com")
                .Create();
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_get = await _personService.GetAllPersons();

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_get)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            person_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }
        #endregion

        #region GetFilteredPersons

        //if the serach text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone1@example.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone2@example.com")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone3@example.com")
                .Create();
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            person_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }

        //First we add a few persons, then we search based on the person name and search string. It should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse1.CountryId)
                .With(p => p.PersonName, "Rahman")
                .With(p => p.Email, "someone1@example.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse1.CountryId)
                .With(p => p.PersonName, "Mary")
                .With(p => p.Email, "someone2@example.com")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse2.CountryId)
                .With(p => p.PersonName, "Scott")
                .With(p => p.Email, "someone3@example.com")
                .Create();
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            person_list_from_search.Should().OnlyContain(p => p.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return person list in descending order on PersonName
        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse1.CountryId)
                .With(p => p.PersonName, "Smith")
                .With(p => p.Email, "someone1@example.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse1.CountryId)
                .With(p => p.PersonName, "Mary")
                .With(p => p.Email, "someone2@example.com")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse2.CountryId)
                .With(p => p.PersonName, "Rahman")
                .With(p => p.Email, "someone3@example.com")
                .Create();
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(personResponse);
            }

            //print person_response_list_from_add (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> allPersons = await _personService.GetAllPersons();
            List<PersonResponse> person_list_from_sort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            person_list_from_sort.Should().BeInDescendingOrder(p => p.PersonName);
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Act
            Func<Task> action = (async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                .Create();
            //Assert
            Func<Task> action = (async () =>
            {
                //Act
                await _personService.UpdatePerson(personUpdateRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When person name is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(country_request);
           
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse.CountryId)
                .With(p => p.PersonName, "Rahman")
                .With(p => p.Email, "someone@example.com")
                .Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest? personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;
            //Assert
            Func<Task> action = (async () =>
            {
                //Act
                await _personService.UpdatePerson(personUpdateRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //First, add a new person, then try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse.CountryId)
                .With(p => p.PersonName, "Rahman")
                .With(p => p.Email, "someone@example.com")
                .Create();
            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest? personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonId(person_response_from_update.PersonId);

            //Assert
            person_response_from_get.Should().Be(person_response_from_update);
        }

        #endregion

        #region DeletePerson

        //If we supply valid PersonId, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonId()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(p => p.CountryId, countryResponse.CountryId)
                .With(p => p.PersonName, "Rahman")
                .With(p => p.Email, "someone@example.com")
                .Create();
            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonId);

            //Assert
            isDeleted.Should().BeTrue();
        }

        //If we supply invalid PersonId, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }

        #endregion
    }
}
