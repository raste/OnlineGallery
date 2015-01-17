﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Web.UI.WebControls;
using System.Text;

namespace OnlineGallery
{
    public partial class EditUser : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();
        protected User currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            if (IsPostBack == false)
            {
                FillDDLists();
            }
            FillUserInfo();
            HideLabels();
            
        }

        private void HideLabels()
        {
            lblErrorAvatar.Visible = false;
            lblErrorBirthDate.Visible = false;
            lblErrorCity.Visible = false;
            lblErrorMail.Visible = false;
            lblErrorMoreInfo.Visible = false;
            lblErrorName.Visible = false;
            lblErrorPass.Visible = false;

            notifDiv.Visible = false;
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
                currentUser = currUser;
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }

        private void ShowSuccessLable(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            notifDiv.Visible = true;
            lblOpSucc.Text = message;
        }

        protected void btnNewPassword_Click(object sender, EventArgs e)
        {
            lblErrorPass.Visible = true;

            CodeUsers codeUsers = new CodeUsers();
            if (currentUser.password != tbPassword.Text)
            {
                if (CodeTools.ValidateInput(tbPassword.Text, 4, 20, false))
                {
                    if(tbPassword.Text == tbRepPass.Text)
                    {
                        currentUser.password = tbPassword.Text;

                        CodeTools.Save(objectContext);
                        ShowSuccessLable("Паролата е сменена!");

                        lblErrorPass.Visible = false;

                    }
                    else
                    {
                        lblErrorPass.Text = "Двете пароли не съвпадат.";
                    }
                }
                else
                {
                    lblErrorPass.Text = "Невалидна парола.";
                }
            }
            else
            {
                lblErrorPass.Text = "Въведете парола различна от настоящата.";
            }

        }

        protected void btnNewName_Click(object sender, EventArgs e)
        {
            CodeUsers codeUsers = new CodeUsers();

            lblErrorName.Visible = true;

            if (currentUser.name != tbName.Text)
            {
                if (CodeTools.ValidateInput(tbName.Text, 0, 50, true))
                {

                    currentUser.name = tbName.Text;

                    CodeTools.Save(objectContext);
                    ShowSuccessLable("Името е сменено!");

                    lblErrorName.Visible = false;

                    tbName.Text = "";

                    FillUserInfo();
                }
                else
                {
                    lblErrorName.Text = "Невалидно име.";
                }
            }
            else
            {
                lblErrorName.Text = "Въведете име различно от настоящето.";
            }

        }

        protected void btnNewCity_Click(object sender, EventArgs e)
        {
            lblErrorCity.Visible = true;

            CodeUsers codeUsers = new CodeUsers();

            if (currentUser.city != tbCity.Text)
            {
                if (CodeTools.ValidateInput(tbCity.Text, 0, 50, true))
                {

                    currentUser.city = tbCity.Text;

                    CodeTools.Save(objectContext);
                    ShowSuccessLable("Градът е сменен!");

                    lblErrorCity.Visible = false;

                    tbCity.Text = "";

                    FillUserInfo();
                }
                else
                {
                    lblErrorCity.Text = "Невалиден град.";
                }
            }
            else
            {
                lblErrorCity.Text = "Въведете град различен от настоящия.";
            }

        }

        protected void btnNewMail_Click(object sender, EventArgs e)
        {
            lblErrorMail.Visible = true;

            CodeUsers codeUsers = new CodeUsers();

            if (currentUser.email != tbEmail.Text)
            {
                if (CodeTools.ValidateInput(tbEmail.Text, 0, 40, false))
                {

                    currentUser.email = tbEmail.Text;

                    CodeTools.Save(objectContext);
                    ShowSuccessLable("Имейлът е сменен!");

                    lblErrorMail.Visible = false;

                    tbEmail.Text = "";

                    FillUserInfo();
                }
                else
                {
                    lblErrorMail.Text = "Невалиден емайл.";
                }
            }
            else
            {
                lblErrorMail.Text = "Въведете емайл различен от настоящия.";
            }

        }

        protected void btnNewAvatar_Click(object sender, EventArgs e)
        {
            CodeUsers codeUsers = new CodeUsers();

            lblErrorAvatar.Visible = true;

            if (fuImage.HasFile)
            {
                string imageErrorDescription = string.Empty;
                byte[] fileBytes = fuImage.FileBytes;
                string fileName = fuImage.FileName;
                string fileType = System.IO.Path.GetExtension(fileName);

                bool resizeAvatar = false;

                if (CodeTools.IsValidImage(fileName, fileBytes))
                {
                    int width = 0;
                    int height = 0;
                    if (CodeTools.GetImageWidthAndHeight(fileBytes, out width, out height))
                    {
                        if (width < 100 || height < 100)
                        {
                            lblErrorAvatar.Text = "Невалидна широчина и/или височина на картинката.";
                        }
                        else
                        {
                            if (width > 150)
                            {
                                resizeAvatar = true;
                            }

                            if (resizeAvatar)
                            {
                                CodeImages codeImages = new CodeImages();
                                currentUser.avatar = codeImages.GetResizedImgFromBytes(objectContext, fuImage.FileBytes, 150);
                            }
                            else
                            {
                                currentUser.avatar = fuImage.FileBytes;
                            }

                            CodeTools.Save(objectContext);

                            lblErrorAvatar.Visible = false;

                            ShowSuccessLable("Аватарът е подменен.");

                            FillUserInfo();
                        }
                    }
                    else
                    {
                        lblErrorAvatar.Text = "Невалидна широчина и/или височина на картинката.";
                    }
                }
                else
                {
                    lblErrorAvatar.Text = "Невалидна картинка.";
                }
            }
            else
            {
                if (currentUser.avatar != null)
                {
                    currentUser.avatar = null;

                    CodeTools.Save(objectContext);

                    ShowSuccessLable("Аватарът е премахнат.");

                    lblErrorAvatar.Visible = false;

                    FillUserInfo();
                }
                else
                {
                    lblErrorAvatar.Text = "Изберете картинка.";
                }
            }

        }

        protected void btnNewInfo_Click(object sender, EventArgs e)
        {
            lblErrorMoreInfo.Visible = true;

            CodeUsers codeUsers = new CodeUsers();

            if (currentUser.moreInfo != tbMoreInfo.Text)
            {
                if (CodeTools.ValidateInput(tbMoreInfo.Text, 0, 500, true))
                {

                    currentUser.moreInfo = tbMoreInfo.Text;

                    CodeTools.Save(objectContext);
                    ShowSuccessLable("Информацията е сменена!");

                    lblErrorMoreInfo.Visible = false;

                    tbMoreInfo.Text = "";

                    FillUserInfo();
                }
                else
                {
                    lblErrorMoreInfo.Text = "Невалидна информация.";
                }
            }
            else
            {
                lblErrorMoreInfo.Text = "Въведете информация различна от настоящата.";
            }

        }

        protected void btnNewBirthDate_Click(object sender, EventArgs e)
        {
            CodeUsers codeUsers = new CodeUsers();

            lblErrorBirthDate.Visible = true;

            bool passed = true;
            bool change = false;

            if (ddlDay.SelectedIndex > 0 || ddlMonth.SelectedIndex > 0 || ddlYear.SelectedIndex > 0)
            {
                if (ddlDay.SelectedIndex < 1)
                {
                    passed = false;
                }

                if (ddlMonth.SelectedIndex < 1)
                {
                    passed = false;
                }

                if (ddlYear.SelectedIndex < 1)
                {
                    passed = false;
                }
            }
            else
            {
                if (currentUser.birthdate != null)
                {
                    lblErrorBirthDate.Visible = false;

                    currentUser.birthdate = null;

                    CodeTools.Save(objectContext);

                    ShowSuccessLable("Рожденната дата е изтрита.");

                    change = true;

                    FillUserInfo();
                }
                else
                {
                    lblErrorBirthDate.Text = "Изберете дата.";
                    passed = false;
                }
            }

            if (change == false)
            {
                if (passed)
                {
                    
                        string date = String.Format("{0}/{1}/{2}", ddlDay.SelectedIndex, ddlMonth.SelectedIndex, ddlYear.SelectedValue);
                        DateTime newDate = new DateTime();
                        if (DateTime.TryParse(date, out newDate))
                        {
                            if (newDate != currentUser.birthdate)
                            {
                                currentUser.birthdate = newDate;

                                CodeTools.Save(objectContext);

                                ShowSuccessLable("Рожденната дата е обновена.");

                                lblErrorBirthDate.Visible = false;

                                FillDDLists();

                                FillUserInfo();
                            }
                            else
                            {
                                lblErrorBirthDate.Text = "Изберете дата различна от настоящата.";
                            }
                        }
                        else
                        {
                            throw new Exception("couldnt parse to date");
                        }
                
                }
                else
                {
                    lblErrorBirthDate.Text = "Не правилно избрана дата.";
                }
            }
        }


        private void FillUserInfo()
        {

            if (currentUser.avatar != null)
            {
                giCurrUser.Parameters.Clear();

                Microsoft.Web.ImageParameter avatarIDParam = new Microsoft.Web.ImageParameter();
                avatarIDParam.Name = "avatarID";
                avatarIDParam.Value = currentUser.ID.ToString();
                giCurrUser.Parameters.Add(avatarIDParam);
                giCurrUser.Width = 150;
                giCurrUser.ImageAlign = ImageAlign.Left;

                giCurrUser.Visible = true;
            }
            else
            {
                giCurrUser.Visible = false;
            }

 
            StringBuilder userInfo = new StringBuilder();

            userInfo.Append("Потребителско име : " + currentUser.username + "<br />");
            userInfo.Append("Регистриран на : " + currentUser.dateRegistered.ToLocalTime() + " <br />");

            if (!string.IsNullOrEmpty(currentUser.name))
            {
                userInfo.Append("Име : " + currentUser.name + " <br />");
            }
            if (!string.IsNullOrEmpty(currentUser.city))
            {
                userInfo.Append("Град : " + currentUser.city + " <br />");
            }
            if (!string.IsNullOrEmpty(currentUser.email))
            {
                userInfo.Append("Емайл : " + currentUser.email + " <br />");
            }
            if (currentUser.birthdate != null)
            {
                userInfo.Append("Роден на : " + currentUser.birthdate.Value.ToShortDateString() + " <br />");
            }
            if (!string.IsNullOrEmpty(currentUser.moreInfo))
            {
                userInfo.Append("Повече информация : " + CodeTools.GetFormattedTextFromDB(currentUser.moreInfo));
            }

            lblUserInfo.Text = userInfo.ToString();
        }





    }
}
