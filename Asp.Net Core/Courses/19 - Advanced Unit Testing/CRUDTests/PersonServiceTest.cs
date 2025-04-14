using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions; // More readable assertions
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonsService _personService;

        // Mock is to create a fake object of IPersonRepository, which contains the fake methods that will return single value set by user
        private readonly Mock<IPersonRepository> _personRepositoryMock; // Mock methods into IPersonRepository
        private readonly IPersonRepository _personRepository; // Mocked IPersonRepository object

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            // Initialize the AutoFixture, to create dummy objects
            _fixture = new Fixture();

            _personRepositoryMock = new Mock<IPersonRepository>();
            _personRepository = _personRepositoryMock.Object;

            _personService = new PersonService(_personRepository);
            _testOutputHelper = testOutputHelper;
        }

        #region  AddPerson

        //when supplying null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
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
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(p => p.PersonName, null as string)
                .Create();

            Person person = personAddRequest.ToPerson();
            // When PersonRepository.AddPerson is called, it has to return the same person object
            _personRepositoryMock.Setup(p => p.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

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
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "someone@example.com") // to replace the randomized email which failed the test
                .Create() ;

            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();
            // if we supply any argument value to the AddPerson method, it should return the same return value
            _personRepositoryMock.Setup(p => p.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act
            PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);
            person_response_expected.PersonId = person_response_from_add.PersonId;

            //Assert
            person_response_from_add.PersonId.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_response_expected);
        }
        #endregion

        #region GetPersonByPersonId

        //if we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId_ToBeNull()
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
        public async Task GetPersonById_ValidPersonId_ToBeSuccessful()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(p => p.Email, "someone@example.com")
                .With(p => p.Country, null as Country)
                .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();
            _personRepositoryMock.Setup(p => p.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonId(person.PersonId);

            //Assert
            person_response_from_get.Should().Be(person_response_expected);
        }

        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_ToBeEmptyList()
        {
            // Arrange
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(new List<Person>());

            //Act
            List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

            //Assert
            persons_from_get.Should().BeEmpty();
        }

        //First, we add three persons into the list, then we call GetAllPersons, it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessfull()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(p => p.Email, "someone1@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone2@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone3@example.com")
                .With(p => p.Country, null as Country)
                .Create()
            };
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(persons);


            List<PersonResponse> person_response_list_expected = persons.Select(p => p.ToPersonResponse()).ToList();

            //print person_response_list_expected (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach(PersonResponse person_response_from_add in person_response_list_expected)
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
            person_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion

        #region GetFilteredPersons

        //if the serach text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(p => p.Email, "someone1@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone2@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone3@example.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(p => p.ToPersonResponse()).ToList();

            //print person_response_list_expected (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            _personRepositoryMock.Setup(p => p.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            List<PersonResponse> person_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            person_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        //First we add a few persons, then we search based on the person name and search string. It should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(p => p.Email, "someone1@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone2@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone3@example.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(p => p.ToPersonResponse()).ToList();

            //print person_response_list_expected (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            _personRepositoryMock.Setup(p => p.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            List<PersonResponse> person_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            //print Actual
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            person_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return person list in descending order on PersonName
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(p => p.Email, "someone1@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone2@example.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.Email, "someone3@example.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(p => p.ToPersonResponse()).ToList();

            //print person_response_list_expected (Expected)
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(persons);
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
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
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
        public async Task UpdatePerson_InvalidPersonId_ToBeArgumentException()
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
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(p => p.Country, null as Country )
                .With(p => p.PersonName, null as string)
                .With(p => p.Email, "someone@example.com")
                .With(p => p.Gender, GenderOptions.Male.ToString())
                .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();

            PersonUpdateRequest? personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();

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
        public async Task UpdatePerson_PersonFullDetails_ToBeSuccessful()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(p => p.Country, null as Country)
                .With(p => p.Email, "someone@example.com")
                .With(p => p.Gender, GenderOptions.Male.ToString())
                .Create();
            PersonResponse person_response_expected = person.ToPersonResponse();

            PersonUpdateRequest? personUpdateRequest = person_response_expected.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(p => p.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);
            _personRepositoryMock.Setup(p => p.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(personUpdateRequest);

            //Assert
            person_response_from_update.Should().Be(person_response_expected);
        }

        #endregion

        #region DeletePerson

        //If we supply valid PersonId, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonId_ToBeSuccessful()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(p => p.Country, null as Country)
                .With(p => p.Email, "someone@example.com")
                .With(p => p.Gender, GenderOptions.Male.ToString())
                .Create();

            //Act
            _personRepositoryMock.Setup(p => p.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);
            _personRepositoryMock.Setup(p => p.DetelePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(true);
            bool isDeleted = await _personService.DeletePerson(person.PersonId);

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
