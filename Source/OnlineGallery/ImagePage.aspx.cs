﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineGallery
{
    public partial class ImagePage : System.Web.UI.Page
    {

        protected Entities objectContext = new Entities();
        protected User currentUser = null;
        protected Image currentImage = null;
        protected bool owner = false;

        protected long maxCommentsFromUser = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            ShowInfo();

            pnlNotification.Visible = false;
        }

        private void ShowInfo()
        {
            
            ShowImage();

            NextAndPrevImages();
            FillTblComments();

            ImageInfo();

            ImageStats();

            lblError.Visible = false;

            if (!string.IsNullOrEmpty(currentImage.description))
            {
                lblImgDescription.Text = "<br />" + CodeTools.GetFormattedTextFromDB(currentImage.description);
                lblImgDescription.Visible = true;
            }
            else
            {
                lblImgDescription.Visible = false;
            }

            
        }

        private void ImageStats()
        {

            lblComments.Text = currentImage.commens.ToString();
            lblVisits.Text = currentImage.visits.ToString();
            lblLiked.Text = currentImage.liked.ToString();
            lblDisliked.Text = currentImage.disliked.ToString();

           // lblImgStats.Text = string.Format("Посещения : {0} , Коментари : {1} , Харесана {2} / Нехаресана {3}"
           //     , currentImage.visits, currentImage.commens, currentImage.liked, currentImage.disliked);
        }

        private void ImageInfo()
        {
            
            if(!currentImage.UserReference.IsLoaded)
            {
                currentImage.UserReference.Load();
            }

            lblUploadedBy.Text = "Качена от&nbsp;";

            hlToImgsUser.NavigateUrl = "Album.aspx?user=" + currentImage.User.ID;
            hlToImgsUser.Text = currentImage.User.username;

            lblDateUploaded.Text = "&nbsp;на " + currentImage.dateUploaded.ToLocalTime().ToString();
            lblImgCategory.Text = "&nbsp;в категория ";

            if (!currentImage.CategoryReference.IsLoaded)
            {
                currentImage.CategoryReference.Load();
            }

            hlImgCategory.Text = currentImage.Category.name;
            hlImgCategory.NavigateUrl = hlToImgsUser.NavigateUrl + "&cat=" + currentImage.Category.ID;
        }

        private void ShowImage()
        {
            //imgCurrImage.ImageUrl = string.Format("GetImage.aspx?ImageID={0}&thumb=true&medThumb=true", currentImage.ID);

            hlToImage.NavigateUrl = "~/GetImageHandler.ashx?imageID="+ currentImage.ID.ToString();

            int width = 0;
            int height = 0;
            CodeImages.GetImageWidthAndHeight(currentImage.image, out width, out height);
   
            giCurrImg.ImageHandlerUrl = "~/GetImageHandler.ashx";
            if (width > 770)
            {
                giCurrImg.Width = 770;
            }
            else
            {
                giCurrImg.Width = width;
            }

            giCurrImg.Parameters.Clear();

            Microsoft.Web.ImageParameter imageIDParam = new Microsoft.Web.ImageParameter();
            imageIDParam.Name = "imageID";
            imageIDParam.Value = currentImage.ID.ToString();
            giCurrImg.Parameters.Add(imageIDParam);

            Microsoft.Web.ImageParameter thumbParam = new Microsoft.Web.ImageParameter();
            thumbParam.Name = "thumb";
            thumbParam.Value = "True";
            giCurrImg.Parameters.Add(thumbParam);

            Microsoft.Web.ImageParameter medThumbParam = new Microsoft.Web.ImageParameter();
            medThumbParam.Name = "medThumb";
            medThumbParam.Value = "True";
            giCurrImg.Parameters.Add(medThumbParam);

            //imgCurrImage.Width = Unit.Pixel(980);
            
        }

        private void NextAndPrevImages()
        {
            CodeImages codeImages = new CodeImages();
            Image nextImage = codeImages.GetUserNextImage(objectContext, currentImage);
            Image prevImage = codeImages.GetUserPreviusImage(objectContext, currentImage);
            if (nextImage != null)
            {
                //hlNextImage.NavigateUrl = "ImagePage.aspx?image=" + nextImage.ID;
                imgNext.PostBackUrl = "ImagePage.aspx?image=" + nextImage.ID;
                imgNext.Visible = true;
            }
            else
            {
                //hlNextImage.Visible = false;
                imgNext.Visible = false;
            }

            if (prevImage != null)
            {
                //hlPrevImage.NavigateUrl = "ImagePage.aspx?image=" + prevImage.ID;
                imgPrev.PostBackUrl = "ImagePage.aspx?image=" + prevImage.ID;
                imgPrev.Visible = true;
            }
            else
            {
                //hlPrevImage.Visible = false;
                imgPrev.Visible = false;
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

            CheckImageParams();
            CommentsPanel();
            RateTable();
            VisitImage();
            OwnerOptions();
        }

        private void OwnerOptions()
        {
            if (currentUser != null)
            {
                if (!currentImage.UserReference.IsLoaded)
                {
                    currentImage.UserReference.Load();
                }

                if (currentUser == currentImage.User)
                {
                    pnlEdit.Visible = true;

                    hlEditImage.NavigateUrl = "EditImage.aspx?image=" + currentImage.ID;
                }
            }
        }

        private void VisitImage()
        {
            if (currentUser != null)
            {
                CodeVisits codeVisits = new CodeVisits();
                if (codeVisits.IsUserVisitngForFirstTime(objectContext, currentImage, currentUser))
                {
                    codeVisits.AddVisit(objectContext, currentImage, currentUser);
                }
            }
        }

        private void RateTable()
        {
            if (currentUser != null)
            {
                CodeRatings codeRatings = new CodeRatings();
                if (codeRatings.CanUserRateImate(objectContext, currentImage, currentUser))
                {
                    tblRateImg.Visible = true;
                }
                else
                {
                    tblRateImg.Visible = false;
                }
            }
            else
            {
                tblRateImg.Visible = false;
            }
        }

        private void CommentsPanel()
        {
            if (currentUser != null)
            {
                pnlAddComment.Visible = true;

                CodeComments codeComments = new CodeComments();
                long commFromUser = codeComments.CountUserCommentsOnImage(objectContext, currentImage, currentUser);
                if (commFromUser >= maxCommentsFromUser)
                {
                    pnlAddComment.Visible = false;
                }
                else
                {
                    pnlAddComment.Visible = true;
                }
            }
            else
            {
                pnlAddComment.Visible = false;
            }

        }

        private void CheckImageParams()
        {
            string strImgId = Request["image"];
            if (string.IsNullOrEmpty(strImgId))
            {
                Response.Redirect("Home.aspx");
            }

            long imgID = -1;
            if (!long.TryParse(strImgId, out imgID))
            {
                Response.Redirect("Home.aspx");
            }

            CodeImages codeImages = new CodeImages();
            Image currImage = codeImages.Get(objectContext, imgID);
            if (currImage == null)
            {
                Response.Redirect("Home.aspx");
            }

            currentImage = currImage;

            if (!currentImage.UserReference.IsLoaded)
            {
                currentImage.UserReference.Load();
            }

            if (currentUser == currentImage.User)
            {
                owner = true;
            }
        }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            CodeComments codeComments = new CodeComments();

            if (CodeTools.ValidateInput(tbComment.Text, 10, 500, true))
            {
                ImageComment newComment = new ImageComment();

                newComment.User = currentUser;
                newComment.Image = currentImage;
                newComment.dateWritten = DateTime.UtcNow;
                newComment.description = tbComment.Text;

                codeComments.AddComment(objectContext, newComment);

                ShowNotificationLabel("Коментарът беше добавен !");

                tbComment.Text = "";

                ShowInfo();
                CommentsPanel();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Невалидно съдържание на коментара.";
            }
        }

        private void FillTblComments()
        {
            tblComments.Rows.Clear();

            CodeComments codeComments = new CodeComments();
            List<ImageComment> comments = codeComments.GetImageComments(objectContext, currentImage);

            if (comments != null && comments.Count > 0)
            {
                User commenter = null;

                int i = 0;

                foreach (ImageComment comment in comments)
                {
                    TableRow newRow = new TableRow();
                    tblComments.Rows.Add(newRow);

                    TableCell newCell = new TableCell();
                    newRow.Cells.Add(newCell);

                    newCell.Width = Unit.Percentage(100);

                    Table newTable = new Table();
                    newCell.Controls.Add(newTable);
                    newTable.CssClass = "commentTableStyle";

                    if (i % 2 == 0)
                    {
                        newTable.BackColor = System.Drawing.Color.FromArgb(85,85,85);
                    }

                    TableRow row = new TableRow();
                    newTable.Rows.Add(row);

                    if (!comment.UserReference.IsLoaded)
                    {
                        comment.UserReference.Load();
                    }

                    commenter = comment.User;

                    if (commenter.avatar != null)
                    {
                        TableCell avatarCell = new TableCell();
                        row.Cells.Add(avatarCell);
                        avatarCell.Width = Unit.Pixel(10);
                        avatarCell.CssClass = "commentsTableAvatarCell";

                        Microsoft.Web.GeneratedImage newGeneratedImage = new Microsoft.Web.GeneratedImage();
                        avatarCell.Controls.Add(newGeneratedImage);
                        newGeneratedImage.ImageHandlerUrl = "~/GetImageHandler.ashx";
                        newGeneratedImage.Width = 100;

                        Microsoft.Web.ImageParameter imageIDParam = new Microsoft.Web.ImageParameter();
                        imageIDParam.Name = "avatarID";
                        imageIDParam.Value = commenter.ID.ToString();
                        newGeneratedImage.Parameters.Add(imageIDParam);
                    }

                    TableCell userCell = new TableCell();
                    userCell.VerticalAlign = VerticalAlign.Top;
                    userCell.CssClass = "padding";
                    row.Cells.Add(userCell);

                    HyperLink userLink = new HyperLink();
                    userCell.Controls.Add(userLink);
                    userLink.Text = commenter.username;
                    userLink.NavigateUrl= "Album.aspx?user=" + commenter.ID;

                    Label dateLbl = new Label();
                    userCell.Controls.Add(dateLbl);
                    dateLbl.Text = string.Format("&nbsp;&nbsp;({0})", comment.dateWritten.ToLocalTime().ToString());

                    System.Web.UI.HtmlControls.HtmlGenericControl hr = new System.Web.UI.HtmlControls.HtmlGenericControl("hr");
                    userCell.Controls.Add(hr);

                    Label descrText = new Label();
                    userCell.Controls.Add(descrText);
                    descrText.Text = CodeTools.GetFormattedTextFromDB(comment.description);

                    i++;
                }

            }
            else
            {
                TableRow lastRow = new TableRow();
                tblComments.Rows.Add(lastRow);

                TableCell lastCell = new TableCell();
                lastRow.Cells.Add(lastCell);

                lastCell.Text = "Няма добавени коментари към тази картинка.";
            }

        }

        protected void btnLikeImg_Click(object sender, ImageClickEventArgs e)
        {
            CodeRatings codeRatins = new CodeRatings();
            codeRatins.RateImage(objectContext, currentImage, currentUser, 1);

            ShowNotificationLabel("Картинката е оценена успешно.");

            ShowInfo();
            RateTable();
        }

        private void ShowNotificationLabel(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception();
            }

            pnlNotification.Visible = true;
            lblOperSuccess.Text = "<br />" + text;
        }

        protected void btnDislikeImg_Click(object sender, ImageClickEventArgs e)
        {
            CodeRatings codeRatins = new CodeRatings();
            codeRatins.RateImage(objectContext, currentImage, currentUser, -1);

            ShowNotificationLabel("Картинката е оценена успешно.");

            ShowInfo();
            RateTable();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CodeImages codeImages = new CodeImages();

            if (!currentImage.UserReference.IsLoaded)
            {
                currentImage.UserReference.Load();
            }

            codeImages.DeleteImage(objectContext, currentImage);

            Response.Redirect("Album.aspx?user=" + currentUser.ID);
        }

    }
}
