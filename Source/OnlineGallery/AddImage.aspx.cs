﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace OnlineGallery
{
    public partial class AddImage : System.Web.UI.Page
    {
        private Entities objectContext = new Entities();
        private User currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            ShowInfo();

            pnlNotification.Visible = false;
        }

        private void ShowInfo()
        {
            lblError.Visible = false;

            if (IsPostBack == false)
            {
                FillDdlCategories();
            }
        }

        private void FillDdlCategories()
        {
            ddlCategories.Items.Clear();
            CodeCategories codeCategories = new CodeCategories();

            ListItem firstItem = new ListItem();
            firstItem.Text = "избери категория";
            firstItem.Value = "0";
            ddlCategories.Items.Add(firstItem);

            //Category primary = codeCategories.GetPrimaryCategory(objectContext);
            //ListItem primItem = new ListItem();
            //primItem.Text = primary.name;
            //primItem.Value = primary.ID.ToString();
            //ddlCategories.Items.Add(primItem);

            List<Category> primaryCategories = codeCategories.GetPrimaryCategories(objectContext);
            if (primaryCategories == null || primaryCategories.Count == 0)
            {
                throw new Exception();
            }
            foreach (Category category in primaryCategories)
            {
                ListItem newItem = new ListItem();
                newItem.Text = category.name;
                newItem.Value = category.ID.ToString();
                ddlCategories.Items.Add(newItem);
            }

            List<Category> categories = codeCategories.GetUserCategories(objectContext, currentUser);
            if (categories != null && categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    ListItem newItem = new ListItem();
                    newItem.Text = category.name;
                    newItem.Value = category.ID.ToString();
                    ddlCategories.Items.Add(newItem);
                }
            }
        }

        private void CheckUser()
        {
            CodeUsers codeUsers = new CodeUsers();
            User currUser = codeUsers.GetUser(objectContext, CodeTools.GetCurrentUserId(), false);
            if (currUser != null)
            {
                currentUser = currUser;
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void btnAddImage_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder errorMsg = new StringBuilder();
            bool passed = true;

            if (fuImage.HasFile)
            {
                // image
                string imageErrorDescription = string.Empty;
                byte[] fileBytes = fuImage.FileBytes;
                string fileName = fuImage.FileName;
                string fileType = System.IO.Path.GetExtension(fileName);

                if (CodeTools.IsValidImage(fileName, fileBytes))
                {
                    int width = 0;
                    int height = 0;
                    if (CodeTools.GetImageWidthAndHeight(fileBytes, out width, out height))
                    {
                        if (width < 500 || height < 500)
                        {
                            errorMsg.Append("Невалидна широчина и/или височина на картинката. <br />");
                            passed = false;
                        }
                    }
                    else
                    {
                        errorMsg.Append("Невалидна широчина и/или височина на картинката. <br />");
                        passed = false;
                    }
                }
                else
                {
                    errorMsg.Append("Невалидна картинка. <br />");
                    passed = false;
                }
            }
            else
            {
                errorMsg.Append("Изберете картинка. <br />");
                passed = false;
            }

            if (!CodeTools.ValidateInput(tbDescription.Text, 0, 1000, true))
            {
                errorMsg.Append("Невалиднo описание. <br />");
                passed = false;
            }

            Category selected = GetSelectedCategory();
            if (selected == null)
            {
                passed = false;
                errorMsg.Append("Изберете категория. <br />");
            }

            if (passed)
            {
                CodeImages codeImages = new CodeImages();

                Image newImage = new Image();

                newImage.User = currentUser;
                newImage.Category = selected;
                newImage.dateUploaded = DateTime.UtcNow;
                newImage.image = fuImage.FileBytes;
                newImage.liked = 0;
                newImage.disliked = 0;
                newImage.commens = 0;
                newImage.description = tbDescription.Text;

                newImage.thumbnail = codeImages.GetThumbnailBytes(objectContext, newImage, 300);
                codeImages.AddImage(objectContext, newImage);
                
                tbDescription.Text = "";
                FillDdlCategories();

                lblInfo.Text = "Картинката е добавена успешно! Добави нова?";
                pnlNotification.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = errorMsg.ToString();
            }

        }

        private Category GetSelectedCategory()
        {
            string strCatId = ddlCategories.SelectedValue;
            if (string.IsNullOrEmpty(strCatId))
            {
                throw new Exception();
            }

            long catId = -1;
            if (!long.TryParse(strCatId, out catId))
            {
                throw new Exception();
            }

            Category category = null;

            CodeCategories codeCategories = new CodeCategories();

            category = codeCategories.Get(objectContext, catId);

            return category;
        }

    }
}
