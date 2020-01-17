using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using opg_201910_interview.Models;

namespace opg_201910_interview.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOptions<List<ClientSettingsModel>> _clientSettings;
        public OrderController(IOptions<List<ClientSettingsModel>> clientSettings)
        {
            _clientSettings = clientSettings;
        }
        public IActionResult Index() 
        {
            List<ClientSettingsModel> _clients = new List<ClientSettingsModel>();
            
            _clientSettings.Value.ForEach(x => {
                ClientSettingsModel client = new ClientSettingsModel { 
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    FileDirectoryPath = x.FileDirectoryPath,
                    Delimiter = x.Delimiter,
                    DateFormat = x.DateFormat,
                    ValidItems = x.ValidItems
                };

                _clients.Add(client);
                //List<Order> orders = new List<Order>();
                //orders = GetXMLFiles(x);
                //orders = FilterAndSortOrders(orders, x.ValidItems);
                //foreach (var p in orders)
                //{
                //    Console.WriteLine(p.XMLFileName);
                //}
                ViewBag.Order = GetClientOrder(client);


            });
            return View(_clients);
        }

        public List<Order> FilterAndSortOrders(List<Order> tempOrder, string ValidItems)
        {
            List<Order> orders = new List<Order>();
            string[] validItemArr = ValidItems.Split(',');
            foreach(string s in validItemArr)
            {  
                foreach (var p in tempOrder.Where(p => (p.ItemName == s.Trim())).OrderBy(p => p.DateOrder))
                {
                    Order _order = new Order { 
                        ClientId = p.ClientId,
                        ItemName = p.ItemName,
                        DateOrder = p.DateOrder,
                        XMLFileName = p.XMLFileName
                    };

                    orders.Add(_order);
                }
            }
            return orders;
        }

        public List<Order> GetXMLFiles(ClientSettingsModel client)
        {
            List<Order> tempOrder = new List<Order>();
            var path = client.FileDirectoryPath;
            string fileName = "";
            string itemName = "";
            string dateOrder = "";
            string[] format = { client.DateFormat };

            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                FileInfo iFile = new FileInfo(file);
                fileName = Path.GetFileNameWithoutExtension( iFile.Name);
                //To check if file has valid format
                if (fileName.IndexOf(client.Delimiter) > 0)
                {
                    string[] arrFile = fileName.Split(client.Delimiter, 2);
                    itemName = arrFile[0];
                    dateOrder = arrFile[1];
                    DateTime date;

                    if (DateTime.TryParseExact(dateOrder,
                                               format,
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               System.Globalization.DateTimeStyles.None,
                                               out date))
                    {
                        //If the order date is valid
                        Order order = new Order
                        {
                            ClientId = client.ClientId,
                            ItemName = itemName,
                            DateOrder = date.Date,
                            XMLFileName = fileName
                        };

                        tempOrder.Add(order);
                    }
                }
            }
            return tempOrder;
        }

        public JsonResult GetClientOrder(ClientSettingsModel client)
        {
            List<Order> orders = new List<Order>();
            orders = GetXMLFiles(client);
            orders = FilterAndSortOrders(orders, client.ValidItems);
            return Json(orders);
        }

    }
}