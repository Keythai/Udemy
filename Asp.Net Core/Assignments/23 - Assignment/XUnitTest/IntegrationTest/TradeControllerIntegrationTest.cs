using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.IntegrationTest
{
    /*
     "/Trade/Index/MSFT":

     When you make a HTTP GET request to the "Trade/Index/MSFT" route, it should return a view result (html response) with an HTML element that has class="price".
     */
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Index_ReturnsViewWithPriceElement()
        {
            //Act
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("/Trade/Index/MSFT");

            //Assert
            httpResponseMessage.IsSuccessStatusCode.Should().BeTrue();

            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll(".price").Should().NotBeNull();
        }
    }
}
