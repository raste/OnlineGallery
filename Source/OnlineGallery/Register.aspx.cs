﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Web.UI.WebControls;
using System.Text;

namespace OnlineGallery
{
    public partial class Register : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();
        protected bool resizeAvatar = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            if (IsPostBack == false)
            {
                FillDDLists();
            }
        }

        private void FillDDLists()
        {
            ddlDay.Items.Clear();

            ListItem zeroItem = new ListItem();
            zeroItem.Text = "";
            zeroItem.Value = "";
            ddlDay.Items.Add(zeroItem);

            for (int i = 1; i < 32; i++)
            {
                ListItem newItem = new ListItem();
                newItem.Text = i.ToString();
                newItem.Value = i.ToString();
                ddlDay.Items.Add(newItem);
            }

            ddlYear.Items.Clear();

            ListItem zero1Item = new ListItem();
            zero1Item.Text = "";
            zero1Item.Value = "";
            ddlYear.Items.Add(zero1Item);

            for (int i = 2000; i > 1919; i--)
            {
                ListItem newItem = new ListItem();
                newItem.Text = i.ToString();
                newItem.Value = i.ToString();
                ddlYear.Items.Add(newItem);
            }
        }

        private void CheckUser()
        {

            CodeUsers codeUsers = new CodeUsers();
            User currUser = codeUsers.GetUser(objectContext, CodeTools.GetCurrentUserId(), false);
            if (currUser != null)
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void tbRegister_Click(object sender, EventArgs e)
        {
            if (CheckRegData())
            {
                CodeUsers codeUsers = new CodeUsers();

                User newUser = new User();

                newUser.username = tbUsername.Text;
                newUser.password = tbPassword.Text;
                newUser.dateRegistered = DateTime.UtcNow;
                newUser.name = tbName.Text;
                newUser.city = tbCity.Text;
                newUser.email = tbEmail.Text;
                newUser.moreInfo = tbOtherInfo.Text;

                if (ddlDay.SelectedIndex > 0)
                {
                    string date = String.Format("{0}/{1}/{2}", ddlDay.SelectedIndex, ddlMonth.SelectedIndex, ddlYear.SelectedValue);
                    DateTime newDate = new DateTime();
                    if (DateTime.TryParse(date, out newDate))
                    {
                        newUser.birthdate = newDate;
                    }
                    else
                    {
                        throw new Exception("couldnt parse to date");
                    }
                }
                else
                {
                    newUser.birthdate = null;
                }

                if (fuImage.HasFile)
                {
                    if (resizeAvatar)
                    {
                        CodeImages codeImages = new CodeImages();
                        newUser.avatar = codeImages.GetResizedImgFromBytes(objectContext, fuImage.FileBytes, 150);
                    }
                    else
                    {
                        newUser.avatar = fuImage.FileBytes;
                    } 
                }
                else
                {
                    newUser.avatar = null;
                }

                codeUsers.AddUser(objectContext, newUser);

                pnlRegister.Enabled = false;
                pnlRegSucc.Visible = true;
                lblName.Text = newUser.username;
            }
        }

        private bool CheckRegData()
        {
            StringBuilder errorMsg = new StringBuilder();

            bool passed = true;

            if (!CodeTools.ValidateInput(tbUsername.Text, 4, 20, false))
            {
                // username
                errorMsg.Append("Невалидно потребителско име. <br />");
                passed = false;
            }
            else
            {
                CodeUsers codeUsers = new CodeUsers();
                if (codeUsers.IsUserNameTaken(objectContext, tbUsername.Text))
                {
                    errorMsg.Append("Потребителското име е заето. <br />");
                    passed = false;
                }

            }

            if (!CodeTools.ValidateInput(tbPassword.Text, 4, 20, false))
            {
                // pass
                errorMsg.Append("Невалидна парола. <br />");
                passed = false;
            }
            else
            {
                if (!CodeTools.ValidateInput(tbRepPassword.Text, 4, 20, false))
                {
                    // rep pass
                    errorMsg.Append("Повторете паролата. <br />");
                    passed = false;
                }
                else
                {
                    if (tbPassword.Text != tbRepPassword.Text)
                    {
                        errorMsg.Append("Двете пароли не съвпадат. <br />");
                        passed = false;
                    }
                }
            }

            if (!CodeTools.ValidateInput(tbEmail.Text, 0, 40, false))
            {
                // email
                errorMsg.Append("Невалиден емайл. <br />");
                passed = false;
            }

            if (!CodeTools.ValidateInput(tbName.Text, 0, 50, true))
            {
                // name
                errorMsg.Append("Невалидно име. <br />");
                passed = false;
            }

            if (!CodeTools.ValidateInput(tbCity.Text, 0, 40, false))
            {
                // city
                errorMsg.Append("Невалиден град. <br />");
                passed = false;
            }

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
                        if (width < 100 || height < 100)
                        {
                            errorMsg.Append("Невалидна широчина и/или височина на картинката. <br />");
                            passed = false;
                        }

                        if (width > 150)
                        {
                            resizeAvatar = true;
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

            // data
            if (ddlDay.SelectedIndex > 0 || ddlMonth.SelectedIndex > 0 || ddlYear.SelectedIndex > 0)
            {
                if (ddlDay.SelectedIndex < 1)
                {
                    errorMsg.Append("Изберете ден в който сте роден. <br />");
                    passed = false;
                }

                if (ddlMonth.SelectedIndex < 1)
                {
                    errorMsg.Append("Изберете месец в който сте роден. <br />");
                    passed = false;
                }

                if (ddlYear.SelectedIndex < 1)
                {
                    errorMsg.Append("Изберете година в която сте роден. <br />");
                    passed = false;
                }
            }
            

            if (!CodeTools.ValidateInput(tbOtherInfo.Text, 0, 500, true))
            {
                // other info
                errorMsg.Append("Некоректно въведена допълнителна информация. <br />");
                passed = false;
            }

            if (passed == false)
            {
                lblError.Visible = true;
                lblError.Text = errorMsg.ToString();
            }
            else
            {
                lblError.Visible = false;
                lblError.Text = "";
            }

            return passed;
        }

    }
}
