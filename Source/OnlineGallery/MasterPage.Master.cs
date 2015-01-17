﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineGallery
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected Entities objectContext = new Entities();
        protected User currentUser = null;
        protected Category currentCategory = null;

        protected User visitedUser = null;
        protected bool visiting = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
        }

        private void FillUserPanels()
        {
            if (currentUser != null)
            {
                pnlCurrUser.Visible = true;
                pnlUserOptions.Visible = true;

                pnlLogIn.Visible = false;

                hlAlbum.Visible = true;
                hlAlbum.NavigateUrl = string.Format("Album.aspx?user={0}&cat=1", currentUser.ID);

                hlUserLink.Text = currentUser.username;
                hlUserLink.NavigateUrl = "Album.aspx?user=" + currentUser.ID;
                hlEditProfile.NavigateUrl = "EditUser.aspx?user=" + currentUser.ID;

                FillCategories();
            }
            else
            {
                hlAlbum.Visible = false;

                pnlCurrUser.Visible = false;
                pnlUserOptions.Visible = false;

                pnlLogIn.Visible = true;
            }
        }

        public void CheckUser()
        {
            CheckUserParams();
            CheckCategoryParams();
            FillUserPanels();
            FillVisitedUserOptions();
        }

        private void FillVisitedUserOptions()
        {
            if (visiting)
            {
                if (visitedUser == null)
                {
                    throw new Exception();
                }

                pnlVisitedUser.Visible = true;

                hlVisitedUser.Text = visitedUser.username;
                hlVisitedUser.NavigateUrl = string.Format("Album.aspx?user={0}&cat=1", visitedUser.ID);

                FillVisitedCategories();

                if (currentUser == null)
                {
                    hrVisiting.Visible = false;
                }
                else
                {
                    hrVisiting.Visible = true;
                }
            }
            else
            {
                pnlVisitedUser.Visible = false;
            }
        }

        private void CheckUserParams()
        {
            CodeUsers codeUsers = new CodeUsers();
            User currUser = codeUsers.GetUser(objectContext, CodeTools.GetCurrentUserId(), false);

            if (currUser != null)
            {
                currentUser = currUser;
            }

            string currUrl = Request.Url.ToString();

            if (currUrl.Contains("Album.aspx?"))
            {
                string strUserId = Request["user"];
                if (!string.IsNullOrEmpty(strUserId))
                {
                    long userID = -1;
                    if (long.TryParse(strUserId, out userID))
                    {
                        User user = codeUsers.GetUser(objectContext, userID, false);
                        if (user == null)
                        {
                            Response.Redirect("Home.aspx");
                        }
                        else
                        {
                            if (user != currentUser)
                            {
                                visiting = true;
                                visitedUser = user;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("Home.aspx");
                    }
                }
            }
            else if (currUrl.Contains("ImagePage.aspx?"))
            {
                string strImageId = Request["image"];
                if (!string.IsNullOrEmpty(strImageId))
                {
                    long imageID = -1;
                    if (long.TryParse(strImageId, out imageID))
                    {
                        CodeImages codeImages = new CodeImages();

                        Image image = codeImages.Get(objectContext, imageID);
                        if (image == null)
                        {
                            Response.Redirect("Home.aspx");
                        }
                        else
                        {
                            if (!image.UserReference.IsLoaded)
                            {
                                image.UserReference.Load();
                            }

                            if (image.User != currentUser)
                            {
                                visiting = true;
                                visitedUser = image.User;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("Home.aspx");
                    }
                }
            }
        }

        private void CheckCategoryParams()
        {
            string strCatId = Request["cat"];

            Category currCategory = null;
            CodeCategories codeCategories = new CodeCategories();
            if (!string.IsNullOrEmpty(strCatId))
            {
                long catId = -1;
                long.TryParse(strCatId, out catId);
                currCategory = codeCategories.Get(objectContext, catId);
                if (currCategory == null)
                {
                    Response.Redirect("Home.aspx");
                }
            }
            else
            {
                if (Request.Url.ToString().Contains("Album.aspx?"))
                {
                    Category primary = codeCategories.GetPrimaryCategory(objectContext);
                    currCategory = primary;
                }
            }
            currentCategory = currCategory;
        }


        protected void btnLogIN_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(tbUsername.Text) && !string.IsNullOrEmpty(tbPassword.Text))
            {
                CodeUsers codeUsers = new CodeUsers();
                long userID = codeUsers.Login(objectContext, tbUsername.Text, tbPassword.Text);

                if (userID > 0)
                {
                    Session[CodeTools.CurrentUserIdKey] = userID;
                    Response.Redirect(Request.Url.ToString());
                }
            }
            else
            {
                Session[CodeTools.CurrentUserIdKey] = null;
            }
        }

        protected void lbLogOut_Click(object sender, EventArgs e)
        {
            Session[CodeTools.CurrentUserIdKey] = null;
            Response.Redirect(Request.Url.ToString());
        }



        public void UpdateObjectContext()
        {
            objectContext = new Entities();
        }
        
        private void FillCategories()
        {
            tblCategories.Rows.Clear();

            string url = "Album.aspx?user=" + currentUser.ID;

            CodeCategories codeCategories = new CodeCategories();
            
            //TableRow primaryRow = new TableRow();
            //tblCategories.Rows.Add(primaryRow);

            //TableCell primaryCell = new TableCell();
            //primaryRow.Cells.Add(primaryCell);

            int i = 1;

            //primaryCell.Controls.Add(CodeTools.CategoryItemLbl());

            //if (visiting == false && currentCategory == primary)
            //{
            //    //primaryCell.Controls.Add(CodeTools.GetLbl(primary.name));
            //    primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, false, url));
            //}
            //else
            //{
            //    //HyperLink primaryLink = new HyperLink();
            //    //primaryLink.CssClass = "cat";
            //    //primaryLink.Text = primary.name;
            //    //primaryLink.NavigateUrl = url + "&cat=" + primary.ID;
            //    //primaryCell.Controls.Add(primaryLink);

            //    primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, true, url));
            //}

            //Category primary = codeCategories.GetPrimaryCategory(objectContext);
            //primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, true, url));

            List<Category> primaryCategories = codeCategories.GetPrimaryCategories(objectContext);
            if(primaryCategories.Count == 0)
            {
                throw new Exception();
            }

            string strUserID = Request["user"];

            foreach(Category category in primaryCategories)
            {
                TableRow newRow = new TableRow();
                tblCategories.Rows.Add(newRow);

                TableCell newCell = new TableCell();
                newRow.Cells.Add(newCell);

                //newCell.Controls.Add(CodeTools.CategoryItemLbl());

                if (visiting == false && currentCategory == category && !string.IsNullOrEmpty(strUserID))
                {
                    //newCell.Controls.Add(CodeTools.GetLbl(category.name));
                    newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, false, url));
                }
                else
                {
                    //HyperLink newLink = new HyperLink();
                    //newLink.CssClass = "cat";
                    //newLink.Text = category.name;
                    //newLink.NavigateUrl = url + "&cat=" + category.ID;
                    //newCell.Controls.Add(newLink);

                    newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, true, url));
                } 

                i++;
            }

            List<Category> categories = codeCategories.GetUserCategories(objectContext, currentUser);
            if (categories != null && categories.Count > 0)
            {
                
                foreach (Category category in categories)
                {
                    TableRow newRow = new TableRow();
                    tblCategories.Rows.Add(newRow);

                    TableCell newCell = new TableCell();
                    newRow.Cells.Add(newCell);

                    //newCell.Controls.Add(CodeTools.CategoryItemLbl());

                    if (visiting == false && currentCategory == category)
                    {
                        //newCell.Controls.Add(CodeTools.GetLbl(category.name));
                        newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, false, url));
                    }
                    else
                    {
                        //HyperLink newLink = new HyperLink();
                        //newLink.CssClass = "cat";
                        //newLink.Text = category.name;
                        //newLink.NavigateUrl = url + "&cat=" + category.ID;
                        //newCell.Controls.Add(newLink);

                        newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, true, url));
                    } 

                    i++;
                }
            }

        }

        private void FillVisitedCategories()
        {
            tblVisitedCategories.Rows.Clear();

            if (visitedUser == null)
            {
                throw new Exception();
            }

            string url = "Album.aspx?user=" + visitedUser.ID;

            CodeCategories codeCategories = new CodeCategories();
            Category primary = codeCategories.GetPrimaryCategory(objectContext);

            //TableRow primaryRow = new TableRow();
            //tblVisitedCategories.Rows.Add(primaryRow);

            //TableCell primaryCell = new TableCell();
            //primaryRow.Cells.Add(primaryCell);

            int i = 1;

            //primaryCell.Controls.Add(CodeTools.CategoryItemLbl());

            //if (visiting == true && currentCategory == primary)
            //{
            //    //primaryCell.Controls.Add(CodeTools.GetLbl(primary.name));
            //    primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, false, url));
            //}
            //else
            //{
            //    //HyperLink primaryLink = new HyperLink();
            //    //primaryLink.CssClass = "cat";
            //    //primaryLink.Text = primary.name;
            //    //primaryLink.NavigateUrl = url + "&cat=" + primary.ID;
            //    //primaryCell.Controls.Add(primaryLink);
            //    primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, true, url));
            //}

            string strUserId = Request["user"];

            //if (!string.IsNullOrEmpty(strUserId) && currentCategory == primary && visiting == true)
            //{
            //    primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, false, url));
            //}
            //else
            //{
            //    primaryCell.Controls.Add(CodeTools.GetCategoryLink(i, primary, true, url));
            //}
           
            /////////////

            List<Category> primaryCategories = codeCategories.GetPrimaryCategories(objectContext);
            if (primaryCategories.Count == 0)
            {
                throw new Exception();
            }

            bool imgsInPrimCat = false;

            foreach (Category category in primaryCategories)
            {
                imgsInPrimCat = codeCategories.UserHaveInagesInPrimaryCategory(objectContext, visitedUser, category);

                if (imgsInPrimCat == true || category == primary)
                {
                    TableRow newRow = new TableRow();
                    tblVisitedCategories.Rows.Add(newRow);

                    TableCell newCell = new TableCell();
                    newRow.Cells.Add(newCell);

                    //newCell.Controls.Add(CodeTools.CategoryItemLbl());

                    if (!string.IsNullOrEmpty(strUserId) && currentCategory == category)
                    {
                        newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, false, url));
                    }
                    else
                    {
                        newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, true, url));
                    }

                    i++;
                }
            }

            ////////////

            List<Category> categories = codeCategories.GetUserCategories(objectContext, visitedUser);
            if (categories != null && categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    i++;

                    TableRow newRow = new TableRow();
                    tblVisitedCategories.Rows.Add(newRow);

                    TableCell newCell = new TableCell();
                    newRow.Cells.Add(newCell);

                    //newCell.Controls.Add(CodeTools.CategoryItemLbl());

                    if (visiting == true && currentCategory == category)
                    {
                        //newCell.Controls.Add(CodeTools.GetLbl(category.name));
                        newCell.Controls.Add(CodeTools.GetCategoryLink(i,category,false,url));
                    }
                    else
                    {
                        //HyperLink newLink = new HyperLink();
                        //newLink.CssClass = "cat";
                        //newLink.Text = category.name;
                        //newLink.NavigateUrl = url + "&cat=" + category.ID;
                        //newCell.Controls.Add(newLink);
                        newCell.Controls.Add(CodeTools.GetCategoryLink(i, category, true, url));
                    }
                }
            }

        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Search.aspx?string=" + tbSearch.Text);
        }

   


    }
}
