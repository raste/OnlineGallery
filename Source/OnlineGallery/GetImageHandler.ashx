<%@ WebHandler Language="C#" Class="OnlineGallery.GetImageHandler" %>

﻿// Online Gallery (https://github.com/raste/OnlineGallery)
// Copyright (c) 2015 Georgi Kolev. 
// Licensed under Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Web;
using Microsoft.Web;

namespace OnlineGallery
{

    public class GetImageHandler : ImageHandler {

        protected OnlineGallery.Entities objectContext = new OnlineGallery.Entities();

        public GetImageHandler()
        {
            // Set caching settings and add image transformations here
            // EnableServerCache = true;
        }
        
        public override ImageInfo GenerateImage(NameValueCollection parameters) 
        {
            // Add image generation logic here and return an instance of ImageInfo
            //throw new NotImplementedException();

            ImageInfo generatedImgInfo = new ImageInfo(System.Net.HttpStatusCode.NotFound);

            string strImgID = parameters["ImageID"];
            string strAvatarID = parameters["avatarID"];
            long id = -1;
            if (!string.IsNullOrEmpty(strImgID))
            {
                if (long.TryParse(strImgID, out id))
                {

                    bool thumb = false;
                    bool medThumb = false;
                    bool homeThumb = false;
                    string strThumb = parameters["thumb"];
                    string strMedThumb = parameters["medThumb"];
                    string strHomeThumb = parameters["homeThumb"];
                    
                    if (!string.IsNullOrEmpty(strThumb))
                    {
                        if (!bool.TryParse(strThumb, out thumb))
                        {
                            //Response.Redirect("Home.aspx");
                        }
                    }

                    if (!string.IsNullOrEmpty(strMedThumb))
                    {
                        if (!bool.TryParse(strMedThumb, out medThumb))
                        {
                            //Response.Redirect("Home.aspx");
                        }
                    }

                    if (!string.IsNullOrEmpty(strHomeThumb))
                    {
                        if (!bool.TryParse(strHomeThumb, out homeThumb))
                        {
                            //Response.Redirect("Home.aspx");
                        }
                    }

                    
                    OnlineGallery.CodeImages codeImages = new OnlineGallery.CodeImages();

                    OnlineGallery.Image image = codeImages.Get(objectContext, id);

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
                        else if (homeThumb)
                        {
                            bytes = codeImages.GetThumbnailBytes(objectContext, image, 400);
                        }
                        else if (!thumb && !medThumb)
                        {
                            bytes = image.image;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        generatedImgInfo = new ImageInfo(bytes);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(strAvatarID))
            {
                if (long.TryParse(strAvatarID, out id))
                {

                    OnlineGallery.CodeUsers codeUsers = new OnlineGallery.CodeUsers();

                    OnlineGallery.User user = codeUsers.GetUser(objectContext, id, false);

                    if (user != null && user.avatar != null)
                    {
                        Byte[] bytes = null;

                        bytes = user.avatar;

                        generatedImgInfo = new ImageInfo(bytes);
                    }

                }
                else
                {
                    //Response.Redirect("Home.aspx");
                }
            }
            else
            {
                //Response.Redirect("Home.aspx");
            }

            return generatedImgInfo;   
        }
    }
}
