//The view for the auto-numbering of records
public PXSetup<RSSVSetup> AutoNumSetup;

//The graph constructor
public RSSVWorkOrderEntry()
{
	RSSVSetup setup = AutoNumSetup.Current;
}