// Add two states, Pending Approve and Reject, to the shipment workflow similar to other workflows
// with built-in approvals
// In the Pending Approve state, a user can review the invoice. If the user approves the shipment,
// the system will move it to the Open state, otherwise the shipment will be moved to the Rejected state.
// If the user then rejects the shipment, another user can fix the issues in the shipment
// and move it the hold state and then back to the Pending Approval state.
using System;
using System.Collections;
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using PX.Data.BQL;
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ConvertToUsingDeclaration
// ReSharper disable PossibleInvalidOperationException
// ReSharper disable once CheckNamespace
namespace PX.Objects.SO
{
	// Add all necessary usings including one static BoundedTo using,
	// that will reduce amount of code we will write below.    
	using State = SOShipmentStatus;
	using static BoundedTo<SOShipmentEntry, SOShipment>;

	// Create the helper class which will hold the new Shipment states
	public class SOShipmentApprovalStatus
	{
		public const string PendingApproval = "P";
		public const string Rejected = "J";

		public class pendingApproval : PX.Data.BQL.BqlString.Constant<pendingApproval>
		{
			public pendingApproval() : base(PendingApproval)
			{
			}
		}

		public class rejected : PX.Data.BQL.BqlString.Constant<rejected>
		{
			public rejected() : base(Rejected)
			{
			}
		}
	}

	/// <summary>    
	/// The DAC extension with the new fields
	/// </summary>
	public class SOShipment_ApproveExtension : PXCacheExtension<SOShipment>
	{
		#region Approved

		/// <summary>        
		/// The additional DAC field that indicates whether the shipment has been approved
		/// </summary>
		public abstract class approved : BqlBool.Field<approved>
		{
		}

		[PXDBBool]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Approved")]
		public virtual Boolean? Approved { get; set; }

		#endregion

		#region Rejected

		/// <summary>
		/// The additional DAC field that indicates whether the shipment has been rejected
		/// </summary>
		public abstract class rejected : BqlBool.Field<rejected>
		{
		}

		[PXDBBool]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Rejected")]
		public virtual Boolean? Rejected { get; set; }

		#endregion
	}

	// Graph Extension, that will implement logic of the customized workflow.
	// As it will change the existing workflow, we will reference     
	// the SOShipmentEntry_Workflow extension in the declaration too    
	public class SOShipmentEntry_ApprovalWorkflow : PXGraphExtension<SOShipmentEntry_Workflow, SOShipmentEntry>
	{
		// New action, that will be used to mark the current shipment as approved.
		// The body of this action left empty, as all
		// the logic of this action defined in the workflow code.
		public PXAction<SOShipment> approve;

		[PXButton(CommitChanges = true),
		 PXUIField(DisplayName = "Approve")]
		protected virtual IEnumerable Approve(PXAdapter adapter) => adapter.Get();

		// An action, that will be used to mark the current shipment as rejected.
		public PXAction<SOShipment> reject;
		
		[PXButton(CommitChanges = true),
		 PXUIField(DisplayName = "Reject")]
		protected virtual IEnumerable Reject(PXAdapter adapter) => adapter.Get();

		// Shipments approval process is available only in case, when order approval
		// is enabled too. For this purpose a new DB-slot created
		// Because the Configure method is executed in a very specific graph context,
		// it is strongly recommended not to use Base reference of the graph directly,
		// graph caches or views. So, we can not get value of SOSetup directly from the Graph’s
		// PXSetup property – it has not been initialized yet.
		// To overcome this problem, we can create small database slot, that will also depend on the SOSetup table,
		// read and hold the OrderRequestApproval value from DB.
		private class SOShipmentApproval : IPrefetchable
		{
			// DB-slot, that will hold only the IsActive property.
			// This property will read the SOSetup.orderRequestApproval value
			public static bool IsActive => PXDatabase
				.GetSlot<SOShipmentApproval>(nameof(SOShipmentApproval), typeof(SOSetup))
				.OrderRequestApproval;

			private bool OrderRequestApproval;

