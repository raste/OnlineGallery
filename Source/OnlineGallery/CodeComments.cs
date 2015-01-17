﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGallery
{
    public class CodeComments
    {

        public void AddComment(Entities objectContext, ImageComment newComment)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (newComment == null)
            {
                throw new ArgumentNullException("newComment");
            }

            objectContext.AddToImageCommentSet(newComment);
            CodeTools.Save(objectContext);

            if (!newComment.ImageReference.IsLoaded)
            {
                newComment.ImageReference.Load();
            }

            Image commImage = newComment.Image;
            commImage.commens++;
            CodeTools.Save(objectContext);
        }

        public int CountUserCommentsOnImage(Entities objectContext, Image image, User user)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int count = 0;
            count = objectContext.ImageCommentSet.Count(comm => comm.Image.ID == image.ID && comm.User.ID == user.ID);

            return count;
        }

        public List<ImageComment> GetImageComments(Entities objectContext, Image image)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            IEnumerable<ImageComment> comments = objectContext.ImageCommentSet.Where(comm => comm.Image.ID == image.ID);

            return comments.ToList();
        }

        public long CountImageComments(Entities objectContext, Image image)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            long count = objectContext.ImageCommentSet.Count(comm => comm.Image.ID == image.ID);

            return count;
        }

        public List<ImageComment> GetUserComments(Entities objectContext, User user)
        {
            CodeTools.CheckObjectContext(objectContext);

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IEnumerable<ImageComment> comments = objectContext.ImageCommentSet.Where(comm => comm.User.ID == user.ID);

            return comments.ToList();
        }

        public long CountUserComments(Entities objectContext, User user)
        {
            CodeTools.CheckObjectContext(objectContext);

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            long count = objectContext.ImageCommentSet.Count(comm => comm.User.ID == user.ID);

            return count;

        }









    }
}
