using AutoFixture;
using Castle.Core.Logging;
using CRUDExample.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    // unit testing: white box testing, test for the implementation (code)
    public class PersonsControllerTest
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsAdderService _personsAdderService;

        private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;
        private readonly Mock<IPersonsDeleterService> _personsDeleterServiceMock;
        private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;
        private readonly Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;
        private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;

        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesUploaderService _countriesUploaderService;

        private readonly Mock<ICountriesAdderService> _countriesAdderServiceMock;
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private readonly Mock<ICountriesUploaderService> _countriesUploaderServiceMock;

        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;
        private readonly IFixture _fixture;
        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            // Create a mock of the persons repository
            _personsGetterServiceMock = new Mock<IPersonsGetterService>();
            _personsGetterService = _personsGetterServiceMock.Object;
            _personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
            _personsDeleterService = _personsDeleterServiceMock.Object;
            _personsAdderServiceMock = new Mock<IPersonsAdderService>();
            _personsAdderService = _personsAdderServiceMock.Object;
            _personsSorterServiceMock = new Mock<IPersonsSorterService>();
            _personsSorterService = _personsSorterServiceMock.Object;
            _personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();
            _personsUpdaterService = _personsUpdaterServiceMock.Object;

            _countriesAdderServiceMock = new Mock<ICountriesAdderService>();
            _countriesAdderService = _countriesAdderServiceMock.Object;
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _countriesGetterService = _countriesGetterServiceMock.Object;
            _countriesUploaderServiceMock = new Mock<ICountriesUploaderService>();
            _countriesUploaderService = _countriesUploaderServiceMock.Object;

            _loggerMock = new Mock<ILogger<PersonsController>>();
            _logger = _loggerMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonList()
        {
            // Arrange
            List<PersonResponse> person_response_list = _fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personsGetterService,
                _countriesAdderService,
                _countriesGetterService,
                _countriesUploaderService,
                _logger,
                _personsUpdaterService,
                _personsSorterService,
                _personsDeleterService,
                _personsAdderService);

            _personsGetterServiceMock.Setup(p => p.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(person_response_list);
            _personsSorterServiceMock.Setup(p => p.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(person_response_list);

            // Act
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(person_response_list);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_IfNoModelErrors_ShouldRedirectToIndex()
        {
            // Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();
            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            PersonsController personsController = new PersonsController(_personsGetterService,
                _countriesAdderService,
                _countriesGetterService,
                _countriesUploaderService,
                _logger,
                _personsUpdaterService,
                _personsSorterService,
                _personsDeleterService,
                _personsAdderService);

            _personsAdderServiceMock.Setup(p => p.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(person_response);
            _countriesGetterServiceMock.Setup(c => c.GetAllCountries()).ReturnsAsync(countries);

            // Act
            IActionResult result = await personsController.Create(person_add_request);

            // Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
