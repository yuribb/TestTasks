using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhoneBookService.REST.Models
{
    [DataContract]
    public enum ItemType
    {
        Other = 0,
        Work = 1,
        Home = 2
    }
}