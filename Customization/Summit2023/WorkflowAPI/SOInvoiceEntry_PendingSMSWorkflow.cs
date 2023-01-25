// In this scenario we will add the new PendingSMS state after the PendingEmail state 
using System;
using System.Collections;
using System.Threading;
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Objects.Common;

namespace PX.Objects.SO
{
	// Static usings will reduce amount of code we will write below.
	using State = ARDocStatus;
	using static BoundedTo<SOInvoiceEntry, ARInvoice>;

	
	/// <summary>
	/// The DAC extension with the new fields
	/// </summary>
	public class SOInvoice_SMSExtension : PXCacheExtension<ARInvoice>
	{
		#region SendSMS

		/// <summary>
		/// The additional DAC field that indicates whether it is required to send SMS
		/// </summary>
		public abstract class sendSMS : PX.Data.BQL.BqlBool.Field<sendSMS>
		{
		}

		[PXDBBool]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Send SMS")]
		public virtual Boolean? SendSMS { get; set; }

		#endregion

		#region SMSSent

		/// <summary>
		/// The additional DAC field that indicates whether the SMS was already sent
		/// </summary>
		public abstract class sMSSent : PX.Data.BQL.BqlBool.Field<sMSSent>
		{
		}

		[PXDBBool]
		[PXDefault(false)]
		[PXUIField(DisplayName = "SMS Sent")]
		public virtual Boolean? SMSSent { get; set; }

		#endregion
	}

	// Class, that holds new Status constants
	public class SOInvoiceSMSStatus
	{
		public const string PendingSMS = "A";

		public class pendingSMS : PX.Data.BQL.BqlString.Constant<pendingSMS>
		{
			public pendingSMS() : base(PendingSMS)
			{
			}
		}
	}

	// GraphExtension, that implements workflow customization logic.
	// As it will change the existing workflow, we add reference to the
	// SOInvoiceEntry_Workflow extension too
	public class SOInvoiceEntry_PendingSMSWorkflow : PXGraphExtension<SOInvoiceEntry_Workflow, SOInvoiceEntry>
	{
		// The new action, that will be used to send SMS message.
		public PXAction<ARInvoice> sendSMS;

		[PXButton(CommitChanges = true),
		 PXUIField(DisplayName = "Send SMS")]
		protected virtual IEnumerable SendSMS(PXAdapter adapter)
		{
			PXLongOperation.StartOperation(this, () =>
			{
				// Emulate SMS long run operation
				Thread.Sleep(8000);
			});
			return adapter.Get();
		}

		// Class for a database slot which emulates the new SMS setting. For testing purposes,
		// it is mapped on the SOSetup.orderRequestApproval property.
		private class SOInvoiceSMSExtension : IPrefetchable
		{
			public static bool IsActive => PXDatabase
				.GetSlot<SOInvoiceSMSExtension>(nameof(SOInvoiceSMSExtension), typeof(SOSetup))
				.InvoiceSendSMS;

			private bool InvoiceSendSMS;

			void IPrefetchable.Prefetch()
			{
				using (PXDataRecord soSetup =
				       PXDatabase.SelectSingle<SOSetup>(new PXDataField<SOSetup.orderRequestApproval>()))
				{
					if (soSetup != null)
						InvoiceSendSMS = (bool)soSetup.GetBoolean(0);
				}
			}
		}

		public static bool SendSMSIsActive() => SOInvoiceSMSExtension.IsActive;

		// Conditions class, holds the new Condition, that checks whether we need
		// to send SMS or whether the SMS message has already been sent.
		// This condition is used to determing whether we should skip our new state
		public class Conditions : Condition.Pack
		{
			public Condition IsSMSed => GetOrCreate(b =>
				SOInvoiceSMSExtension.IsActive
					? b.FromBql<SOInvoice_SMSExtension.sendSMS.IsEqual<False>.Or<
						SOInvoice_SMSExtension.sMSSent.IsEqual<True>>>()
					: b.FromBql<True.IsEqual<True>>());
		}

