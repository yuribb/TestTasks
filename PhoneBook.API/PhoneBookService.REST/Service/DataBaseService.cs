using PhoneBookService.REST.Context;
using PhoneBookService.REST.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PhoneBookService.REST.Service
{
    public class DataBaseService
    {
        public List<Contact> GetContacts(string firstName = null, string secondName = null, DateTime? birthDate = null)
        {
            List<Contact> result;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                IQueryable<Contact> query = context.Contacts;
                if (firstName != null)
                {
                    query = query.Where(c => c.FirstName == firstName);
                }

                if (secondName != null)
                {
                    query = query.Where(c => c.SecondName == firstName);
                }
                if (birthDate.HasValue)
                {
                    query = query.Where(c => c.BirthDate == birthDate.Value);
                }
                result = query.ToList();
            }
            return result;
        }

        public Contact GetContact(int id)
        {
            if (id <= 0) return null;
            Contact contact = null;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                contact = context.Contacts.FirstOrDefault(c => c.Id == id);
            }
            return contact;
        }


        public Contact AddContact(Contact contact)
        {
            if (contact == null) return null;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                context.Contacts.Add(contact);
                context.SaveChanges();
            }
            return contact;
        }

        public Contact UpdateContact(Contact contact)
        {
            if (contact == null) return null;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                Contact updatingContact = context.Contacts.FirstOrDefault(c => c.Id == contact.Id);
                if (updatingContact == null)
                {
                    throw new Exception("Contact not found");
                }

                context.Contacts.AddOrUpdate(contact);
                context.SaveChanges();
            }
            return contact;
        }

        public void DeleteContact(int id)
        {
            if (id <= 0) return;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                Contact contact = context.Contacts.FirstOrDefault(c => c.Id == id);
                if (contact != null)
                {
                    context.Contacts.Remove(contact);
                    context.SaveChanges();
                }
            }
        }

        public List<Email> GetEmails(int contactId)
        {
            List<Email> result;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                IQueryable<Email> query = context.Emails.Where(e => e.Contact.Id == contactId);
                result = query.ToList();
            }
            return result;
        }

        public Email GetEmail(int emailId)
        {
            Email result;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                result = context.Emails.FirstOrDefault(e => e.Id == emailId);
            }
            return result;
        }

        public Email AddEmail(int contactId, Email email)
        {
            if (email == null) return null;
            if (contactId <= 0) return null;

            using (PhoneBookContext context = new PhoneBookContext())
            {
                Contact contact = context.Contacts.FirstOrDefault(c => c.Id == contactId);
                if (contact != null)
                {
                    email.Contact = contact;
                    context.Emails.Add(email);
                    context.SaveChanges();
                }
            }
            return email;
        }

        public void DeleteEmail(int emailId)
        {
            if (emailId <= 0) return;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                var email = context.Emails.FirstOrDefault(e => e.Id == emailId);
                if (email != null)
                {
                    context.Emails.Remove(email);
                    context.SaveChanges();
                }
            }
        }

        public Email UpdateEmail(Email email)
        {
            using (PhoneBookContext context = new PhoneBookContext())
            {
                Email existsEmail = context.Emails.FirstOrDefault(e => e.Id == email.Id);
                if (existsEmail != null)
                {
                    existsEmail.Address = email.Address;
                    existsEmail.ItemType = email.ItemType;
                    context.SaveChanges();
                }
            }
            return email;
        }

        public List<Phone> GetPhones(int contactId)
        {
            List<Phone> result;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                IQueryable<Phone> query = context.Phones.Where(e => e.Contact.Id == contactId);
                result = query.ToList();
            }
            return result;
        }

        public Phone GetPhone(int phoneId)
        {
            Phone result;
            using (PhoneBookContext context = new PhoneBookContext())
            {
                result = context.Phones.FirstOrDefault(e => e.Id == phoneId);
            }
            return result;
        }

        public Phone AddPhone(int contactId, Phone phone)
        {
            if (phone == null) return null;
            if (contactId <= 0) return null;

            using (PhoneBookContext context = new PhoneBookContext())
            {
                Contact contact = context.Contacts.FirstOrDefault(c => c.Id == contactId);
                if (contact != null)
                {
                    phone.Contact = contact;
                    context.Phones.Add(phone);
                    context.SaveChanges();
                }
            }
            return phone;
        }

        public void DeletePhone(int phoneId)
        {
            using (PhoneBookContext context = new PhoneBookContext())
            {
                var phone = context.Phones.FirstOrDefault(e => e.Id == phoneId);
                if (phone != null)
                {
                    context.Phones.Remove(phone);
                    context.SaveChanges();
                }
            }
        }

        public Phone UpdatePhone(Phone phone)
        {
            using (PhoneBookContext context = new PhoneBookContext())
            {
                Phone existsPhone = context.Phones.FirstOrDefault(e => e.Id == phone.Id);
                if (existsPhone != null)
                {
                    existsPhone.Number = phone.Number;
                    existsPhone.ItemType = phone.ItemType;
                    context.SaveChanges();
                }
            }
            return phone;
        }
    }
}