			void IPrefetchable.Prefetch()
			{
				using (PXDataRecord soSetup =
				       PXDatabase.SelectSingle<SOSetup>(new PXDataField<SOSetup.orderRequestApproval>()))
				{
					if (soSetup != null)
						OrderRequestApproval = (bool)soSetup.GetBoolean(0);
				}
			}
		}

		public static bool ApprovalIsActive() => PXAccess.FeatureInstalled<CS.FeaturesSet.approvalWorkflow>() &&
		                                         SOShipmentApproval.IsActive;

		// Conditions class, that holds two Conditions, IsApproved and IsRejected
		public class Conditions : Condition.Pack
		{
			public Condition IsApproved =>
				GetOrCreate(b => b.FromBql<SOShipment_ApproveExtension.approved.IsEqual<True>>());

			public Condition IsRejected =>
				GetOrCreate(b => b.FromBql<SOShipment_ApproveExtension.rejected.IsEqual<True>>());
		}

		// If we want to use external dependencies in our configuration code to make it more flexible,
		// such as PXSetup or Database Slot - we need to attach special PXWorkflowDependsOnType attribute
		// to the Configure method and enumerate all types or tables, used in the configuration.
		// In runtime, it will signal the workflow engine
		// that it should re-assemble the configuration in case
		// there are any changes in provided tables occur.
		[PXWorkflowDependsOnType(typeof(SOSetup))]
		public override void Configure(PXScreenConfiguration config)
		{
			// if shipment approvals are activated - let's configure them
			if (ApprovalIsActive())
				Configure(config.GetScreenConfigurationContext<SOShipmentEntry, SOShipment>());
			else
				// otherwise, let's just hide new actions
				HideApproveAndRejectActions(config.GetScreenConfigurationContext<SOShipmentEntry, SOShipment>());
		}

