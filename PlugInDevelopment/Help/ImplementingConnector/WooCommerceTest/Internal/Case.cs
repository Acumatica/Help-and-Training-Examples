using PX.Commerce.Core.API;
using PX.Api.ContractBased.Models;
using PX.Commerce.Core;

[CommerceDescription("Case")]
public partial class Case : CBAPIEntity
{
	public GuidValue NoteID { get; set; }

	public DateTimeValue LastModifiedDateTime { get; set; }

	[CommerceDescription("CaseCD")]
	public StringValue CaseCD { get; set; }

	[CommerceDescription("Subject")]
	public StringValue Subject { get; set; }

	[CommerceDescription("Description")]
	public StringValue Description { get; set; }
}
