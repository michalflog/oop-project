﻿using OOPProject.Db;
using OOPProject.Db.Objects;
using OOPProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Models
{
    public class AdminModel : IUserModel
    {
        private ActivitiesContext db;

        public AdminModel()
        {
            db = new ActivitiesContext();
        }

        public Response<User> CreateUser(string login, string password, string name, UserType userType)
        {

            var users = db.Users.Where(u => u.Login == login);
            if (users.Count() > 0)
                return new Response<User>(
                    $"Użytkownik {login} już istenie w bazie",
                    true
                    );

            db.Users.Add(new User
            {
                Login = login,
                Password = password,
                Name = name,
                Type = UserType.Participant
            });
            db.SaveChanges();

            return new Response<User>(
                $"Uzytkownik {login} został stworzony",
                false
                );
        }

        public ObservableCollection<User> GetUsers()
        {
            db.Users.Load();
            return db.Users.Local;
        }

        public ObservableCollection<Activitie> GetActivities()
        {
            db.Activities.Load();
            return db.Activities.Local;
        }

        public ObservableCollection<ActivitieParticipant> GetActivitiesParticipants()
        {
            db.ActivitiesParticipants.Load();
            return db.ActivitiesParticipants.Local;
        }

        public Response<User> DeleteUser(string login)
        {
            var user = db.Users.Find(login);

            if (user is null)
                return new Response<User>(
                    $"Nie ma takiego użytkownika jak {login}",
                    true
                    );

            db.Users.Remove(user);
            db.SaveChanges();

            return new Response<User>(
                $"Użytkownik {login} został usunięty",
                false
                );
        }

        public Response<User> EditUser(User user, string password, string name)
        {
            var editedUser = db.Users.Find(user.Login);

            if (editedUser is null)
                return new Response<User>(
                    $"Użutkownik {user.Login} nie istnieje w bazie",
                    true
                    );

            editedUser.Password = password;
            editedUser.Name = name;

            db.SaveChanges();

            return new Response<User>(
                $"Użytkownik {user.Login} został zedytowany",
                false
                );
        }
    }
}
