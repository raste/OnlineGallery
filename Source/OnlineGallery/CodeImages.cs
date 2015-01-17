﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OnlineGallery
{
    public class CodeImages
    {

        public void AddImage(Entities objectContext, Image newImage)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (newImage == null)
            {
                throw new NullReferenceException("newImage");
            }

            objectContext.AddToImageSet(newImage);
            CodeTools.Save(objectContext);
        }

        public void EditDescription(Entities objectContext, Image currImage, string newValue)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currImage == null)
            {
                throw new NullReferenceException("currImage");
            }

            if(!CodeTools.ValidateInput(newValue,0, 1000, true))
            {
                throw new Exception(newValue);
            }

            if (currImage.description != newValue)
            {
                currImage.description = newValue;
                CodeTools.Save(objectContext);
            }
        }

        public void EditCategory(Entities objectContext, Image currImage, Category newCategory)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currImage == null)
            {
                throw new NullReferenceException("currImage");
            }
            if (newCategory == null)
            {
                throw new NullReferenceException("newCategory");
            }

            if (!currImage.CategoryReference.IsLoaded)
            {
                currImage.CategoryReference.Load();
            }

            if (currImage.Category != newCategory)
            {
                currImage.Category = newCategory;
                CodeTools.Save(objectContext);
            }
        }


        public void DeleteImage(Entities objectContext, Image currImage)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currImage == null)
            {
                throw new NullReferenceException("currImage");
            }

            List<ImageComment> comments = null;
            List<ImageRating> ratings = null;
            List<ImageVisit> visits = null;

            IEnumerable<ImageComment> ieComments = objectContext.ImageCommentSet.Where(comm => comm.Image.ID == currImage.ID);
            comments = ieComments.ToList();
            if (comments != null && comments.Count > 0)
            {
                foreach (ImageComment comment in comments)
                {
                    objectContext.DeleteObject(comment);
                    CodeTools.Save(objectContext);
                }
            }
           

            IEnumerable<ImageRating> ieRatings = objectContext.ImageRatingSet.Where(rat => rat.Image.ID == currImage.ID);
            ratings = ieRatings.ToList();
            if (ratings != null && ratings.Count > 0)
            {
                foreach (ImageRating rating in ratings)
                {
                    objectContext.DeleteObject(rating);
                    CodeTools.Save(objectContext);
                }
            }
            

            IEnumerable<ImageVisit> ieVisits = objectContext.ImageVisitSet.Where(vis => vis.Image.ID == currImage.ID);
            visits = ieVisits.ToList();
            if (visits != null && visits.Count > 0)
            {
                foreach (ImageVisit visit in visits)
                {
                    objectContext.DeleteObject(visit);
                    CodeTools.Save(objectContext);
                }
            }

            objectContext.DeleteObject(currImage);
            CodeTools.Save(objectContext);
        }

        public Image Get(Entities objectContext, long id)
        {
            CodeTools.CheckObjectContext(objectContext);

            Image image = objectContext.ImageSet.FirstOrDefault(img => img.ID == id);

            return image;
        }

        public List<Image> GetLastImages(Entities objectContext)
        {
            CodeTools.CheckObjectContext(objectContext);

            IEnumerable<Image> images = objectContext.ImageSet.
                OrderByDescending<Image, long>(new Func<Image, long>(IdSelector)); ;

            return images.ToList();
        }

        public List<Image> GetImagesInCategory(Entities objectContext, User currUser, bool primaryCategory, Category category, string sortBy)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (category == null)
            {
                throw new NullReferenceException("category");
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                throw new ArgumentNullException(sortBy);
            }

            if (primaryCategory == false && currUser == null)
            {
                throw new Exception("currUser is null");
            }

            IEnumerable<Image> ieImages = null;

            CodeCategories codeCategories = new CodeCategories();
            Category primary = codeCategories.GetPrimaryCategory(objectContext);

            if (primaryCategory == true)
            {
                if (category.IsPrimary == false)
                {
                    throw new Exception();
                }

                if (category == primary)
                {
                    switch (sortBy)
                    {
                        case ("date"):
                            ieImages = objectContext.ImageSet.OrderByDescending<Image, long>(new Func<Image, long>(IdSelector));
                            break;
                        case ("rating"):
                            ieImages = objectContext.ImageSet.OrderByDescending<Image, long>(new Func<Image, long>(RatingSelector));
                            break;
                        case ("comments"):
                            ieImages = objectContext.ImageSet.OrderByDescending<Image, long>(new Func<Image, long>(CommentsSelector));
                            break;
                        case ("visits"):
                            ieImages = objectContext.ImageSet.OrderByDescending<Image, long>(new Func<Image, long>(VisitsSelector));
                            break;
                        default:
                            throw new Exception("sortBy =  " + sortBy + " is not supported");
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case ("date"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID)
                                .OrderByDescending<Image, long>(new Func<Image, long>(IdSelector));
                            break;
                        case ("rating"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID)
                                .OrderByDescending<Image, long>(new Func<Image, long>(RatingSelector));
                            break;
                        case ("comments"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID)
                           .OrderByDescending<Image, long>(new Func<Image, long>(CommentsSelector));
                            break;
                        case ("visits"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID)
                          .OrderByDescending<Image, long>(new Func<Image, long>(VisitsSelector));
                            break;
                        default:
                            throw new Exception("sortBy =  " + sortBy + " is not supported");
                    }
                }
            }
            else
            {
                if (category == primary)
                {
                    switch (sortBy)
                    {
                        case ("date"):
                            ieImages = objectContext.ImageSet.Where(img => img.User.ID == currUser.ID)
                                .OrderByDescending<Image, long>(new Func<Image, long>(IdSelector));
                            break;
                        case ("rating"):
                            ieImages = objectContext.ImageSet.Where(img => img.User.ID == currUser.ID)
                                .OrderByDescending<Image, long>(new Func<Image, long>(RatingSelector));
                            break;
                        case ("comments"):
                            ieImages = objectContext.ImageSet.Where(img => img.User.ID == currUser.ID)
                           .OrderByDescending<Image, long>(new Func<Image, long>(CommentsSelector));
                            break;
                        case ("visits"):
                            ieImages = objectContext.ImageSet.Where(img => img.User.ID == currUser.ID)
                          .OrderByDescending<Image, long>(new Func<Image, long>(VisitsSelector));
                            break;
                        default:
                            throw new Exception("sortBy =  " + sortBy + " is not supported");
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case ("date"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID && img.User.ID == currUser.ID)
                                .OrderByDescending<Image, long>(new Func<Image, long>(IdSelector));
                            break;
                        case ("rating"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID && img.User.ID == currUser.ID)
                                .OrderByDescending<Image, long>(new Func<Image, long>(RatingSelector));
                            break;
                        case ("comments"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID && img.User.ID == currUser.ID)
                           .OrderByDescending<Image, long>(new Func<Image, long>(CommentsSelector));
                            break;
                        case ("visits"):
                            ieImages = objectContext.ImageSet.Where(img => img.Category.ID == category.ID && img.User.ID == currUser.ID)
                          .OrderByDescending<Image, long>(new Func<Image, long>(VisitsSelector));
                            break;
                        default:
                            throw new Exception("sortBy =  " + sortBy + " is not supported");
                    }
                }
            }

            return ieImages.ToList();
        }

        private long IdSelector(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            return image.ID;
        }

        private long RatingSelector(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            return (image.liked - image.disliked);
        }

        private long CommentsSelector(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            return image.commens;
        }

        private long VisitsSelector(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            return image.visits;
        }

        public long CountImagesInCategory(Entities objectContext, Category category, bool isPrimary, User currUser)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (category == null)
            {
                throw new NullReferenceException("category");
            }

            if (isPrimary == false)
            {
                if (currUser == null)
                {
                    throw new NullReferenceException("currUser");
                }
            }

            long number = 0;

            CodeCategories codeCategories = new CodeCategories();
            Category primary = codeCategories.GetPrimaryCategory(objectContext);

            if (isPrimary == true)
            {
                if (category == primary)
                {
                    number = objectContext.ImageSet.Count();
                }
                else
                {
                    number = objectContext.ImageSet.Count(img => img.Category.ID == category.ID);
                }
            }
            else
            {
                if (category == primary)
                {
                    number = objectContext.ImageSet.Count(img => img.User.ID == currUser.ID);
                }
                else
                {
                    number = objectContext.ImageSet.Count(img => img.Category.ID == category.ID && img.User.ID == currUser.ID);
                }
            }

            return number;
        }

        public long CountUserImages(Entities objectContext, User currUser)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currUser == null)
            {
                throw new NullReferenceException("currUser");
            }

            long number = 0;

            number = objectContext.ImageSet.Count(img => img.User.ID == currUser.ID);
         
            return number;
        }



        public Image GetUserNextImage(Entities objectContext, Image currImage)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currImage == null)
            {
                throw new NullReferenceException("currImage");
            }

            if (!currImage.UserReference.IsLoaded)
            {
                currImage.UserReference.Load();
            }

            Image nextImage = objectContext.ImageSet.FirstOrDefault(img => img.User.ID == currImage.User.ID && img.ID > currImage.ID);

            return nextImage;
        }

        public Image GetUserPreviusImage(Entities objectContext, Image currImage)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currImage == null)
            {
                throw new NullReferenceException("currImage");
            }

            if (!currImage.UserReference.IsLoaded)
            {
                currImage.UserReference.Load();
            }

            IEnumerable<Image> prevImages = objectContext.ImageSet.Where(img => img.User.ID == currImage.User.ID && img.ID < currImage.ID);

            Image prevImage = null;

            List<Image> images = prevImages.ToList();
            if (images != null && images.Count > 0)
            {
                prevImage = images.Last();
            }

            return prevImage;
        }



        public void ScriptMakeThumbnails(Entities objectContext)
        {
            CodeTools.CheckObjectContext(objectContext);

            IEnumerable<Image> images = objectContext.ImageSet;
            List<Image> listImages = images.ToList();

            if (listImages != null && listImages.Count > 0)
            {
                foreach (Image image in listImages)
                {
                    MakeThumbnail(objectContext, image);
                }
                
            }

        }

        public void MakeThumbnail(Entities objectContext, Image image)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            byte[] thumbnailBytes = GetThumbnailAsBytes(image, 300);

            image.thumbnail = thumbnailBytes;
            objectContext.SaveChanges();
        }

        public byte[] GetThumbnailBytes(Entities objectContext, Image image, int width)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            byte[] thumbnailBytes = GetThumbnailAsBytes(image, width);
            return thumbnailBytes;
        }

        public byte[] GetResizedImgFromBytes(Entities objectContext, byte[] fileBytes, int width)
        {
            CodeTools.CheckObjectContext(objectContext);

            byte[] thumbnailBytes = ResizeFromBytes(width, fileBytes, null);

            if (thumbnailBytes == null)
            {
                string msg = string.Format("Could not create a thumbnail for image from bytes ");
                throw new InvalidOperationException(msg);
            }
            return thumbnailBytes;
        }

        private byte[] GetThumbnailAsBytes(Image image, int thumbWidth)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            if (thumbWidth < 1)
            {
                throw new ArgumentOutOfRangeException("thumbWidth < 1");
            }

            byte[] filebytes = image.image;

            byte[] thumbnailBytes = thumbnailBytes = ResizeFromBytes(thumbWidth, filebytes, image.ID);
            return thumbnailBytes;
        }

        private byte[] ResizeFromBytes(int thumbWidth, byte[] filebytes, long? imageId)
        {
            byte[] thumbnailBytes = null;

            using (System.IO.Stream strm = new System.IO.MemoryStream(filebytes, false))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(strm, true, true);

                if (img.Width > 0)
                {
                    int thumbHeight = thumbWidth * img.Height / img.Width;
                    int buffSize = thumbWidth * thumbHeight * 4;  // * 4 - for 32-bit color

                    ThumbnailAborted = false;
                    System.Drawing.Image thumbnail =
                        img.GetThumbnailImage(thumbWidth, thumbHeight, ProcessThumbnailAbort, IntPtr.Zero);
                    if (ThumbnailAborted == false)
                    {
                        string tempFileName = Path.GetTempFileName();
                        using (FileStream tempFileStream = File.Create(tempFileName, buffSize, FileOptions.DeleteOnClose))
                        {
                            try
                            {
                                thumbnail.Save(tempFileStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                                long streamSize = tempFileStream.Position;
                                if (streamSize <= int.MaxValue)
                                {
                                    int thumbnailBytesCount = (int)streamSize;

                                    thumbnailBytes = new byte[thumbnailBytesCount];
                                    tempFileStream.Seek(0, SeekOrigin.Begin);
                                    if (tempFileStream.Read(thumbnailBytes, 0, thumbnailBytesCount) != thumbnailBytesCount)
                                    {
                                        throw new InvalidOperationException("Unsuccessful read from temporary file.");
                                    }
                                }
                                else
                                {
                                    throw new InvalidOperationException("Temporary file too big.");
                                }
                            }
                            finally
                            {
                                tempFileStream.Close();
                            }
                        }
                    }
                    else
                    {
                        string msg = string.Format("The width of the image (ID = {0}) is 0.",
                            imageId.HasValue ? imageId.Value.ToString() : "not specified");
                        throw new InvalidOperationException(msg);
                    }
                }

            }

            if (thumbnailBytes == null)
            {
                string msg = string.Format("Could not create a thumbnail of the image which ID is {0}.", 
                    imageId.HasValue ? imageId.Value.ToString() : "not specified");
                throw new InvalidOperationException(msg);
            }

            return thumbnailBytes;
        }

        private bool ProcessThumbnailAbort()
        {
            ThumbnailAborted = true;
            return false;
        }

        /// <summary>
        /// To be only used by MakeThumbnail() and ProcessThumbnailAbort().
        /// </summary>
        private bool ThumbnailAborted { get; set; }


        public static Boolean GetImageWidthAndHeight(byte[] fileContents, out int width, out int height)
        {
            width = 0;
            height = 0;

            using (System.IO.Stream strm = new System.IO.MemoryStream(fileContents, false))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(strm, true, true);
                height = img.Height;
                width = img.Width;
            }

            Boolean result = false;
            if (width > 0 && height > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
