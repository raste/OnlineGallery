﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;

namespace OnlineGallery
{
    public partial class EditCategory : System.Web.UI.Page
    {
        private Entities objectContext = new Entities();
        private User currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            CheckPanels();
            ShowInfo();

            pnlNotifPanel.Visible = false;
        }

        private void ShowInfo()
        {
            lblAddCatError.Visible = false;
            lblEditCatError.Visible = false;

            if (IsPostBack == false)
            {
                UpdateEditPanel();
            }

        }

        private void UpdateEditPanel()
        {
            tbEditCatDescr.Enabled = false;
            tbEditCatName.Enabled = false;

            tbEditCatDescr.Text = "";
            tbEditCatName.Text = "";

            FillDdlCategories();
        }

        private void FillDdlCategories()
        {
            ddlCategories.Items.Clear();
            CodeCategories codeCategories = new CodeCategories();

            ListItem firstItem = new ListItem();
            firstItem.Text = "избери категория";
            firstItem.Value = "0";
            ddlCategories.Items.Add(firstItem);

            List<Category> categories = codeCategories.GetUserCategories(objectContext, currentUser);
            if (categories.Count < 1)
            {
                pnlEditCategory.Enabled = false;
            }
            else
            {
                pnlEditCategory.Enabled = true;
            }

           

            foreach (Category category in categories)
            {
                ListItem newItem = new ListItem();
                newItem.Text = category.name;
                newItem.Value = category.ID.ToString();
                ddlCategories.Items.Add(newItem);
            }

        }

        private void CheckPanels()
        {
            CodeCategories codeCategories = new CodeCategories();
            int count = codeCategories.CountUserCategories(objectContext, currentUser);
            if (count < 16)
            {
                pnlAddCategory.Visible = true;
            }
            else
            {
                pnlAddCategory.Visible = false;
            }

            if (count > 0)
            {
                pnlEditCategory.Visible = true;
            }
            else
            {
                pnlEditCategory.Visible = false;
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

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {

            bool validInput = true;
            StringBuilder error = new StringBuilder();

            CodeCategories codeCategories = new CodeCategories();

            if (!CodeTools.ValidateInput(tbAddCatName.Text, 2, 20, true))
            {
                validInput = false;
                error.Append("Невалидно име на категория. <br />");
            }
            else
            {
                if (codeCategories.IsCategoryNameTaken(objectContext, currentUser, tbAddCatName.Text))
                {
                    error.Append("Това име е заето вече. <br />");
                    validInput = false;
                }
            }

            if (!CodeTools.ValidateInput(tbAddCatDescr.Text, 0, 1000, true))
            {
                error.Append("Невалидно описание. <br />");
                validInput = false;
            }

            if (validInput)
            {
                Category newCategory = new Category();

                newCategory.User = currentUser;
                newCategory.name = tbAddCatName.Text;
                newCategory.description = tbAddCatDescr.Text;
                newCategory.IsPrimary = false;

                codeCategories.AddCategory(objectContext, newCategory);

                pnlNotifPanel.Visible = true;
                lblChangeDone.Text = "Категорията е добавена успешно !";

                tbAddCatName.Text = "";
                tbAddCatDescr.Text = "";

                CheckPanels();
                UpdateEditPanel();

                MasterPage master = this.Master as MasterPage;
                if (master != null)
                {
                    master.UpdateObjectContext();
                    master.CheckUser();
                }


            }
            else
            {
                lblAddCatError.Visible = true;
                lblAddCatError.Text = error.ToString();
            }

        }

        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
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

            if (catId == 0)
            {
                tbEditCatName.Enabled = false;
                tbEditCatDescr.Enabled = false;
                tbEditCatDescr.Text = "";
                tbEditCatName.Text = "";
            }
            else
            {
                Category category = GetSelectedCategory();

                tbEditCatName.Enabled = true;
                tbEditCatDescr.Enabled = true;

                tbEditCatDescr.Text = category.description;
                tbEditCatName.Text = category.name;
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

            if (catId != 0)
            {
                CodeCategories codeCategories = new CodeCategories();

                Category category = codeCategories.Get(objectContext, catId);
                if (category == null)
                {
                    throw new Exception();
                }

                if (!category.UserReference.IsLoaded)
                {
                    category.UserReference.Load();
                }

                if (category.User != currentUser)
                {
                    throw new Exception();
                }

                return category;
            }
            else
            {
                throw new Exception();
            }


        }

        protected void btnEditCat_Click(object sender, EventArgs e)
        {
            Category category = GetSelectedCategory();

            StringBuilder error = new StringBuilder();

            CodeCategories codeCategories = new CodeCategories();
            bool valid = true;
            bool newName = false;
            bool newDescr = false;
            if (tbEditCatName.Text != category.name)
            {
                if(CodeTools.ValidateInput(tbEditCatName.Text, 2, 20, true))
                {
                    if (codeCategories.IsCategoryNameTaken(objectContext, currentUser, tbEditCatName.Text))
                    {
                        error.Append("Имате категория с такова име вече. <br />");
                        valid = false;
                    }
                    else
                    {
                        newName = true;
                    }
                }
                else
                {
                    error.Append("Невалидно име. <br />");
                    valid = false;
                }
            }

            if (tbEditCatDescr.Text != category.description)
            {
                if (CodeTools.ValidateInput(tbEditCatDescr.Text, 0, 1000, true))
                {
                    newDescr = true;
                }
                else
                {
                    error.Append("Невалидно описание. <br />");
                    valid = false;
                }
            }

            if (valid == true && newName == false && newDescr == false)
            {
                error.Append("Въведете ново име или ново описание. <br />");
                valid = false;
            }

            if (valid)
            {
                if (newName)
                {
                    codeCategories.EditCategoryName(objectContext, category, tbEditCatName.Text);
                }
                if (newDescr)
                {
                    codeCategories.EditCategoryDescription(objectContext, category, tbEditCatDescr.Text);
                }

                UpdateEditPanel();

                pnlNotifPanel.Visible = true;
                lblChangeDone.Text = "Категорията беше променена успешно !";

                MasterPage master = this.Master as MasterPage;
                if (master != null)
                {
                    master.UpdateObjectContext();
                    master.CheckUser();
                }
            }
            else
            {
                lblEditCatError.Visible = true;
                lblEditCatError.Text = error.ToString();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CodeCategories codeCategories = new CodeCategories();

            Category category = GetSelectedCategory();

            codeCategories.DeleteCategory(objectContext, category);

            CheckPanels();
            UpdateEditPanel();

            pnlNotifPanel.Visible = true;
            lblChangeDone.Text = "Категорията беше изтрита успешно !";

            MasterPage master = this.Master as MasterPage;
            if (master != null)
            {
                master.UpdateObjectContext();
                master.CheckUser();
            }
        }







    }
}
