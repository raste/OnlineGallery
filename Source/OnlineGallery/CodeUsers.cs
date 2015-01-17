﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Linq;

namespace OnlineGallery
{
    public class CodeUsers
    {
        
        public void AddUser(Entities objectContext, User newUser)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (newUser == null)
            {
                throw new NullReferenceException("newUser");
            }

            objectContext.AddToUserSet(newUser);
            CodeTools.Save(objectContext);
        }

        public User GetUser(Entities objectContext, long userID, bool excIfNull)
        {
            CodeTools.CheckObjectContext(objectContext);

            User currUser = objectContext.UserSet.FirstOrDefault(usr => usr.ID == userID);

            if (excIfNull == true && currUser == null)
            {
                throw new Exception("no user with id = " + userID);
            }

            return currUser;
        }

        public long Login(Entities objectContext, String name, String password)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name is null or empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("password is null or empty");
            }

            CodeTools.CheckObjectContext(objectContext);

            User user = objectContext.UserSet.FirstOrDefault<User>
                (usr => usr.username == name && usr.password == password);

            long userid = -1;

            if (user != null)
            {
                userid = user.ID;
            }

            return userid;
        }

        public bool IsUserNameTaken(Entities objectContext, String name)
        {
            CodeTools.CheckObjectContext(objectContext);
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            bool result = false;

            User user = objectContext.UserSet.FirstOrDefault(usr => usr.username == name);
            if (user != null)
            {
                result = true;
            }

            return result;
        }






    }
}
