using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Moq;
using Services;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using System.Diagnostics.Metrics;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesGetterService _countriesGetterService;

        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IFixture _fixture;
        public CountriesServiceTest()
        {
            _fixture = new Fixture();

            // Create a mock of the countries repository
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;

            // Apply the mock to the countriesService
            _countriesAdderService = new CountriesAdderService(_countriesRepository);
            _countriesGetterService = new CountriesGetterService(_countriesRepository);
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Func<Task> action = (async () =>
            {
                //Act
                await _countriesAdderService.AddCountry(request);
            });
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest request = _fixture.Build<CountryAddRequest>()
                .With(c => c.CountryName, null as string)
                .Create();
            Country country = request.ToCountry();
            _countriesRepositoryMock.Setup(c => c.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);

            //Assert
            Func<Task> action = (async () =>
            {
                //Act
                await _countriesAdderService.AddCountry(request);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = _fixture.Build<CountryAddRequest>()
                .With(c => c.CountryName, "USA")
                .Create();
            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
                .With(c => c.CountryName, "USA")
                .Create();

            Country country1 = request1.ToCountry();
            Country country2 = request2.ToCountry();

            //Assert
            Func<Task> action = (async () =>
            {
                //Act
                _countriesRepositoryMock.Setup(c => c.AddCountry(It.IsAny<Country>())).ReturnsAsync(country1);
                _countriesRepositoryMock.Setup(c => c.GetCountryByName(It.IsAny<string>())).ReturnsAsync(country1);
                await _countriesAdderService.AddCountry(request2);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply proper country name, it should insert(add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Create<CountryAddRequest>();
            Country country = request.ToCountry();
            CountryResponse countryResponse = country.ToCountryResponse();
            _countriesRepositoryMock.Setup(c => c.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);
            _countriesRepositoryMock.Setup(c => c.GetCountryByName(It.IsAny<string>())).ReturnsAsync(null as Country);

            //Act
            CountryResponse response = await _countriesAdderService.AddCountry(request);
            countryResponse.CountryId = response.CountryId;

            //Assert
            (response.CountryId != Guid.Empty).Should().BeTrue();
            countryResponse.Should().Be(response);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            _countriesRepositoryMock.Setup(c => c.GetAllCountries())
                .ReturnsAsync(new List<Country>());
            //Act
            List<CountryResponse> actual_country_response_list = await _countriesGetterService.GetAllCountries();

            //Assert
            actual_country_response_list.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<Country> country_list = new List<Country>()
            {
                _fixture.Build<Country>()
                .With(c => c.Persons, null as List<Person>)
                .Create(),
                _fixture.Build<Country>()
                .With(c => c.Persons, null as List<Person>)
                .Create(),
            };
            List<CountryResponse> country_expected_response_list = country_list.Select(c => c.ToCountryResponse()).ToList();
            _countriesRepositoryMock.Setup(c => c.GetAllCountries()).ReturnsAsync(country_list);

            //Act
            List<CountryResponse> actualCountryResponseList = await _countriesGetterService.GetAllCountries();

            //Assert
            actualCountryResponseList.Should().BeEquivalentTo(country_expected_response_list);
        }
        #endregion

        #region GetCountryByCountryId

        //If we supply null as CountryId, it should return null as CountryResponse
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            _countriesRepositoryMock.Setup(c => c.GetCountryById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Country);

            //Act
            CountryResponse? country_response_from_get_method = await _countriesGetterService.GetCountryByCountryId(countryId);

            //Assert
            country_response_from_get_method.Should().BeNull();
        }

        [Fact]
        //If we supply a valid CountryId, it should return the matching country details as CountryResponse object
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            //Arrange
            Country country = _fixture.Build<Country>()
                .With(c => c.Persons, null as List<Person>)
                .Create();
            CountryResponse country_response_from_add = country.ToCountryResponse();
            _countriesRepositoryMock.Setup(c => c.GetCountryById(It.IsAny<Guid>()))
                .ReturnsAsync(country);

            //Act
            CountryResponse? country_response_from_get = await _countriesGetterService.GetCountryByCountryId(country_response_from_add.CountryId);

            //Assert
            country_response_from_get.Should().Be(country_response_from_add);
        }

        #endregion
    }
}
