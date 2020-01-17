using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opg_201910_interview.Models
{
    public class Order
    {
        public int ClientId { get; set; }
        public string ItemName { get; set; }
        public DateTime? DateOrder { get; set; }
        public string XMLFileName { get; set; }
    }

    public class ClientSettingsModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string FileDirectoryPath { get; set; }
        public char Delimiter { get; set; }
        public string DateFormat { get; set; }
        public string ValidItems { get; set; }
    }
}
