﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace OnlineGallery
{
    public class CodeTools
    {
        public static string CurrentUserIdKey = "CurrentUserId";

        public static long GetCurrentUserId()
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext == null)
            {
                throw new InvalidOperationException("The current HttpContext is not available.");
            }

            long currentUserId = -1;
            object currentUserIdObj = currentContext.Session[CurrentUserIdKey];
            if (currentUserIdObj != null)
            {
                try
                {
                    currentUserId = Convert.ToInt64(currentUserIdObj);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        string.Format("The current user ID is not a \"{0}\".",
                        typeof(long).FullName),
                        ex);
                }
            }

            return currentUserId;
        }

        public static void CheckObjectContext(Entities objectContext)
        {
            if (objectContext == null)
            {
                throw new NullReferenceException("objectContext");
            }
        }

        public static void Save(Entities objectContext)
        {
            CheckObjectContext(objectContext);
            objectContext.SaveChanges();
        }


        public static bool ValidateInput(string input, int minLength, int maxLength, bool canContainIntervals)
        {
            if (minLength >= maxLength)
            {
                throw new Exception("minLength >= maxLength");
            }
            if (minLength < 0 || maxLength < 0)
            {
                throw new Exception("minLength < 0 || maxLength < 0");
            }

            if (string.IsNullOrEmpty(input)) 
            {
                if (minLength == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            bool passed = true;

            if (input.Length < minLength || input.Length > maxLength)
            {
                passed = false;
            }
            if (canContainIntervals == false)
            {
                if (input.Contains(' '))
                {
                    passed = false;
                }
            }
            else
            {
                if (input.StartsWith(" ") || input.EndsWith(" "))
                {
                    passed = false;
                }
            }

            return passed;
        }


        public static bool IsValidImage(string fileName, byte[] fileBytes)
        {
            bool imageOk = false;

            if (fileBytes != null)
            {
                int maxFileLength = 2097152;  // 2MB
                int minFileLength = 1024; // 1kb

                if (minFileLength < fileBytes.Length && fileBytes.Length <= maxFileLength)
                {
                    if (IsImage(fileName, fileBytes) == true)
                    {
                        imageOk = true;
                    }
                }
            }
           
            return imageOk;
        }

        public static bool IsImage(string fileName, byte[] fileContents)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentNullException("fileName is empty.");
            }
            if (fileContents == null)
            {
                throw new ArgumentNullException("fileContents");
            }
            if (fileContents.Length == 0)
            {
                throw new ArgumentNullException("fileContents is empty.");
            }

            bool result = false;
            try
            {
                string fileType = System.IO.Path.GetExtension(fileName);

                fileType = (fileType ?? string.Empty).ToUpper();
                switch (fileType)
                {
                    case ".JPG":
                        result = IsJpg(fileContents);
                        break;
                    case ".PNG":
                        result = IsPng(fileContents);
                        break;
                    case ".GIF":
                        result = IsGif(fileContents);
                        break;
                    case ".BMP":
                        result = IsBmp(fileContents);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cound not determine whether the file is an image", ex);
            }
            return result;
        }


        private static bool IsJpg(byte[] fileContents)
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException("fileContents");
            }
            if (fileContents.Length == 0)
            {
                throw new ArgumentNullException("fileContents is empty.");
            }

            byte[] jpgStart = new byte[] { 0xFF, 0xD8, 0xFF };
            bool isJpg = ByteArrayStartsWith(fileContents, jpgStart);
            return isJpg;
        }

        private static bool IsBmp(byte[] fileContents)
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException("fileContents");
            }
            if (fileContents.Length == 0)
            {
                throw new ArgumentNullException("fileContents is empty.");
            }

            byte[] jpgStart = new byte[] { 0x42, 0x4D };
            bool isJpg = ByteArrayStartsWith(fileContents, jpgStart);
            return isJpg;
        }


        private static bool IsPng(byte[] fileContents)
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException("fileContents");
            }
            if (fileContents.Length == 0)
            {
                throw new ArgumentNullException("fileContents is empty.");
            }

            byte[] pngStart = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
            bool isPng = ByteArrayStartsWith(fileContents, pngStart);
            return isPng;
        }

        private static bool IsGif(byte[] fileContents)
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException("fileContents");
            }
            if (fileContents.Length == 0)
            {
                throw new ArgumentNullException("fileContents is empty.");
            }

            byte[] pngStart = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
            bool isPng = ByteArrayStartsWith(fileContents, pngStart);
            return isPng;
        }


        private static bool ByteArrayStartsWith(byte[] byteArray, byte[] controlArray)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }
            if (controlArray == null)
            {
                throw new ArgumentNullException("controlArray");
            }
            if (controlArray.Length == 0)
            {
                throw new ArgumentNullException("controlArray is empty.");
            }

            bool beginningMatches = false;

            if (byteArray.Length > controlArray.Length)
            {
                bool difference = false;
                for (int i = 0; difference == false && i < controlArray.Length; i++)
                {
                    byte fileBute = byteArray[i];
                    byte controlByte = controlArray[i];
                    if (fileBute != controlByte)
                    {
                        difference = true;
                    }
                }
                if (difference == false)
                {
                    beginningMatches = true;
                }
            }
            return beginningMatches;
        }


        public static Boolean GetImageWidthAndHeight(byte[] fileContents, out int width, out int height)
        {
            width = 0;
            height = 0;

            using (System.IO.Stream strm = new System.IO.MemoryStream(fileContents, false))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(strm, true, true);
                height = img.Height;
                width = img.Width;
            }

            Boolean result = false;
            if (width > 0 && height > 0)
            {
                result = true;
            }
            return result;
        }




        public static TableRow GetPagesRow(long itemsNumber, long itemsPerPage, long currPage, string urlToAssignTo)
        {
            TableRow newRow = new TableRow();

            if (itemsNumber > 0 && itemsPerPage > 0 && currPage >= 0 &&
                !string.IsNullOrEmpty(urlToAssignTo) && itemsNumber > itemsPerPage)
            {

                string strPageParam = "";
                if (urlToAssignTo.Contains('?'))
                {
                    strPageParam = "&page=";
                }
                else
                {
                    strPageParam = "?page=";
                }

                long numOfPages = itemsNumber / itemsPerPage;
                if ((itemsNumber % itemsPerPage) > 0)
                {
                    numOfPages++;
                }

                TableCell pagesCell = new TableCell();
                pagesCell.Text = "Страница : ";
                newRow.Cells.Add(pagesCell);

                int numOfLinks = 5;

                if (numOfPages <= numOfLinks)
                {
                    for (int i = 1; i <= numOfPages; i++)
                    {
                        if (i != currPage)
                        {
                            newRow.Cells.Add(GetLinkCell(i.ToString(), string.Format("{0}{1}{2}", urlToAssignTo, strPageParam, i)));
                        }
                        else
                        {
                            TableCell currCell = new TableCell();
                            currCell.Text = currPage.ToString();
                            newRow.Cells.Add(currCell);
                        }
                    }
                }
                else
                {

                    newRow.Cells.Add(GetLinkCell(" << ", string.Format("{0}{1}1", urlToAssignTo, strPageParam)));

                    int pb = 0;    // pages behind
                    int pa = 0;    // pages after

                    int pagesBehindAndAfter = numOfLinks / 2;


                    for (int pagesBehind = pagesBehindAndAfter; pagesBehind > 0; pagesBehind--)
                    {
                        if (currPage - pagesBehind >= 1)
                        {
                            pb++;
                        }
                    }

                    for (int pagesAfter = 1; pagesAfter <= pagesBehindAndAfter; pagesAfter++)
                    {
                        if (currPage + pagesAfter <= numOfPages)
                        {
                            pa++;
                        }
                    }

                    if (pb < pagesBehindAndAfter)
                    {
                        pa += pagesBehindAndAfter - pb;
                    }
                    if (pa < pagesBehindAndAfter)
                    {
                        pb += pagesBehindAndAfter - pa;
                    }

                    for (long cp = (currPage - pb); cp < currPage; cp++)
                    {

                        newRow.Cells.Add(GetLinkCell(cp.ToString(), string.Format("{0}{1}{2}", urlToAssignTo, strPageParam, cp)));
                    }

                    TableCell currCell = new TableCell();
                    Label lblCurrPage = new Label();
                    lblCurrPage.Text = currPage.ToString();
                    currCell.Controls.Add(lblCurrPage);
                    newRow.Cells.Add(currCell);
                    for (long cp = (currPage + 1); cp <= (pa + currPage); cp++)
                    {
                        newRow.Cells.Add(GetLinkCell(cp.ToString(), string.Format("{0}{1}{2}", urlToAssignTo, strPageParam, cp)));
                    }

                    newRow.Cells.Add(GetLinkCell(" >> ", string.Format("{0}{1}{2}", urlToAssignTo, strPageParam, numOfPages)));
                }

            }

            return newRow;
        }

        public static TableCell GetLinkCell(String text, String URL)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("text is null or empty");
            }
            if (string.IsNullOrEmpty(URL))
            {
                throw new Exception("URL is null or empty");
            }

            TableCell newCell = new TableCell();
            HyperLink newHyper = new HyperLink();
            newHyper.Text = text;
            newHyper.NavigateUrl = URL;
            newCell.Controls.Add(newHyper);
            return newCell;
        }

        public static void GetFromItemNumberToItemNumber(long currPage, long itemsOnPage, out long from, out long to)
        {
            from = itemsOnPage * (currPage - 1);
            to = itemsOnPage * currPage;
        }

        public static String GetFormattedTextFromDB(String text)
        {
            if (text == null || text == string.Empty)
            {
                return text;
            }

            string FormattedStr;

            FormattedStr = text.Replace(Environment.NewLine, "<br/>");
            FormattedStr = FormattedStr.Replace("<br/> ", "<br/>&nbsp;");
            FormattedStr = FormattedStr.Replace("  ", " &nbsp;");

            return FormattedStr;
        }

        public static Label CategoryItemLbl()
        {
            Label newLabel = new Label();
            newLabel.Text = "&nbsp;-&nbsp;";
            return newLabel;
        }

        public static Label GetLbl(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception();
            }

            Label newLabel = new Label();
            newLabel.Text = text;
            return newLabel;
        }

        public static Table GetImageStatisticsTable(long comments, long visits, long liked, long disliked)
        {
            Table newTable = new Table();

            newTable.CssClass = "statisticsTable";
            newTable.CellPadding = 0;
            newTable.CellSpacing = 0;

            TableRow newRow = new TableRow();
            newTable.Rows.Add(newRow);

            TableCell imgCommCell = new TableCell();
            newRow.Cells.Add(imgCommCell);

            System.Web.UI.WebControls.Image imgComm = new System.Web.UI.WebControls.Image();
            imgCommCell.Controls.Add(imgComm);
            imgComm.Height = Unit.Pixel(20);
            imgComm.ImageUrl = "~/images/messages.png";
            imgComm.ToolTip = "коментари";

            TableCell commCell = new TableCell();
            newRow.Cells.Add(commCell);

            Label commLbl = new Label();
            commCell.Controls.Add(commLbl);
            commLbl.Text = comments.ToString() + "&nbsp;";

            /////////

            TableCell imgVisCell = new TableCell();
            newRow.Cells.Add(imgVisCell);

            System.Web.UI.WebControls.Image imgVis = new System.Web.UI.WebControls.Image();
            imgVisCell.Controls.Add(imgVis);
            imgVis.Height = Unit.Pixel(20);
            imgVis.ImageUrl = "~/images/visits.png";
            imgVis.ToolTip = "посещения";

            TableCell visCell = new TableCell();
            newRow.Cells.Add(visCell);

            Label visLbl = new Label();
            visCell.Controls.Add(visLbl);
            visLbl.Text = visits.ToString() + "&nbsp;";

            /////////

            TableCell imgLikedCell = new TableCell();
            newRow.Cells.Add(imgLikedCell);

            System.Web.UI.WebControls.Image imgLike = new System.Web.UI.WebControls.Image();
            imgLikedCell.Controls.Add(imgLike);
            imgLike.Height = Unit.Pixel(20);
            imgLike.ImageUrl = "~/images/rate_plus.png";
            imgLike.ToolTip = "харесана";

            TableCell likedCell = new TableCell();
            newRow.Cells.Add(likedCell);

            Label likedLbl = new Label();
            likedCell.Controls.Add(likedLbl);
            likedLbl.Text = liked.ToString() + "&nbsp;";

            /////////

            TableCell imgDisLikedCell = new TableCell();
            newRow.Cells.Add(imgDisLikedCell);

            System.Web.UI.WebControls.Image imgDisLike = new System.Web.UI.WebControls.Image();
            imgDisLikedCell.Controls.Add(imgDisLike);
            imgDisLike.Height = Unit.Pixel(20);
            imgDisLike.ImageUrl = "~/images/rate_minus.png";
            imgDisLike.ToolTip = "не харесана";

            TableCell dislikedCell = new TableCell();
            newRow.Cells.Add(dislikedCell);

            Label dislikedLbl = new Label();
            dislikedCell.Controls.Add(dislikedLbl);
            dislikedLbl.Text = disliked.ToString() + "&nbsp;";

            /////////

            return newTable;
        }

        public static Table GetCategoryLink(int i, Category category, bool asLink, string url)
        {
            if (category == null)
            {
                throw new Exception();
            }

            Table newTable = new Table();
            newTable.CellSpacing = 0;
            newTable.CellPadding = 0;
            newTable.CssClass = "statisticsTable";

            int catNum = (i % 7) + 1;

            TableRow newRow = new TableRow();
            newTable.Rows.Add(newRow);

            TableCell imgCell = new TableCell();
            newRow.Cells.Add(imgCell);

            System.Web.UI.WebControls.Image catImg = new System.Web.UI.WebControls.Image();
            imgCell.Controls.Add(catImg);
            catImg.ImageUrl = string.Format("~/images/cat{0}.png", catNum);
            catImg.Height = Unit.Pixel(20);

            TableCell catCell = new TableCell();
            newRow.Cells.Add(catCell);

            catCell.Controls.Add(GetLbl("&nbsp;"));

            if (asLink)
            {
                HyperLink catLink = new HyperLink();
                catLink.CssClass = "cat";
                catLink.Text = category.name;
                catLink.NavigateUrl = url + "&cat=" + category.ID;
                catCell.Controls.Add(catLink);
            }
            else
            {
                //catCell.Text = category.name;
                catCell.Controls.Add(GetLbl(category.name));
            }

            return newTable;

        }


    }
}
