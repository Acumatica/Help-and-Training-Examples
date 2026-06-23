using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class LastModifiedDateTimeScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "last-modified-date-times"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			context.Step(
				"PUT and update contact",
				() =>
				{
					var contact = client.Put(new Contact
					{
						LastName = new StringValue { Value = "Diagnostic Contact" }
					});

					var original = contact.LastModifiedDateTime == null ? null : contact.LastModifiedDateTime.Value;
					contact.FirstName = new StringValue { Value = "Updated" };
					var updated = client.Put(contact);

					return new
					{
						OriginalLastModifiedDateTime = original,
						UpdatedContact = updated
					};
				});

			context.Step(
				"PUT and update business account",
				() =>
				{
					var businessAccountId = IdGenerator.BusinessAccountId();
					var account = client.Put(new BusinessAccount
					{
						BusinessAccountID = new StringValue { Value = businessAccountId },
						Name = new StringValue { Value = "Diagnostic business account " + businessAccountId }
					});

					account = client.GetByKeys<BusinessAccount>(account.BusinessAccountID.Value, expand: "MainContact");
					var original = account.LastModifiedDateTime == null ? null : account.LastModifiedDateTime.Value;

					if (account.MainContact != null)
					{
						account.MainContact.CompanyName = new StringValue { Value = "Updated diagnostic company" };
					}

					var updated = client.Put(account);

					return new
					{
						OriginalLastModifiedDateTime = original,
						UpdatedBusinessAccount = updated
					};
				});

			context.Step(
				"PUT and update purchase order",
				() =>
				{
					var purchaseOrder = client.Put(new PurchaseOrder
					{
						VendorID = new StringValue { Value = "ALLFRUITS" }
					});

					purchaseOrder = client.GetById(purchaseOrder);
					var original = purchaseOrder.LastModifiedDateTime == null ? null : purchaseOrder.LastModifiedDateTime.Value;
					purchaseOrder.Description = new StringValue { Value = "Diagnostic purchase order update" };

					var updated = client.Put(purchaseOrder);

					return new
					{
						OriginalLastModifiedDateTime = original,
						UpdatedPurchaseOrder = updated
					};
				});
		}
	}
}
