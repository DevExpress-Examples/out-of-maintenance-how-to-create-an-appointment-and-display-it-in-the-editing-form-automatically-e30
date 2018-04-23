Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler

Partial Public Class [Default]
	Inherits System.Web.UI.Page
	Private ReadOnly Property Storage() As ASPxSchedulerStorage
		Get
			Return ASPxScheduler1.Storage
		End Get
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		SetupMappings()
		ResourceFiller.FillResources(Storage, 3)

		ASPxScheduler1.AppointmentDataSource = appointmentDataSource
		ASPxScheduler1.DataBind()

		If (Not IsPostBack) Then
			ASPxScheduler1.Services.Selection.SelectedInterval = New TimeInterval(ASPxScheduler1.Start.Date.AddHours(1), ASPxScheduler1.Start.Date.AddHours(2))
		End If
	End Sub

	#Region "Data-Related Events"

	Private Sub SetupMappings()
		Dim mappings As ASPxAppointmentMappingInfo = Storage.Appointments.Mappings
		Storage.BeginUpdate()
		Try
			mappings.AppointmentId = "Id"
			mappings.Start = "StartTime"
			mappings.End = "EndTime"
			mappings.Subject = "Subject"
			mappings.AllDay = "AllDay"
			mappings.Description = "Description"
			mappings.Label = "Label"
			mappings.Location = "Location"
			mappings.RecurrenceInfo = "RecurrenceInfo"
			mappings.ReminderInfo = "ReminderInfo"
			mappings.ResourceId = "OwnerId"
			mappings.Status = "Status"
			mappings.Type = "EventType"
		Finally
			Storage.EndUpdate()
		End Try
	End Sub

	'Initilazing ObjectDataSource
	Protected Sub appointmentsDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
		e.ObjectInstance = New CustomEventDataSource(GetCustomEvents())
	End Sub

	Private Function GetCustomEvents() As CustomEventList
		Dim events As CustomEventList = TryCast(Session("ListBoundModeObjects"), CustomEventList)
		If events Is Nothing Then
			events = New CustomEventList()
			Session("ListBoundModeObjects") = events
		End If
		Return events
	End Function

	' User generated appointment id    
	Protected Sub ASPxScheduler1_AppointmentInserting(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
		SetAppointmentId(sender, e)
	End Sub

	Private Sub SetAppointmentId(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
		Dim storage As ASPxSchedulerStorage = CType(sender, ASPxSchedulerStorage)
		Dim apt As Appointment = CType(e.Object, Appointment)
		storage.SetAppointmentId(apt, apt.GetHashCode())
	End Sub

	#End Region ' Data-Related Events

	Protected Sub ASPxScheduler1_BeforeExecuteCallbackCommand(ByVal sender As Object, ByVal e As SchedulerCallbackCommandEventArgs)
		If e.CommandId = "CRTAPT" Then
			e.Command = New CreateAppointmentCallbackCommand(CType(sender, ASPxScheduler))
		End If
	End Sub

End Class

Public Class CreateAppointmentCallbackCommand
	Inherits SchedulerCallbackCommand
	Public Overrides ReadOnly Property Id() As String
		Get
			Return "CRTAPT"
		End Get
	End Property

	Private subject_Renamed As String
	Protected Property Subject() As String
		Get
			Return subject_Renamed
		End Get
		Set(ByVal value As String)
			subject_Renamed = value
		End Set
	End Property

	Public Sub New(ByVal control As ASPxScheduler)
		MyBase.New(control)

	End Sub

	Protected Overrides Sub ParseParameters(ByVal parameters As String)
		'base.ParseParameters(parameters);

		Subject = parameters
	End Sub

	Protected Overrides Sub ExecuteCore()
		'base.ExecuteCore();

		CreateAppointment()
	End Sub

	Protected Sub CreateAppointment()
		Dim apt As Appointment = Me.Control.Storage.CreateAppointment(AppointmentType.Normal)

		apt.Subject = Subject
		apt.Start = Me.Control.SelectedInterval.Start
		apt.End = Me.Control.SelectedInterval.End
		apt.ResourceId = Me.Control.SelectedResource.Id

		Me.Control.Storage.Appointments.Add(apt)

		Me.Control.ActiveView.SelectAppointment(apt)

		Dim showAppointmentFormByServerIdCallbackCommand As New ShowAppointmentFormByServerIdCallbackCommand(Me.Control)

		showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString())
	End Sub

End Class
