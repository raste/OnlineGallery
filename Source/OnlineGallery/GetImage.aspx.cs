﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Web;

namespace OnlineGallery
{
    public partial class GetImage : System.Web.UI.Page
    {
        protected Entities objectContext = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            string strImgID = Request.QueryString["ImageID"];
            string strAvatarID = Request.QueryString["avatarID"];
            long id = -1;
            if (!string.IsNullOrEmpty(strImgID))
            {             
                if (long.TryParse(strImgID, out id))
                {

                    bool thumb = false;
                    bool medThumb = false;
                    string strThumb = Request.QueryString["thumb"];
                    string strMedThumb = Request.QueryString["medThumb"];

                    if (!string.IsNullOrEmpty(strThumb))
                    {
                        if (!bool.TryParse(strThumb, out thumb))
                        {
                            Response.Redirect("Home.aspx");
                        }
                    }

                    if (!string.IsNullOrEmpty(strMedThumb))
                    {
                        if (!bool.TryParse(strMedThumb, out medThumb))
                        {
                            Response.Redirect("Home.aspx");
                        }
                    }

                    CodeImages codeImages = new CodeImages();

                    Image image = codeImages.Get(objectContext, id);

                    if (image != null)
                    {
                        Byte[] bytes = null;
                        if (thumb && !medThumb)
                        {
                            bytes = image.thumbnail;
                        }
                        else if (thumb && medThumb)
                        {
                            bytes = codeImages.GetThumbnailBytes(objectContext, image, 980);
                        }
                        else if (!thumb && !medThumb)
                        {
                            bytes = image.image;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        SendResponse(bytes);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(strAvatarID))
            {
                if (long.TryParse(strAvatarID, out id))
                {

                    CodeUsers codeUsers = new CodeUsers();

                    User user = codeUsers.GetUser(objectContext, id, false);

                    if (user != null && user.avatar != null)
                    {
                        Byte[] bytes = null;

                        bytes = user.avatar;

                        SendResponse(bytes);
                    }

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


        private void SendResponse(Byte[] bytes)
        {

            Response.Clear();

            Response.BufferOutput = true;

            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "image/JPEG";

            //Response.Clear();

            //string attachmentStr = string.Format("attachment;filename=img.jpg");
            //Response.AddHeader("content-disposition", attachmentStr);


            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }





    }
}
