using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Data;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Editor.TinyMCE.Plugins;
using EPiServer.Web.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BVNetwork.Attend.Business.Pdf
{
    public class PdfGenerator
    {

        protected static Regex databindPattern = new Regex(@"[[]?([a-zA-Z0-9~_+-]+)[.]([a-z0-9.~ _+-]+)[]]?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static void Create(Stream template, IParticipant registration, EventPageBase pageData, System.IO.Stream outputStream)
        {
            try
            {
                PdfReader pr = new PdfReader(template);
                PdfStamper ps = new PdfStamper(pr, outputStream);

                AcroFields fields = ps.AcroFields;

                foreach (string fieldName in fields.Fields.Keys)
                {
                    string objectName;
                    string propertyName;

                    Match m = databindPattern.Match(fieldName);

                    if (!m.Success)
                    {
                        objectName = "CurrentRegistration";
                        propertyName = fieldName;
                    }
                    else
                    {
                        objectName = m.Groups[1].Value;
                        propertyName = m.Groups[2].Value;
                    }

                    

                    if (0 == String.Compare("CurrentRegistration", objectName, true))
                    {
                        if(!string.IsNullOrEmpty(pageData.EventDetails[propertyName] as string))
                            fields.SetField(fieldName, pageData.EventDetails[propertyName] as string);
                        if (!string.IsNullOrEmpty(pageData[propertyName] as string))
                            fields.SetField(fieldName, pageData[propertyName] as string);
                        if (!string.IsNullOrEmpty(AttendRegistrationEngine.GetParticipantInfo(registration, propertyName) as string))
                            fields.SetField(fieldName, AttendRegistrationEngine.GetParticipantInfo(registration, propertyName) as string);
                    }
                    else if (0 == String.Compare(objectName, "CurrentPage", true))
                    {
                        
                        if (0 == String.Compare(propertyName, "PageLinkUrl", true))
                        {
                            string linkUrl = pageData.LinkURL;
                            UrlBuilder ub = new UrlBuilder(linkUrl);

                            Global.UrlRewriteProvider.ConvertToExternal(ub, pageData.PageLink, System.Text.Encoding.UTF8);
                            fields.SetField(fieldName, ub.ToString());
                        }
                        else
                        {
                            if (pageData.EventDetails.Property[propertyName] != null && !string.IsNullOrEmpty(pageData.EventDetails.Property[propertyName].ToString()))
                                fields.SetField(fieldName, pageData.EventDetails[propertyName] as string);
                            else
                                fields.SetField(fieldName, pageData.Property[propertyName].ToString());
                        }
                    }
                    else
                    {
                        // unrecognized ObjectName, simply set ""
                        fields.SetField(fieldName, "");
                    }
                }


                ps.FormFlattening = true;
                ps.Close();

            }
            catch (Exception)
            {
            }
        }

    }
}