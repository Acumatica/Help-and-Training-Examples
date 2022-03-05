//Display an error if the priority is too low for the selected service
protected virtual void _(Events.RowUpdating<RSSVWorkOrder> e)
{
    // The modified data record (not in the cache yet)
    RSSVWorkOrder row = e.NewRow;
    // The data record that is stored in the cache
    RSSVWorkOrder originalRow = e.Row;

    if (!e.Cache.ObjectsEqual<RSSVWorkOrder.priority, 
           RSSVWorkOrder.serviceID>(row, originalRow))
    {
        if (row.Priority == WorkOrderPriorityConstants.Low)
        {
            //Obtain the service record
            RSSVRepairService service = SelectFrom<RSSVRepairService>.
                Where<RSSVRepairService.serviceID.IsEqual<@P.AsInt>>.
                    View.Select(this, row.ServiceID);

            if (service != null && service.PreliminaryCheck == true)
            {
                //Display the error for the Priority field
                WorkOrders.Cache.RaiseExceptionHandling<RSSVWorkOrder.priority>(row,
                    originalRow.Priority,
                    new PXSetPropertyException(Messages.PriorityTooLow));

                //Assign the proper priority
                e.NewRow.Priority = WorkOrderPriorityConstants.Medium;
            }
        }
    }
}