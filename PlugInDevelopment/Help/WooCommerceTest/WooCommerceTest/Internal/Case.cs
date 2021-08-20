using PX.Commerce.Core.API;
using PX.Api.ContractBased.Models;
using System.ComponentModel;

namespace WooCommerceTest
{

    [Description("Case")]
    public partial class Case : CBAPIEntity
    {
        public GuidValue NoteID { get; set; }

        public DateTimeValue LastModifiedDateTime { get; set; }

        public StringValue CaseCD { get; set; }

        public StringValue Subject { get; set; }

        public StringValue Description { get; set; }
    }
}