		private void Configure(WorkflowContext<SOShipmentEntry, SOShipment> context)
		{
			const string initialState = "_";
			// First, let's take all necessary objects from the screen configuration context.
			// First, we define the 'Approval' category, where we will put our new actions
			// Also we create an instance of the class that will hold all conditions.
			var conditions = context.Conditions.GetPack<Conditions>();
			var approvalCategory = CommonActionCategories.Get(context).Approval;
			
			// Next, we define the logic of our new Aprove action
			// add it to the Approve category, specify that the action should update
			// the approved field with the TRUE value,
			// make the action visible and enabled
			var approveAction = context.ActionDefinitions
				.CreateExisting<SOShipmentEntry_ApprovalWorkflow>(g => g.approve, a => a
					.WithCategory(approvalCategory)
					.PlaceInCategory(Placement.First)
					.DisplayName("Approve")
					.MapEnableToSelect()
					.MapVisibleToSelect()
					.WithFieldAssignments(fa => fa.Add<SOShipment_ApproveExtension.approved>(true)));
			
			// Then, let's specify the same condifuration for the Reject action
			var rejectAction = context.ActionDefinitions.CreateExisting<SOShipmentEntry_ApprovalWorkflow>(g => g.reject,
				a => a.WithCategory(approvalCategory)
					.PlaceInCategory(Placement.First)
					.DisplayName("Reject")
					.MapEnableToSelect()
					.MapVisibleToSelect()
					.PlaceAfter(approveAction)
					.WithFieldAssignments(fa => fa.Add<SOShipment_ApproveExtension.rejected>(true)));
			
			// Change the screen configuration of the SOShipment screen. First we update the default workflow
			context.UpdateScreenConfigurationFor(screen =>
			{
				return screen
					.UpdateDefaultFlow(flow => flow
						.WithFlowStates(states =>
						{
							// Add new states. First one is pendingApproval
							states.Add<SOShipmentApprovalStatus.pendingApproval>(flowState =>
							{
								return flowState
									// and add some specific action configurations.
									// We want the putOnHold action to be available here, and our
									// new actions available too
									// And they should be duplicated on the toolbar and marked as primary
									.WithActions(actions =>
									{
										actions.Add(approveAction, a => a.IsDuplicatedInToolbar());
										actions.Add(rejectAction, a => a.IsDuplicatedInToolbar());
										actions.Add(g => g.putOnHold);
									});
							});
							// Add the rejected state and configure it
							states.Add<SOShipmentApprovalStatus.rejected>(flowState =>
							{
								return flowState
									.WithActions(actions =>
									{
										actions.Add(g => g.putOnHold, a => a.IsDuplicatedInToolbar());
									});
							});
						})
						// States are ready now, so let’s go to the transitions section.
						// First, we need to add a transition from the initial and Hold states
						// to our new pendingApproval state
						// when the IsApproved condition is false
						.WithTransitions(transitions =>
						{
							// These transitions are placed before the transition to the Open state
							// otherwise the system will check the transition to the Open state first,
							// and skip the checking for transition to the pendingApproval state.
							
							transitions.UpdateGroupFrom(initialState, ts =>
							{
								ts.Add(t => t // New Pending Approval
									.To<SOShipmentApprovalStatus.pendingApproval>()
									.IsTriggeredOn(g => g.initializeState)
									.When(!conditions.IsApproved)
									.PlaceAfter(tr => tr.To<State.hold>()));
							});
							transitions.UpdateGroupFrom<State.hold>(ts =>
							{
								ts.Add(t => t
									.To<SOShipmentApprovalStatus.pendingApproval>()
									.IsTriggeredOn(g => g.releaseFromHold)
									.When(!conditions.IsApproved)
									.PlaceBefore(tr => tr.To<State.open>())
									.WithFieldAssignments(fas => fas.Add<SOShipment.hold>(false)));
							});
							// Add two transitions from the pendingApproval state, depending on the pressed button
							transitions.AddGroupFrom<SOShipmentApprovalStatus.pendingApproval>(ts =>
							{
								ts.Add(t => t
									.To<State.open>()
									.IsTriggeredOn(approveAction)
									.When(conditions.IsApproved));
								ts.Add(t => t
									.To<SOShipmentApprovalStatus.rejected>()
									.IsTriggeredOn(rejectAction)
									.When(conditions.IsRejected));
							});
							// and one more transition from the rejected state to the On Hold state
							transitions.AddGroupFrom<SOShipmentApprovalStatus.rejected>(ts =>
							{
								ts.Add(t => t
									.To<State.hold>()
									.IsTriggeredOn(g => g.putOnHold)
									.DoesNotPersist()
									.WithFieldAssignments(fas => fas.Add<SOShipment.hold>(true))
								);
							});
						}))
					// Register our new actions in the screen configuration
					.WithActions(actions =>
					{
						actions.Add(approveAction);
						actions.Add(rejectAction);
						actions.Update(
							g => g.putOnHold,
							a => a.WithFieldAssignments(fas =>
							{
								fas.Add<SOShipment_ApproveExtension.approved>(f => f.SetFromValue(false));
								fas.Add<SOShipment_ApproveExtension.rejected>(f => f.SetFromValue(false));
							}));
					})
					// Add our two new states to the list of available combo-box values of the status property.
					.WithFieldStates(fields => fields.Add<SOShipment.status>(field =>
						field.SetComboValue(SOShipmentApprovalStatus.PendingApproval, "Pending Approval")
							.SetComboValue(SOShipmentApprovalStatus.Rejected, "Rejected")));
			});
		}

		// Method to modify the screen configuration in the way that
		// new actions are always hidden
		protected virtual void HideApproveAndRejectActions(WorkflowContext<SOShipmentEntry, SOShipment> context)
		{
			var approveHidden = context.ActionDefinitions
				.CreateExisting<SOShipmentEntry_ApprovalWorkflow>(g => g.approve, a => a
					.WithCategory(PredefinedCategory.Actions)
					.IsHiddenAlways());
			var rejectHidden = context.ActionDefinitions
				.CreateExisting<SOShipmentEntry_ApprovalWorkflow>(g => g.reject, a => a
					.WithCategory(PredefinedCategory.Actions)
					.IsHiddenAlways());
			context.UpdateScreenConfigurationFor(screen =>
			{
				return screen
					.WithActions(actions =>
					{
						actions.Add(approveHidden);
						actions.Add(rejectHidden);
					});
			});
		}
	}
}