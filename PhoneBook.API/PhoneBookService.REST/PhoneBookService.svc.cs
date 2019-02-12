using Newtonsoft.Json;
using PhoneBookService.REST.Models;
using PhoneBookService.REST.Service;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace PhoneBookService.REST
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PhoneBookService
    {
        public const string EMPTY_JSON = "{}";
        public const string EMPTY_JSON_ARR = "[]";

        DataBaseService service = new DataBaseService();
        public PhoneBookService() { }

        [WebGet(UriTemplate = "/Contacts?firstName={firstName}&secondName={secondName}&birthDate={birthDate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public string GetContacts(string firstName = null, string secondName = null, string birthDate = null)
        {
            DateTime? date = null;
            if (birthDate != null)
            {

                bool isParsed = DateTime.TryParse(birthDate, out DateTime dateValue);
                if (isParsed)
                {
                    date = dateValue;
                }
            }
            var result = service.GetContacts(firstName, secondName, date);
            if (result == null || !result.Any()) return EMPTY_JSON_ARR;
            return JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebGet(UriTemplate = "/Contacts/{contactId}", ResponseFormat = WebMessageFormat.Json)]
        public string GetContact(string contactId)
        {
            bool isParsed = int.TryParse(contactId, out int id);
            if (!isParsed) return EMPTY_JSON;
            if (id <= 0) return EMPTY_JSON;
            var contact = service.GetContact(id);
            if (contact == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(contact, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "POST", UriTemplate = "/Contacts", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string AddContact(Contact contact)
        {
            contact = service.AddContact(contact);
            if (contact == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(contact, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "PUT", UriTemplate = "/Contacts", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string UpdateContact(Contact contact)
        {
            contact = service.UpdateContact(contact);
            if (contact == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(contact, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "DELETE", UriTemplate = "/Contacts/{contactId}")]
        public void DeleteContact(string contactId)
        {
            bool isParsed = int.TryParse(contactId, out int id);
            if (!isParsed) return;
            if (id <= 0) return;
            service.DeleteContact(id);
        }

        [WebGet(UriTemplate = "Contacts/{contactId}/Emails")]
        public string GetEmails(string contactId)
        {
            bool isParsed = int.TryParse(contactId, out int id);
            if (!isParsed) return EMPTY_JSON_ARR;
            if (id <= 0) return EMPTY_JSON_ARR;

            var result = service.GetEmails(id);
            if (result == null || !result.Any()) return EMPTY_JSON_ARR;
            return JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebGet(UriTemplate = "Contacts/{contactId}/Emails/{emailId}", ResponseFormat = WebMessageFormat.Json)]
        public string GetEmail(string contactId, string emailId)
        {
            bool isParsed = int.TryParse(emailId, out int id);
            if (!isParsed) return EMPTY_JSON;
            if (id <= 0) return EMPTY_JSON;

            var result = service.GetEmail(id);
            if (result == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "POST", UriTemplate = "/Contacts/{contactId}/Emails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string AddEmail(string contactId, Email email)
        {
            if (email == null) return EMPTY_JSON;
            bool isParsed = int.TryParse(contactId, out int id);
            if (!isParsed) return EMPTY_JSON;
            if (id <= 0) return EMPTY_JSON;

            email = service.AddEmail(id, email);
            if (email == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(email, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "DELETE", UriTemplate = "Contacts/{contactId}/Emails/{emailId}")]
        public void DeleteEmail(string contactId, string emailId)
        {
            bool isParsed = int.TryParse(emailId, out int id);
            if (!isParsed) return;
            if (id <= 0) return;
            service.DeleteEmail(id);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "/Contacts/{contactId}/Emails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string UpdateEmail(string contactId, Email email)
        {
            if (email == null) return EMPTY_JSON;

            email = service.UpdateEmail(email);
            if (email == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(email, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebGet(UriTemplate = "Contacts/{contactId}/Phones", ResponseFormat = WebMessageFormat.Json)]
        public string GetPhones(string contactId)
        {
            bool isParsed = int.TryParse(contactId, out int id);
            if (!isParsed) return EMPTY_JSON_ARR;
            if (id <= 0) return EMPTY_JSON_ARR;

            var result = service.GetPhones(id);
            if (result == null || !result.Any()) return EMPTY_JSON_ARR;
            return JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebGet(UriTemplate = "Contacts/{contactId}/Phones/{phoneId}", ResponseFormat = WebMessageFormat.Json)]
        public string GetPhone(string contactId, string phoneId)
        {
            bool isParsed = int.TryParse(phoneId, out int id);
            if (!isParsed) return EMPTY_JSON;
            if (id <= 0) return EMPTY_JSON;

            var result = service.GetPhone(id);
            if (result == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "POST", UriTemplate = "/Contacts/{contactId}/Phones", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string AddPhone(string contactId, Phone phone)
        {
            if (phone == null) return EMPTY_JSON;
            bool isParsed = int.TryParse(contactId, out int id);
            if (!isParsed) return EMPTY_JSON;
            if (id <= 0) return EMPTY_JSON;

            phone = service.AddPhone(id, phone);
            if (phone == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(phone, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [WebInvoke(Method = "DELETE", UriTemplate = "Contacts/{contactId}/Phones/{phoneId}")]
        public void DeletePhone(string contactId, string phoneId)
        {
            bool isParsed = int.TryParse(phoneId, out int id);
            if (!isParsed) return;
            if (id <= 0) return;
            service.DeletePhone(id);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "/Contacts/{contactId}/Phones", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string UpdatePhone(Phone phone)
        {
            if (phone == null) return EMPTY_JSON;
            phone = service.UpdatePhone(phone);
            if (phone == null) return EMPTY_JSON;
            return JsonConvert.SerializeObject(phone, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}