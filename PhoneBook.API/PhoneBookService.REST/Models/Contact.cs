using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PhoneBookService.REST.Models
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string SecondName { get; set; }
        [DataMember]
        public string Paternal { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public List<Phone> Phones { get; set; }
        [DataMember]
        public List<Email> Emails { get; set; }
    }
}