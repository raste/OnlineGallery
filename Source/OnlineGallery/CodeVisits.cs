﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Linq;

namespace OnlineGallery
{
    public class CodeVisits
    {

        public void AddVisit(Entities objectContext, Image image, User user)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new NullReferenceException("image");
            }

            if (user == null)
            {
                throw new NullReferenceException("user");
            }

            ImageVisit newVisit = new ImageVisit();

            newVisit.User = user;
            newVisit.Image = image;

            objectContext.AddToImageVisitSet(newVisit);    
            CodeTools.Save(objectContext);

            image.visits++;
            CodeTools.Save(objectContext);
        }


        public bool IsUserVisitngForFirstTime(Entities objectContext, Image image, User user)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new NullReferenceException("image");
            }

            if (user == null)
            {
                throw new NullReferenceException("user");
            }

            bool firstTime = false;

            ImageVisit visit = objectContext.ImageVisitSet.FirstOrDefault
                (vis => vis.Image.ID == image.ID && vis.User.ID == user.ID);

            if (visit == null)
            {
                firstTime = true;
            }

            return firstTime;
        }










    }
}
