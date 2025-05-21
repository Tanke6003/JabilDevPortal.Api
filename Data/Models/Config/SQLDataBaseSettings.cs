using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JabilDevPortal.Api.Data.Models.Config
{ public class SQLDataBaseSettings
    {
        public string Server { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}