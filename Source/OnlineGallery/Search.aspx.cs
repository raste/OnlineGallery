﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineGallery
{
    public partial class Search : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();
        protected string search = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetSearchString();
            ShowInfo();
        }

        private void ShowInfo()
        {
            lblSearchFor.Text = string.Format("Резултати в търсенето за '{0}'", search);

            FillTblSearchResults();
        }

        private void FillTblSearchResults()
        {
            tblSearchResults.Rows.Clear();

            if (!string.IsNullOrEmpty(search))
            {
                IEnumerable<User> foundUsers = objectContext.UserSet.Where(usr => usr.username.Contains(search));
                if (foundUsers != null && foundUsers.Count() > 0)
                {

                    CodeImages codeImages = new CodeImages();
                    CodeComments codeComments = new CodeComments();

                    foreach (User user in foundUsers)
                    {
                        if (user.name != "system")
                        {
                            TableRow newRow = new TableRow();
                            tblSearchResults.Rows.Add(newRow);

                            TableCell newCell = new TableCell();
                            newRow.Cells.Add(newCell);

                            newCell.Width = Unit.Percentage(100);

                            Table newTable = new Table();
                            newCell.Controls.Add(newTable);
                            newTable.CssClass = "commentTableStyle";

                            TableRow row = new TableRow();
                            newTable.Rows.Add(row);

                            if (user.avatar != null)
                            {
                                TableCell avatarCell = new TableCell();
                                row.Cells.Add(avatarCell);
                                avatarCell.Width = Unit.Pixel(10);
                                avatarCell.CssClass = "imageBackground";

                                Microsoft.Web.GeneratedImage newGeneratedImage = new Microsoft.Web.GeneratedImage();
                                avatarCell.Controls.Add(newGeneratedImage);
                                newGeneratedImage.ImageHandlerUrl = "~/GetImageHandler.ashx";
                                newGeneratedImage.Width = 100;

                                Microsoft.Web.ImageParameter imageIDParam = new Microsoft.Web.ImageParameter();
                                imageIDParam.Name = "avatarID";
                                imageIDParam.Value = user.ID.ToString();
                                newGeneratedImage.Parameters.Add(imageIDParam);
                            }

                            TableCell infoCell = new TableCell();
                            row.Cells.Add(infoCell);
                            infoCell.VerticalAlign = VerticalAlign.Top;

                            infoCell.Controls.Add(CodeTools.GetLbl("Потребител : "));

                            HyperLink userLink = new HyperLink();
                            infoCell.Controls.Add(userLink);
                            userLink.NavigateUrl = "Album.aspx?user=" + user.ID;
                            userLink.Text = user.username;

                            Label infoLbl = new Label();
                            infoCell.Controls.Add(infoLbl);
                            infoLbl.Text = string.Format("<br />Качени снимки : {0}", codeImages.CountUserImages(objectContext, user));

                            infoCell.Controls.Add(CodeTools.GetLbl("<br />Коментари : " + codeComments.CountUserComments(objectContext, user)));
                        }
                    }
                }
                else
                {
                    tblSearchResults.Rows.Add(GetNoDataRow());
                }
            }
            else
            {
                tblSearchResults.Rows.Add(GetNoDataRow());
            }
        }

        private TableRow GetNoDataRow()
        {
            TableRow noDataRow = new TableRow();

            TableCell noDataCell = new TableCell();
            noDataRow.Cells.Add(noDataCell);
            noDataCell.Text = string.Format("Няма резултати за '{0}'", search);

            return noDataRow;
        }

        private void GetSearchString()
        {
            search = Request["string"];
        }






    }
}
