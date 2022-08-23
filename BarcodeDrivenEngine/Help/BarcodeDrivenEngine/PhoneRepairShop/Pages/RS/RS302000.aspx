<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS302000.aspx.cs" Inherits="Page_IN305020" Title="Scan and Count" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
 
<asp:content id="cont1" contentplaceholderid="phDS" runat="Server">
    <script type="text/javascript">
        function Barcode_Initialize(ctrl) { // This script makes focus to be kept on the Barcode field's control.
            ctrl.element.addEventListener('keydown', function (e) {
                if (e.keyCode === 13) { // Enter key
                    e.preventDefault();
                    e.stopPropagation();
                }
            });
        };
    </script>
 
    <script type="text/javascript">
        function ActionCallback(callbackContext) { // This script plays the sound that corresponds to the message type of the barcode-driven engine.
            var baseUrl = (location.href.indexOf("HideScript") > 0) ? "../../Sounds/" : "../../../Sounds/";
            var edInfoMessageSoundFile = px_alls["edInfoMessageSoundFile"];

            if ((callbackContext.info.name.toLowerCase().startsWith("scan") || callbackContext.info.name == "ElapsedTime") && callbackContext.control.longRunInProcess == null && edInfoMessageSoundFile != null) {
                var soundFile = edInfoMessageSoundFile.getValue();
                if (soundFile != null && soundFile != "") {
                    var audio = new Audio(baseUrl + soundFile + '.wav');
                    audio.play();
                }
            }
        };
        window.addEventListener('load', function () { px_callback.addHandler(ActionCallback); });
    </script>
  
    <%-- Note that the datasource binds to the Host graph of the barcode scan class --%>
    <px:PXDataSource ID="ds" runat="server" TypeName="PhoneRepairShop.INScanCount+Host" PrimaryView="HeaderView">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:content>
 
<asp:content id="cont2" contentplaceholderid="phF" runat="Server">
    <px:PXFormView ID="formHeader" runat="server" DataSourceID="ds" Height="120px" Width="100%" DataMember="HeaderView" DefaultControlID="edBarcode">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="L" />
            <px:PXTextEdit ID="edBarcode" runat="server" DataField="Barcode">
                <AutoCallBack Command="Scan" Target="ds">
                    <Behavior CommitChanges="True" />
                </AutoCallBack>
                <ClientEvents Initialize="Barcode_Initialize"/>
            </px:PXTextEdit>
            <px:PXSelector ID="edRefNbr" runat="server" DataField="RefNbr" AllowEdit="true" />
            <px:PXSelector ID="edSiteID" runat="server" DataField="SiteID" AllowEdit="true" />
 
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="L" ColumnWidth="M" />
            <px:PXTextEdit ID="edMessage" runat="server" DataField="Message"
                Height="55px" Width="800px" Style="font-size: 10pt; font-weight: bold;"
                SuppressLabel="true" TextMode="MultiLine" SkinID="Label" DisableSpellcheck="True" Enabled="False" />
        </Template>
    </px:PXFormView>
    <px:PXFormView ID="formInfo" runat="server" DataSourceID="ds" DataMember="Info">
        <Template>
            <px:PXTextEdit ID="edInfoMode" runat="server" DataField="Mode"/>
            <px:PXTextEdit ID="edInfoMessage" runat="server" DataField="Message"/>
            <px:PXTextEdit ID="edInfoMessageSoundFile" runat="server" DataField="MessageSoundFile"/>
            <px:PXTextEdit ID="edInfoPrompt" runat="server" DataField="Prompt"/>
        </Template>
    </px:PXFormView>
</asp:content>
 
<asp:content id="cont3" contentplaceholderid="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="540px" Style="z-index: 100;" Width="100%">
    <Items>
        <px:PXTabItem Text="Count">
            <Template>
                <px:PXGrid ID="gridPIDetail" runat="server" DataSourceID="ds" SyncPosition="true" Width="100%" SkinID="DetailsInTab">
                    <Levels>
                        <px:PXGridLevel DataMember="PIDetail">
                            <Columns>
                                <px:PXGridColumn DataField="LineNbr" />
                                <px:PXGridColumn DataField="Status" />
                                <px:PXGridColumn DataField="LocationID" />
                                <px:PXGridColumn DataField="InventoryID" />
                                <px:PXGridColumn DataField="InventoryID_InventoryItem_descr"/>
                                <px:PXGridColumn DataField="LotSerialNbr" />
                                <px:PXGridColumn DataField="BookQty" TextAlign="Right" />
                                <px:PXGridColumn DataField="PhysicalQty" TextAlign="Right" />
                                <px:PXGridColumn DataField="VarQty" TextAlign="Right" />
                            </Columns>
                            <RowTemplate>
                                <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                                <px:PXSegmentMask ID="edLocationID" runat="server" DataField="LocationID" AutoRefresh="True" />
                                <px:PXNumberEdit ID="edBookQty" runat="server" DataField="BookQty" />
                                <px:PXNumberEdit ID="edPhysicalQty" runat="server" DataField="PhysicalQty" />
                                <px:PXNumberEdit ID="edVarQty" runat="server" DataField="VarQty" />
                            </RowTemplate>
                        </px:PXGridLevel>
                    </Levels>
                    <AutoSize Container="Window" Enabled="True" MinHeight="400" />
                </px:PXGrid>
            </Template>
        </px:PXTabItem>
        <px:PXTabItem Text="Scan Log">
    <Template>
        <px:PXGrid ID="gridScanLog" runat="server" DataSourceID="ds" Style="height: 250px;" Width="100%" SkinID="Inquire" Height="372px" >
            <Levels>
                <px:PXGridLevel DataMember="Logs">
                    <Columns>
                        <px:PXGridColumn DataField="ScanTime" />
                        <px:PXGridColumn DataField="Mode" />
                        <px:PXGridColumn DataField="Prompt" />
                        <px:PXGridColumn DataField="Scan" />
                        <px:PXGridColumn DataField="Message" />
                    </Columns>
                </px:PXGridLevel>
            </Levels>
            <AutoSize Container="Window" Enabled="True" MinHeight="400" />
        </px:PXGrid>
    </Template>
</px:PXTabItem>
    </Items>
    <AutoSize Enabled="True" Container="Window" />
</px:PXTab>
</asp:content>