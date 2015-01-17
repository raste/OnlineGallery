﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGallery
{
    public class CodeCategories
    {
        public void AddCategory(Entities objectContext, Category newCategory)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (newCategory == null)
            {
                throw new NullReferenceException("newCategory");
            }

            objectContext.AddToCategorySet(newCategory);
            CodeTools.Save(objectContext);
        }

        public void EditCategoryName(Entities objectContext, Category currCategory, string newName)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currCategory == null)
            {
                throw new NullReferenceException("currCategory");
            }
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException("newName");
            }

            if (!currCategory.UserReference.IsLoaded)
            {
                currCategory.UserReference.Load();
            }

            if (!IsCategoryNameTaken(objectContext, currCategory.User, newName))
            {

                currCategory.name = newName;
                CodeTools.Save(objectContext);
            }
            else
            {
                throw new Exception();
            }
        }

        public void EditCategoryDescription(Entities objectContext, Category currCategory, string newDescription)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currCategory == null)
            {
                throw new NullReferenceException("currCategory");
            }


            if (CodeTools.ValidateInput(newDescription, 0, 1000, true))
            {
                if (currCategory.description != newDescription)
                {
                    currCategory.description = newDescription;
                    CodeTools.Save(objectContext);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
            
        }


        public bool IsCategoryNameTaken(Entities objectContext, User currUser, string name)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currUser == null)
            {
                throw new NullReferenceException("currUser");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            bool taken = false;

            Category category = objectContext.CategorySet.FirstOrDefault(cat => cat.User.ID == currUser.ID && cat.name == name && cat.IsPrimary == false);
            if (category == null)
            {
                category = objectContext.CategorySet.FirstOrDefault(cat => cat.name == name && cat.IsPrimary == true);
                if (category != null)
                {
                    taken = true;
                }
            }
            else
            {
                taken = true;
            }

            return taken;
        }

        public void DeleteCategory(Entities objectContext, Category currCategory)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currCategory == null)
            {
                throw new NullReferenceException("currCategory");
            }

            if (currCategory.IsPrimary == true)
            {
                throw new Exception();
            }

            CodeImages codeImages = new CodeImages();

            List<Image> categoryImages = GetCategoryImages(objectContext, currCategory);
            if (categoryImages != null && categoryImages.Count > 0)
            {
                foreach (Image image in categoryImages)
                {
                    codeImages.DeleteImage(objectContext, image);
                }

            }

            objectContext.DeleteObject(currCategory);
            CodeTools.Save(objectContext);
        }

        public List<Image> GetCategoryImages(Entities objectContext, Category currCategory)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currCategory == null)
            {
                throw new NullReferenceException("currCategory");
            }

            IEnumerable<Image> images = objectContext.ImageSet.Where(img => img.Category.ID == currCategory.ID);
            if (images.Count() > 0)
            {
                return images.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<Category> GetUserCategories(Entities objectContext, User currUser)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currUser == null)
            {
                throw new NullReferenceException("currUser");
            }

            IEnumerable<Category> categories = objectContext.CategorySet.Where(cat => cat.User.ID == currUser.ID && cat.IsPrimary == false);
            return categories.ToList();
        }

        public List<Category> GetPrimaryCategories(Entities objectContext)
        {
            CodeTools.CheckObjectContext(objectContext);

            IEnumerable<Category> categories = objectContext.CategorySet.Where(cat => cat.IsPrimary == true);
            return categories.ToList();
        }

        public Category GetPrimaryCategory(Entities objectContext)
        {
            CodeTools.CheckObjectContext(objectContext);

            Category category = objectContext.CategorySet.FirstOrDefault(cat => cat.name == "всички" && cat.IsPrimary == true);
            if (category == null)
            {
                throw new Exception("no primary category");
            }

            return category;
        }

        public Category Get(Entities objectContext, long catId)
        {
            CodeTools.CheckObjectContext(objectContext);

            Category currCat = objectContext.CategorySet.FirstOrDefault(cat => cat.ID == catId);

            return currCat;
        }

        public int CountUserCategories(Entities objectContext, User currUser)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currUser == null)
            {
                throw new ArgumentNullException("currUser");
            }

            int count = objectContext.CategorySet.Count(cat => cat.User.ID == currUser.ID);

            return count;
        }

        public bool UserHaveInagesInPrimaryCategory(Entities objectContext, User currUser, Category primCategory)
        {
            CodeTools.CheckObjectContext(objectContext);
            if (currUser == null)
            {
                throw new ArgumentNullException("currUser");
            }
            if (primCategory == null)
            {
                throw new ArgumentNullException();
            }
            else if (primCategory.IsPrimary == false)
            {
                throw new Exception();
            }

            bool result = false;

            Image image = objectContext.ImageSet.FirstOrDefault(img => img.User.ID == currUser.ID && img.Category.ID == primCategory.ID);
            if (image != null)
            {
                result = true;
            }

            return result;

        }





    }
}
