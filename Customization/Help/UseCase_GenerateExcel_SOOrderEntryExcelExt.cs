using PX.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.SO;
using PX.SM;
using PX.Export;

namespace test_examples
{
    public class SOOrderEntryExcelExt : PXGraphExtension<SOOrderEntry>
    {

        private void ExportFile(PX.Export.Excel.Core.Package excel)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                excel.Write(stream);
                string path = String.Format("SO-{0}-Transaction Info.xlsx", Base.Document.Current.OrderNbr);
                var info = new PX.SM.FileInfo(path, null, stream.ToArray());
                throw new PXRedirectToFileException(info, true);
            }
        }

        // Email generated Excel as an attachment
        private void SendEmail(PX.Export.Excel.Core.Package excel)
        {
            bool sent = false;

            // Select the notification template creates in the previous instruction
            Notification rowNotification = PXSelect<Notification,
                   Where<Notification.name,
                       Equal<Required<Notification.name>>>>
               .Select(Base, "SOExcelNotification");

            if (rowNotification == null)
                throw new PXException("Notification Template for is not specified.");

            // Create an email
            var sender = PX.Objects.EP.TemplateNotificationGenerator.Create(Base.Document.Current,
                rowNotification.NotificationID.Value);
            // You can assign an account to process sending of email, system email account
            //sender.MailAccountId = rowNotification.NFrom.HasValue ?
            //                       rowNotification.NFrom.Value :
            //                       PX.Data.EP.MailAccountManager.DefaultMailAccountID;
            // You can sepcify the email address and change other properties of the template
            //sender.To = "demo@demo.com";
            // By default, all properties are copied from the template

            //Attach Excel
            using (MemoryStream stream = new MemoryStream())
            {
                excel.Write(stream);
                string path = String.Format("SO-{0}-Transaction Info.xlsx",
                       Base.Document.Current.OrderNbr);
                sender.AddAttachment(path, stream.ToArray());
            }

            sender.MassProcessMode = false;
            // Initiate sending of the email
            sent |= sender.Send().Any();

            // The email is put into queue, the scheduler sends emails according to the Send and Receive Email (SM507010) form.
            // The All Emails (CO409070) shows all emails. When the email is in the queue, it has the Pending Processing status. 
        }

        public PXAction<SOOrder> ExportToExcelAndSendEmailAttachment;
        [PXUIField(DisplayName = "Export To Excel", MapViewRights = PXCacheRights.Select,
            MapEnableRights = PXCacheRights.Update)]
        [PXButton]
        protected virtual void exportToExcelAndSendEmailAttachment()
        {
            //Work only with documents that were saved to the database (have Inserted status in Cache)
            if (Base.Document.Current == null ||
                Base.Document.Cache.GetStatus(Base.Document.Current) == PXEntryStatus.Inserted) return;

            var excel = new PX.Export.Excel.Core.Package();
            var sheet = excel.Workbook.Sheets[1];

            // Add header
            sheet.Add(1, 1, "Line #");
            sheet.Add(1, 2, "Transaction Description");
            sheet.Add(1, 3, "Ordered Quantity");

            // Add data
            var index = 2;
            foreach (PXResult<SOLine> lineItem in Base.Transactions.Select())
            {
                SOLine dataRow = (SOLine)lineItem;
                sheet.Add(index, 1, Convert.ToString(dataRow.LineNbr));
                sheet.Add(index, 2, dataRow.TranDesc);
                sheet.Add(index, 3, Convert.ToString(dataRow.OrderQty));
                index++;
            }
            sheet.SetColumnWidth(1, 20);
            sheet.SetColumnWidth(2, 45);
            sheet.SetColumnWidth(3, 35);

            //ExportFile(excel);
            SendEmail(excel);
        }
    }
}
