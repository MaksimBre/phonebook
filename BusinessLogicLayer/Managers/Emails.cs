﻿using System;
using System.Collections.Generic;
using Phonebook.BusinessLogicLayer.Managers.Properties;
using Phonebook.BusinessLogicLayer.Models;
using System.Linq;
using System.Diagnostics;

namespace Phonebook.BusinessLogicLayer.Managers
{
   public class Emails
    {
        public IEnumerable<Email> GetAllByContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException("email", "Valid email is mandatory!");

            using (DataAccessLayer.DBAccess.Phonebook phonebook = new DataAccessLayer.DBAccess.Phonebook(Settings.Default.PhonebookDBConnection))
            {
                return phonebook.Emails.GetAllByContactId(contact.Id).Select(pt => Map(pt, contact));
            }
        }

        public int Add(Email email)
        {
            using (DataAccessLayer.DBAccess.Phonebook phonebook = new DataAccessLayer.DBAccess.Phonebook(Settings.Default.PhonebookDBConnection))
            {
                return phonebook.Emails.Insert(Map(email));
            }
        }

        public void Save(Email email)
        {
            using (DataAccessLayer.DBAccess.Phonebook phonebook = new DataAccessLayer.DBAccess.Phonebook(Settings.Default.PhonebookDBConnection))
            {
                phonebook.Emails.Update(Map(email));
            }
        }

        public void Delete(int id)
        {
            using (DataAccessLayer.DBAccess.Phonebook phonebook = new DataAccessLayer.DBAccess.Phonebook(Settings.Default.PhonebookDBConnection))
            {
                phonebook.Emails.Delete(id);
            }
        }

        private Email Map(DataAccessLayer.Models.Email dbEmail, Contact contact)
        {
            if (Equals(dbEmail, null))
                return null;
            Debug.Assert(dbEmail.ContactId == contact.Id);

            Email email = new Email(contact, dbEmail.EmailAddress, dbEmail.TypeId.HasValue ? new EmailTypes().GetById(dbEmail.TypeId.Value) : null)
            {
                Id = dbEmail.Id
            };

            return email;
        }
        private DataAccessLayer.Models.Email Map(Email email)
        {
            if (Equals(email, null))
                throw new ArgumentNullException("email", "Valid email is mandatory!");

            return new DataAccessLayer.Models.Email(email.Id, email.Contact.Id, email.EmailAddress, email.EmailType?.Id);

           
        }
    }
}
