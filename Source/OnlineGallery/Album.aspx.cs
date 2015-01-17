﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;

namespace OnlineGallery
{
    public partial class Album : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();
        protected User currentUser = null;

        protected bool visited = false;
        protected User visitedUser = null;

        protected Category currentCategory = null;

        protected bool primaryCategory = true;

        protected string sortBy = "date";
        protected long imageNumber = 0;
        protected long pageNumber = 1;
        protected long imagesPerPage = 12;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();

            // edit panel s opcii ako usera e owner (edit categories, delete category ako ne e primary)
            ShowInfo();

        }

        private void CheckSortByParams()
        {
            string strSortBy = Request["sortby"];
            if (!string.IsNullOrEmpty(strSortBy))
            {
                if (strSortBy != "date" && strSortBy != "rating" && strSortBy != "comments" && strSortBy != "visits")
                {
                    Response.Redirect(GetCurrAlbumLink());
                }
                else
                {
                    sortBy = strSortBy;
                }
            }
        }

        private void ShowInfo()
        {
            if (currentCategory.IsPrimary == false)
            {
                if (!string.IsNullOrEmpty(currentCategory.description))
                {
                    lblCatInfo.Text = "<br /> Описание : " + currentCategory.description;
                }
                else
                {
                    lblCatInfo.Visible = false;
                }
                lblCatName.Text = " Категория : " + currentCategory.name;
            }
            else
            {
                pnlCategoryInfo.Visible = false;
            }

            //ShowCategories();
            FillTblImages();
            FillPagesTBls();
            FillUserInfo();
            SortByLinks();
        }

        private void SortByLinks()
        {
            if (imageNumber > 5)
            {
                string url = string.Empty;

                if (primaryCategory == true)
                {
                    url = string.Format("Album.aspx?cat={0}", currentCategory.ID);
                }
                else
                {
                    url = string.Format("{0}&cat={1}", GetCurrAlbumLink(), currentCategory.ID);
                }

                hlSortByDate.NavigateUrl = string.Format("{0}&sortby=date", url);
                hlSortByRating.NavigateUrl = string.Format("{0}&sortby=rating", url);
                hlSortByVisits.NavigateUrl = string.Format("{0}&sortby=visits", url);
                hlSortByComments.NavigateUrl = string.Format("{0}&sortby=comments", url);
            }
            else
            {
                divSortBy.Visible = false;
            }
        }

        private void FillUserInfo()
        {
            if (primaryCategory == true)
            {
                userAccordion.Visible = false;
                return;
            }
            else
            {
                userAccordion.Visible = true;
            }


            User user = GetAlbumsUser();

            if (user.avatar != null)
            {
                //imgUserImg.ImageUrl = "GetImage.aspx?avatarID=" + user.ID;
                //imgUserImg.Height = Unit.Pixel(300);
                hlUserAvatar.NavigateUrl = "GetImage.aspx?avatarID=" + user.ID;

                generatedImgUserImg.Parameters.Clear();

                Microsoft.Web.ImageParameter avatarIDParam = new Microsoft.Web.ImageParameter();
                avatarIDParam.Name = "avatarID";
                avatarIDParam.Value = user.ID.ToString();
                generatedImgUserImg.Parameters.Add(avatarIDParam);
                generatedImgUserImg.Width = 150;
                generatedImgUserImg.ImageAlign = ImageAlign.Left;
            }
            else
            {
                hlUserAvatar.Visible = false;
            }

            lblUserName.Text = "Потребителско име : " + user.username;

            StringBuilder userInfo = new StringBuilder();

            userInfo.Append("Регистриран на : " + user.dateRegistered.ToLocalTime() + " <br />");
            if (!string.IsNullOrEmpty(user.name))
            {
                userInfo.Append("Име : " + user.name + " <br />");
            }
            if (!string.IsNullOrEmpty(user.city))
            {
                userInfo.Append("Град : " + user.city + " <br />");
            }
            if (!string.IsNullOrEmpty(user.email))
            {
                userInfo.Append("Емайл : " + user.email + " <br />");
            }
            if (user.birthdate != null)
            {
                userInfo.Append("Роден на : " + user.birthdate.Value.ToShortDateString() + " <br />");
            }
            if (!string.IsNullOrEmpty(user.moreInfo))
            {
                userInfo.Append("Повече информация : " + CodeTools.GetFormattedTextFromDB(user.moreInfo));
            }

            lblUserInfo.Text = userInfo.ToString();
        }

        private void FillPagesTBls()
        {
            if (imageNumber > imagesPerPage)
            {
                string url = string.Empty;
                if (primaryCategory == true)
                {
                    url = string.Format("Album.aspx?cat={0}&sortby={1}", currentCategory.ID, sortBy);
                }
                else
                {
                    url = string.Format("{0}&cat={1}&sortby={2}", GetCurrAlbumLink(), currentCategory.ID, sortBy);
                }
                
                tblPagesTop.Rows.Clear();
                tblPagesBtm.Rows.Clear();

                tblPagesTop.Rows.Add(CodeTools.GetPagesRow(imageNumber, imagesPerPage, pageNumber, url));
                tblPagesBtm.Rows.Add(CodeTools.GetPagesRow(imageNumber, imagesPerPage, pageNumber, url));
            }
            else
            {
                tblPagesBtm.Visible = false;
                tblPagesTop.Visible = false;
            }

        }

        private void FillTblImages()
        {
            tblImages.Rows.Clear();

            CodeImages codeImages = new CodeImages();

            User user = GetAlbumsUser();

            List<Image> images = codeImages.GetImagesInCategory(objectContext, user, primaryCategory, currentCategory, sortBy);

            if (images != null && images.Count > 0)
            {
                long from = (imagesPerPage * pageNumber) - imagesPerPage;
                long to = imagesPerPage * pageNumber;
                long i = 0;

                long imagesPerRow = 4;

                long imgOnCurrPage = imageNumber - (imagesPerPage * (pageNumber - 1));

                if (imgOnCurrPage >= imagesPerRow)
                {
                    tblImages.Width = Unit.Percentage(98);
                }

                TableRow newRow = new TableRow();
                tblImages.Rows.Add(newRow);

                foreach (Image image in images)
                {
                    if (i >= from && i < to)
                    {
                        if (newRow.Cells.Count == imagesPerRow)
                        {
                            newRow = new TableRow();
                            tblImages.Rows.Add(newRow);
                        }

                        TableCell newCell = new TableCell();
                        //if (imgOnCurrPage >= imagesPerRow)
                        //{
                        //    newCell.Width = Unit.Percentage(100 / imagesPerRow);
                        //}
                        newCell.HorizontalAlign = HorizontalAlign.Center;
                        newRow.Cells.Add(newCell);
                        newCell.CssClass = "paddingTB";

                        System.Web.UI.HtmlControls.HtmlGenericControl outsideDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                        newCell.Controls.Add(outsideDiv);
                        outsideDiv.Attributes.Add("class", "outsideDiv");

                        System.Web.UI.HtmlControls.HtmlGenericControl insideDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                        outsideDiv.Controls.Add(insideDiv);
                        insideDiv.Attributes.Add("class", "insideDiv");

                        HyperLink imgPage = new HyperLink();
                        insideDiv.Controls.Add(imgPage);

                        imgPage.NavigateUrl = "ImagePage.aspx?image=" + image.ID;

                        Microsoft.Web.GeneratedImage newGeneratedImage = new Microsoft.Web.GeneratedImage();
                        newGeneratedImage.CssClass = "albumImages";

                        imgPage.Controls.Add(newGeneratedImage);
                        newGeneratedImage.ImageHandlerUrl = "~/GetImageHandler.ashx";
                        newGeneratedImage.Width = 170;

                        Microsoft.Web.ImageParameter imageIDParam = new Microsoft.Web.ImageParameter();
                        imageIDParam.Name = "imageID";
                        imageIDParam.Value = image.ID.ToString();
                        newGeneratedImage.Parameters.Add(imageIDParam);

                        Microsoft.Web.ImageParameter thumbParam = new Microsoft.Web.ImageParameter();
                        thumbParam.Name = "thumb";
                        thumbParam.Value = "True";
                        newGeneratedImage.Parameters.Add(thumbParam);

                        /////////////////

                        newCell.Controls.Add(CodeTools.GetImageStatisticsTable(image.commens, image.visits, image.liked, image.disliked));

                        /*
                        Label imgInfoLbl = new Label();
                        imgInfoLbl.Text = string.Format("V:{3} , C:{0} , L:{1} / D:{2}", image.commens, image.liked, image.disliked, image.visits);
                        newCell.Controls.Add(imgInfoLbl);
                        */

                        if (primaryCategory == true)
                        {
                            if (!image.UserReference.IsLoaded)
                            {
                                image.UserReference.Load();
                            }

                            newCell.Controls.Add(CodeTools.GetLbl("<br />oт : "));

                            HyperLink byUser = new HyperLink();
                            newCell.Controls.Add(byUser);
                            byUser.Text = image.User.username;
                            byUser.NavigateUrl = string.Format("Album.aspx?user={0}&cat=1", image.User.ID);
                        }
                    }
                    i++;
                }
            }
            else
            {
                TableRow noDataRow = new TableRow();
                tblImages.Rows.Add(noDataRow);

                TableCell noDataCell = new TableCell();
                noDataRow.Cells.Add(noDataCell);

                noDataCell.Text = "Няма качени снимки в тази категория";
            }

        }

        private void CheckCategoryParams()
        {
            string strCatId = Request["cat"];
            string strUsrId = Request["user"];

            Category currCategory = null;
            CodeCategories codeCategories = new CodeCategories();
            if (string.IsNullOrEmpty(strCatId))
            {
                if (visited == false && currentUser == null)
                {
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    currCategory = codeCategories.GetPrimaryCategory(objectContext);
                }
            }
            else
            {
                long catId = -1;
                long.TryParse(strCatId, out catId);
                currCategory = codeCategories.Get(objectContext, catId);
                if (currCategory == null)
                {
                    Response.Redirect("Home.aspx");
                }

                if (currCategory.IsPrimary == true && visited == false && string.IsNullOrEmpty(strUsrId))
                {
                    primaryCategory = true;
                }
                else
                {
                    primaryCategory = false;
                }
            }

            currentCategory = currCategory;

            User user = GetAlbumsUser();
            CodeImages codeImages = new CodeImages();
            imageNumber = codeImages.CountImagesInCategory(objectContext, currentCategory, primaryCategory, user);
        }

        private string GetCurrAlbumLink()
        {
            string url = string.Empty;

            if (primaryCategory == false)
            {
                if (visited == false)
                {
                    if (currentUser == null)
                    {
                        throw new ArgumentNullException("current user");
                    }

                    url = "Album.aspx?user=" + currentUser.ID;
                }
                else
                {
                    if (visitedUser == null)
                    {
                        throw new ArgumentNullException("visitedUser");
                    }

                    url = "Album.aspx?user=" + visitedUser.ID;
                }
            }
            else
            {
                url = "Album.aspx?cat=" + currentCategory.ID;
            }

            return url;
        }

        /*
        private void ShowCategories()
        {
            tblCategories.Rows.Clear();

            CodeCategories codeCategories = new CodeCategories();
            Category primary = codeCategories.GetPrimaryCategory(objectContext);

            TableRow primaryRow = new TableRow();
            tblCategories.Rows.Add(primaryRow);

            TableCell primaryCell = new TableCell();
            primaryRow.Cells.Add(primaryCell);

            if (currentCategory != primary)
            {
                HyperLink primaryLink = new HyperLink();
                primaryLink.Text = primary.name;
                primaryLink.NavigateUrl = GetCurrAlbumLink() + "&cat=" + primary.ID;
                primaryCell.Controls.Add(primaryLink);
            }
            else
            {
                primaryCell.Text = primary.name;
            }

            User albumsUser = GetAlbumsUser();

            List<Category> categories = codeCategories.GetUserCategories(objectContext, albumsUser);
            foreach (Category category in categories)
            {
                TableRow newRow = new TableRow();
                tblCategories.Rows.Add(newRow);

                TableCell newCell = new TableCell();
                newRow.Cells.Add(newCell);

                if (currentCategory != category)
                {
                    HyperLink newLink = new HyperLink();
                    newLink.Text = category.name;
                    newLink.NavigateUrl = GetCurrAlbumLink() + "&cat=" + category.ID;
                    newCell.Controls.Add(newLink);
                }
                else
                {
                    newCell.Text = category.name;
                }
            }

        }
        */

        private void CheckUser()
        {
            CodeUsers codeUsers = new CodeUsers();
            User currUser = codeUsers.GetUser(objectContext, CodeTools.GetCurrentUserId(), false);
            if (currUser != null)
            {
                currentUser = currUser;
            }

            CheckUserParams();
            CheckCategoryParams();
            CheckPageParams();
            CheckSortByParams();
          
        }

        private void CheckPageParams()
        {
            string strPageNum = Request["page"];
            if (!string.IsNullOrEmpty(strPageNum))
            {
                long pageNum = -1;
                if (long.TryParse(strPageNum, out  pageNum))
                {
                    if (pageNum < 1)
                    {
                        Response.Redirect(GetCurrAlbumLink());
                    }
                    else
                    {
                        if (pageNum > 1)
                        {
                            long expression = imagesPerPage * (pageNum - 1);
                            if (imageNumber > expression)
                            {
                                // oK
                                pageNumber = pageNum;
                            }
                            else
                            {
                                Response.Redirect(GetCurrAlbumLink());
                            }

                        }
                    }
                }
                else
                {
                    Response.Redirect(GetCurrAlbumLink());
                }
            }
        }

        private void CheckUserParams()
        {
            string strUserId = Request["user"];

            if (!string.IsNullOrEmpty(strUserId))
            {
                long userID = -1;
                if (long.TryParse(strUserId, out userID))
                {
                    CodeUsers codeUsers = new CodeUsers();
                    User currUser = codeUsers.GetUser(objectContext, userID, false);
                    if (currUser == null)
                    {
                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        if (currUser != currentUser)
                        {
                            visited = true;
                            visitedUser = currUser;
                        }
                        else
                        {
                            visitedUser = null;
                            visited = false;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }
            }

        }


        private User GetAlbumsUser()
        {
            User albumsUser = null;

            if (visited == false)
            {
                albumsUser = currentUser;
            }
            else
            {
                albumsUser = visitedUser;
            }

            return albumsUser;
        }





    }
}