		// If we want to use external dependencies in our configuration code to make it more flexible,
		// such as PXSetup or Database Slot - we need to attach the special PXWorkflowDependsOnType attribute
		// to the Configure method and enumerate all types or tables, used in the configuration.
		// At runtime, the workflow engine will monitor these tables and rebuild the screen
		// configuration if any change in provided tables occur.
		[PXWorkflowDependsOnType(typeof(SOSetup))]
		public override void Configure(PXScreenConfiguration config)
		{
			if (SendSMSIsActive())
			{
				Configure(config.GetScreenConfigurationContext<SOInvoiceEntry, ARInvoice>());
			}
		}

		private void Configure(WorkflowContext<SOInvoiceEntry, ARInvoice> context)
		{
			// First, take all necessary objects from the screen configuration context.
			// We need conditions holder and the 'PrintingAndEmailing' category, where we put our new action
			var conditions = context.Conditions.GetPack<Conditions>();
			var commonCategories = CommonActionCategories.Get(context);

			var printingEmailingCategory = commonCategories.PrintingAndEmailing;

			// Next, define logic of our new SendSMS action
			// add the action to the PrintingAndEmailing category and let it update SMSSent field with the TRUE value,
			// make it visible and enabled
			var sendSMSAction = context.ActionDefinitions
				.CreateExisting<SOInvoiceEntry_PendingSMSWorkflow>(g => g.sendSMS, a => a
					.WithCategory(printingEmailingCategory)
					.PlaceInCategory(Placement.Last)
					.DisplayName("Send SMS")
					.MapEnableToSelect()
					.MapVisibleToSelect()
					.WithFieldAssignments(fa => fa.Add<SOInvoice_SMSExtension.sMSSent>(true)));

			// Change the screen configuration of ARInvoice. First, update the default workflow
			context.UpdateScreenConfigurationFor(screen =>
			{
				return screen
					.UpdateDefaultFlow(flow => flow
						.WithFlowStates(states =>
						{
							// Update the definition of HoldToBalance sequence. 
							states.UpdateSequence<State.HoldToBalance>(seq =>
								seq.WithStates(sss =>
								{
									// Add the pendingSMS state to the sequence and configure it
									sss.Add<SOInvoiceSMSStatus.pendingSMS>(flowState =>
									{
										// Apply the skip condition to this state as we don't want to send SMS for the Invoice if it has already been sent
										return flowState
											.IsSkippedWhen(conditions.IsSMSed)
											// Add some specific action configurations.
											// for the PutOnHold action
											// and the SendSMSAction action which should be available to the user. The SendSMS action should be duplicated on
											// the form toolbar 
											.WithActions(actions =>
											{
												actions.Add(sendSMSAction, a => a.IsDuplicatedInToolbar());
												actions.Add(g => g.putOnHold);
											})
											// Place the new state just after the pendingPrint state.
											// Now we have the following order of states
											// ON Hold -> Pending Processing -> CreditHold ->  pendingPrint -> pendingSMS  -> pendingEmail -> Balanced
											.PlaceAfter<State.pendingPrint>();
									});
								}));
	
						})
						// Add a transition from the new PendingSMS state to the next state in the sequence
						// the transition is triggered when the user clicks the SendSMSAction action.
						// The transition does not target any specific state to make it easier to customize later.
						// The system will determine the next state of the transition at runtime,
						// when the whole workflow definition is assembled.
						.WithTransitions(transitions =>
						{
							transitions.AddGroupFrom<SOInvoiceSMSStatus.pendingSMS>(ts =>
							{
								ts.Add(t => t
									.ToNext()
									.IsTriggeredOn(sendSMSAction)
									.When(conditions.IsSMSed));
							});
						}))
					// Register new action in the screen configuration
					.WithActions(actions => { actions.Add(sendSMSAction); })
					// Add the new status value to the list of combo box values for the Status field
					// Speciy the default value for the SOInvoice_SMSExtension.sendSMS field based on the system settings
					.WithFieldStates(fields =>
					{
						fields.Add<ARInvoice.status>(field =>
							field.SetComboValue(SOInvoiceSMSStatus.PendingSMS, "Pending SMS"));
						fields.Add<SOInvoice_SMSExtension.sendSMS>(field =>
							field.WithDefaultValue(SendSMSIsActive()));
					});
			});
		}
	}
}