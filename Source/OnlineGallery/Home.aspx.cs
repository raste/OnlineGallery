﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace OnlineGallery
{
    public partial class Home : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();
        protected long lastImageNum = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowLastImages();
        }

        private void ShowLastImages()
        {
            CodeImages codeImages = new CodeImages();
            List<Image> images = codeImages.GetLastImages(objectContext);

            if (images != null && images.Count > 0)
            {
                tblLastImages.Rows.Clear();

                long count = images.Count;
                if (count >= lastImageNum)
                {
                    count = lastImageNum;
                }

                lblLastImages.Text = string.Format("Последни {0} добавени снимки :", count);

                int imagesPerRow = 2;

                TableRow newRow = new TableRow();
                tblLastImages.Rows.Add(newRow);

                int i = 0;

                foreach (Image image in images)
                {
                    if (i < count)
                    {
                        if (newRow.Cells.Count == imagesPerRow)
                        {
                            newRow = new TableRow();
                            tblLastImages.Rows.Add(newRow);
                        }

                        TableCell newCell = new TableCell();

                        newCell.HorizontalAlign = HorizontalAlign.Center;
                        newRow.Cells.Add(newCell);

                        System.Web.UI.HtmlControls.HtmlGenericControl outsideDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                        newCell.Controls.Add(outsideDiv);
                        outsideDiv.Attributes.Add("class", "imageDiv");

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
                        newGeneratedImage.Width = 350;

                        Microsoft.Web.ImageParameter imageIDParam = new Microsoft.Web.ImageParameter();
                        imageIDParam.Name = "imageID";
                        imageIDParam.Value = image.ID.ToString();
                        newGeneratedImage.Parameters.Add(imageIDParam);

                        Microsoft.Web.ImageParameter thumbParam = new Microsoft.Web.ImageParameter();
                        thumbParam.Name = "homeThumb";
                        thumbParam.Value = "True";
                        newGeneratedImage.Parameters.Add(thumbParam);

                        //Microsoft.Web.ImageParameter medThumbParam = new Microsoft.Web.ImageParameter();
                        //medThumbParam.Name = "medThumb";
                        //medThumbParam.Value = "True";
                        //newGeneratedImage.Parameters.Add(medThumbParam);

                        newCell.Controls.Add(CodeTools.GetImageStatisticsTable(image.commens, image.visits, image.liked, image.disliked));

                        Label imgInfoLbl = new Label();
                        imgInfoLbl.Text = string.Format("&nbsp;({0})",image.dateUploaded.ToLocalTime().ToString());
                        newCell.Controls.Add(imgInfoLbl);
                        

                        if (!image.UserReference.IsLoaded)
                        {
                            image.UserReference.Load();
                        }

                        newCell.Controls.Add(CodeTools.GetLbl("<br />Качена от : "));

                        HyperLink byUser = new HyperLink();
                        newCell.Controls.Add(byUser);
                        byUser.Text = image.User.username;
                        byUser.NavigateUrl = string.Format("Album.aspx?user={0}&cat=1", image.User.ID);
                    }
                    else
                    {
                        break;
                    }

                    i++;
                }
            }
            else
            {
                pnlLastImages.Visible = false;
                tblLastImages.Visible = false;
            }

        }



    }
}
