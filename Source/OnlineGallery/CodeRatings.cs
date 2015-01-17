﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Linq;

namespace OnlineGallery
{
    public class CodeRatings
    {

        public void RateImage(Entities objectContext, Image image, User user, int rating)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            if (rating != -1 && rating != 1)
            {
                throw new ArgumentOutOfRangeException("rating");
            }

            ImageRating newRating = new ImageRating();

            newRating.User = user;
            newRating.Image = image;
            newRating.rating = rating;

            objectContext.AddToImageRatingSet(newRating);
            CodeTools.Save(objectContext);

            if (rating == -1)
            {
                image.disliked++;
            }
            else
            {
                image.liked++;
            }
            CodeTools.Save(objectContext);
        }


        public bool CanUserRateImate(Entities objectContext, Image image, User user)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            bool canRate = false;

            ImageRating rate = objectContext.ImageRatingSet.FirstOrDefault
                (rat => rat.Image.ID == image.ID && rat.User.ID == user.ID);

            if (rate == null)
            {
                canRate = true;
            }

            return canRate;
        }











    }
}
