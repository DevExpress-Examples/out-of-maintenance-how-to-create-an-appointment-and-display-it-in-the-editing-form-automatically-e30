using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;

public partial class Default : System.Web.UI.Page {
    ASPxSchedulerStorage Storage { get { return ASPxScheduler1.Storage; } }

    protected void Page_Load(object sender, EventArgs e) {
        SetupMappings();
        ResourceFiller.FillResources(Storage, 3);

        ASPxScheduler1.AppointmentDataSource = appointmentDataSource;
        ASPxScheduler1.DataBind();

        if (!IsPostBack)
            ASPxScheduler1.Services.Selection.SelectedInterval = new TimeInterval(
                ASPxScheduler1.Start.Date.AddHours(1),
                ASPxScheduler1.Start.Date.AddHours(2));
    }

    #region Data-Related Events

    void SetupMappings() {
        ASPxAppointmentMappingInfo mappings = Storage.Appointments.Mappings;
        Storage.BeginUpdate();
        try {
            mappings.AppointmentId = "Id";
            mappings.Start = "StartTime";
            mappings.End = "EndTime";
            mappings.Subject = "Subject";
            mappings.AllDay = "AllDay";
            mappings.Description = "Description";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "OwnerId";
            mappings.Status = "Status";
            mappings.Type = "EventType";
        }
        finally {
            Storage.EndUpdate();
        }
    }

    //Initilazing ObjectDataSource
    protected void appointmentsDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
        e.ObjectInstance = new CustomEventDataSource(GetCustomEvents());
    }

    CustomEventList GetCustomEvents() {
        CustomEventList events = Session["ListBoundModeObjects"] as CustomEventList;
        if (events == null) {
            events = new CustomEventList();
            Session["ListBoundModeObjects"] = events;
        }
        return events;
    }

    // User generated appointment id    
    protected void ASPxScheduler1_AppointmentInserting(object sender, PersistentObjectCancelEventArgs e) {
        SetAppointmentId(sender, e);
    }

    void SetAppointmentId(object sender, PersistentObjectCancelEventArgs e) {
        ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
        Appointment apt = (Appointment)e.Object;
        storage.SetAppointmentId(apt, apt.GetHashCode());
    }

    #endregion Data-Related Events

    protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e) {
        if (e.CommandId == "CRTAPT")
            e.Command = new CreateAppointmentCallbackCommand((ASPxScheduler)sender);
    }

}

public class CreateAppointmentCallbackCommand : SchedulerCallbackCommand {
    public override string Id { get { return "CRTAPT"; } }

    private string subject;
    protected string Subject { get { return subject; } set { subject = value; } }

    public CreateAppointmentCallbackCommand(ASPxScheduler control)
        : base(control) {

    }

    protected override void ParseParameters(string parameters) {
        //base.ParseParameters(parameters);

        Subject = parameters;
    }

    protected override void ExecuteCore() {
        //base.ExecuteCore();

        CreateAppointment();
    }

    protected void CreateAppointment() {
        Appointment apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);

        apt.Subject = Subject;
        apt.Start = this.Control.SelectedInterval.Start;
        apt.End = this.Control.SelectedInterval.End;
        apt.ResourceId = this.Control.SelectedResource.Id;

        this.Control.Storage.Appointments.Add(apt);

        this.Control.ActiveView.SelectAppointment(apt);

        ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

        showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());
    }

}
