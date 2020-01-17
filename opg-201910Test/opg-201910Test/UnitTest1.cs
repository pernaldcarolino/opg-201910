using System;
using Xunit;
using opg_201910_interview.Models;
using opg_201910_interview.Controllers;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace opg_201910Test
{
    public class UnitTest1
    {

        private readonly IOptions<List<ClientSettingsModel>> _clientSettings;
        
        public UnitTest1(IOptions<List<ClientSettingsModel>> clientSettings)
        {
            _clientSettings = clientSettings;
        }
        
        [Theory(DisplayName = "Show Client Orders")]
        [InlineData("{ 'Order': " +
                    " { 'ClientId': '1001', " +
                    "   'ItemName': 'shovel', " +
                    "   'DateOrder': '2000-01-01', " +
                    "   'XMLFileName': 'shovel-2000-01-01' " +
                    " }," +
                    " { 'ClientId': '1001', " +
                    "   'ItemName': 'waghor', " +
                    "   'DateOrder': '2012-06-20', " +
                    "   'XMLFileName': 'waghor-2012-06-20' " +
                    " }," +
                    " { 'ClientId': '1001', " +
                    "   'ItemName': 'blaze', " +
                    "   'DateOrder': '2018-05-01', " +
                    "   'XMLFileName': 'blaze-2018-05-01' " +
                    " }," +
                    " { 'ClientId': '1001', " +
                    "   'ItemName': 'blaze', " +
                    "   'DateOrder': '2019-01-23', " +
                    "   'XMLFileName': 'blaze-2019-01-23' " +
                    " }," +
                    " { 'ClientId': '1001', " +
                    "   'ItemName': 'discus', " +
                    "   'DateOrder': '2015-12-16', " +
                    "   'XMLFileName': 'discus-2015-12-16' " +
                    " }" +
                    "}")]
        public void TestGetClientOrder(string json)
        {
            ClientSettingsModel client1 = new ClientSettingsModel
            {
                ClientId = 1001,
                ClientName = "Client A",
                FileDirectoryPath = "",
                Delimiter = '-',
                DateFormat = "yyyy-MM-dd",
                ValidItems = ""
            };

            var expected = JsonConvert.DeserializeObject(json);
            // 1. Arrange
            var cs = new OrderController(_clientSettings);

            // 2. Act 
            var result = cs.GetClientOrder(client1);

            // 3. Assert 
            Assert.Equal(expected, result);
        }
    }
}
