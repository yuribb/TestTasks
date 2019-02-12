using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhoneBookService.REST.Models
{
    [DataContract]
    public class Email
    {
        [DataMember]
        public int Id { get; set; }
        [Required]
        [DataMember]
        public Contact Contact { get; set; }
        [DataMember]
        public ItemType ItemType { get; set; }
        [DataMember]
        public string Address { get; set; }
    }
}