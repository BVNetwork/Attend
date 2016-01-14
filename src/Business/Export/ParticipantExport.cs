using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Core;
using ExcelLibrary.SpreadSheet;

namespace BVNetwork.Attend.Business.Export
{
    public class ParticipantExport
    {
        public static void Export(List<IParticipant> participants)
        {
            Export(participants, null);
        }

        public static void ExportXLSX(List<IParticipant> participants, List<string> formFields)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=participants-" + participants[0].EventPage.ID + ".xls");
            HttpContext.Current.Response.Charset = Encoding.UTF8.WebName;
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.BinaryWrite(Encoding.UTF8.GetPreamble());



            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BufferOutput = true;

            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("Participants");
            if(formFields != null)
            for(int i = 0; i < formFields.Count; i++)
                worksheet.Cells[0, i]= new Cell(formFields[i]);
            else
            {
                string[] headers = AttendRegistrationEngine.GetFormData(participants[0]).AllKeys;
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[0, i] = new Cell(headers[i]);
                }
            }

            for (int i = 0; i < participants.Count; i++)
            {
                AddParticipantToWorksheet(worksheet, i+1, participants[i]);
            }

            workbook.Worksheets.Add(worksheet);

            MemoryStream ms = new MemoryStream();
            workbook.SaveToStream(ms);
            ms.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.End();
        }

        public static void AddParticipantToWorksheet(Worksheet ws, int row, IParticipant p)
        {
            string participantData = GetParticipantData(p, null);
            string[] participantValues = participantData.Split(';');
            for (int i = 0; i < participantValues.Length; i++)
            {
                ws.Cells[row, i] = new Cell(participantValues[i]);
            }
        }

        public static void Export(List<IParticipant> participants, List<string> formFields)
        {
            ExportXLSX(participants, formFields);
        }


        public static void ExportCSV(List<ParticipantBlock> participants, List<string> formFields)
        {
            string attachment = "attachment; filename=ParticipantList.csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Attend", "public");
            HttpContext.Current.Response.Charset = Encoding.UTF8.WebName;
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.BinaryWrite(Encoding.UTF8.GetPreamble());


            var sb = new StringBuilder();
            foreach (var participant in participants)
                sb.AppendLine(GetParticipantData(participant as ParticipantBlock, formFields));

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }


        private static string GetParticipantData(IParticipant participant, List<string> formFields)
        {
            string data = participant.AttendStatus.ToString() + "," + participant.Email + "," + participant.Code + ",";
            NameValueCollection allFormFields = AttendRegistrationEngine.GetFormData(participant);
            if (formFields == null)
                foreach (var key in allFormFields.AllKeys)
                    data += allFormFields.Get(key).Replace(System.Environment.NewLine, ", ") + ";";
            else
            {
                foreach (string formField in formFields)
                {
                    if (!string.IsNullOrEmpty(formField))
                        data += AttendRegistrationEngine.GetParticipantInfo(participant, formField).Replace(System.Environment.NewLine, ", ") + ";";
                }
            }
            return data;
        }

    }
}
