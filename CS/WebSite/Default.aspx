<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.3, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v9.3.Core, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v9.3, Version=9.3.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Width="150px" Text="Subject:" />
        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ClientInstanceName="tbSubject" Width="150px" Text="New Appointment" />
        <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Create Appointment">
            <ClientSideEvents Click="function(s, e) {
	            scheduler.RaiseCallback('CRTAPT|' + tbSubject.GetText());
            }" />
        </dx:ASPxButton>
        
        <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" ClientInstanceName="scheduler"
            OnAppointmentInserting="ASPxScheduler1_AppointmentInserting" 
            onbeforeexecutecallbackcommand="ASPxScheduler1_BeforeExecuteCallbackCommand">
        </dxwschs:ASPxScheduler>
    </div>
        <asp:ObjectDataSource ID="appointmentDataSource" runat="server" DataObjectTypeName="CustomEvent"
            TypeName="CustomEventDataSource" DeleteMethod="DeleteMethodHandler" SelectMethod="SelectMethodHandler" InsertMethod="InsertMethodHandler" UpdateMethod="UpdateMethodHandler" OnObjectCreated="appointmentsDataSource_ObjectCreated"></asp:ObjectDataSource>
    </form>
</body>
</html>
