﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace OnlineGallery
{
    public partial class EditImage : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();
        protected Image currentImage = null;
        protected User currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckImage();

            ShowImage();

            if (IsPostBack == false)
            {
                FillDdlCategories();
            }

            HideLabels();
        }

        private void HideLabels()
        {
            lblInfoError.Visible = false;           
            lblErrorCategory.Visible = false;

            pnlNotification.Visible = false;
        }

        private void ShowImage()
        {
            giCurrImage.Parameters.Clear();

            giCurrImage.ImageHandlerUrl = "~/GetImageHandler.ashx";

            Microsoft.Web.ImageParameter avatarIDParam = new Microsoft.Web.ImageParameter();
            avatarIDParam.Name = "imageID";
            avatarIDParam.Value = currentImage.ID.ToString();
            giCurrImage.Parameters.Add(avatarIDParam);

            Microsoft.Web.ImageParameter thumbParam = new Microsoft.Web.ImageParameter();
            thumbParam.Name = "thumb";
            thumbParam.Value = "True";
            giCurrImage.Parameters.Add(thumbParam);

            giCurrImage.Width = 180;

            if (!currentImage.CategoryReference.IsLoaded)
            {
                currentImage.CategoryReference.Load();
            }

            hlImageLink.NavigateUrl = "ImagePage.aspx?image=" + currentImage.ID;

            lblDescription.Text = string.Format("категория : {0} <br/>описание : {1}",
                currentImage.Category.name, CodeTools.GetFormattedTextFromDB(currentImage.description));
        }

        private void CheckImage()
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

            CheckImageParams();
        }

        private void CheckImageParams()
        {
            string strImageId = Request["image"];
            if (!string.IsNullOrEmpty(strImageId))
            {

                long id = -1;
                if (long.TryParse(strImageId, out id))
                {
                    CodeImages codeImages = new CodeImages();
                    Image currImage = codeImages.Get(objectContext, id);
                    if (currImage == null)
                    {
                        Response.Redirect("Home.aspx");
                    }

                    if (!currImage.UserReference.IsLoaded)
                    {
                        currImage.UserReference.Load();
                    }

                    if (currImage.User != currentUser)
                    {
                        Response.Redirect("Home.aspx");
                    }

                    currentImage = currImage;
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }
            }
            else
            {
                Response.Redirect("Home.aspx");
            }

        }

        private void FillDdlCategories()
        {
            ddlCategory.Items.Clear();

            CodeCategories codeCategories = new CodeCategories();

            if (!currentImage.CategoryReference.IsLoaded)
            {
                currentImage.CategoryReference.Load();
            }

            Category primary = codeCategories.GetPrimaryCategory(objectContext);

            List<Category> categories = codeCategories.GetUserCategories(objectContext, currentUser);
            categories.Add(primary);

            ListItem firstItem = new ListItem();
            firstItem.Text = "избери категория";
            firstItem.Value = "0";
            ddlCategory.Items.Add(firstItem);

            if (categories != null && categories.Count > 1)
            {
                foreach (Category category in categories)
                {
                    if (category != currentImage.Category)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Text = category.name;
                        newItem.Value = category.ID.ToString();
                        ddlCategory.Items.Add(newItem);
                    }
                }
            }
            else
            {
                ddlCategory.Enabled = false;
                btnCategory.Enabled = false;
            }
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            CodeImages codeImages = new CodeImages();

            Category selectedCategory = GetSelectedCategory();
            if (selectedCategory != null)
            {
                codeImages.EditCategory(objectContext, currentImage, selectedCategory);

                pnlNotification.Visible = true;
                lblEditSucces.Text = "Категорията сменена !";

                FillDdlCategories();
                ShowImage();
            }
            else
            {
                lblErrorCategory.Visible = true;
                lblErrorCategory.Text = "Избери категория.";
            }

        }

        protected void btnInfo_Click(object sender, EventArgs e)
        {
            CodeImages codeImages = new CodeImages();

            lblInfoError.Visible = true;

            if (tbImageInfo.Text != currentImage.description)
            {
                if (CodeTools.ValidateInput(tbImageInfo.Text, 0, 1000, true))
                {

                    codeImages.EditDescription(objectContext, currentImage, tbImageInfo.Text);

                    pnlNotification.Visible = true;

                    if (string.IsNullOrEmpty(tbImageInfo.Text))
                    {
                        lblEditSucces.Text = "Описанието е изтрито !";
                    }
                    else
                    {
                        lblEditSucces.Text = "Описанието е подновено !";
                    }
                    
                    lblInfoError.Visible = false;

                    ShowImage();
                }
                else
                {
                    lblInfoError.Text = "Невалиден текст.";
                }
            }
            else
            {
                lblInfoError.Text = "Въведете информация.";
            }
        }

        private Category GetSelectedCategory()
        {
            string strCatId = ddlCategory.SelectedValue;
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

            if (catId != 0)
            {
                CodeCategories codeCategories = new CodeCategories();

                category = codeCategories.Get(objectContext, catId);
                if (category == null)
                {
                    throw new Exception();
                }

                if (!category.UserReference.IsLoaded)
                {
                    category.UserReference.Load();
                }

                if (category.IsPrimary == false && category.User != currentUser)
                {
                    throw new Exception();
                }

                if (!currentImage.CategoryReference.IsLoaded)
                {
                    currentImage.CategoryReference.Load();
                }

                if (category == currentImage.Category)
                {
                    throw new Exception();
                }
            }

            return category;
        }












    }
}
