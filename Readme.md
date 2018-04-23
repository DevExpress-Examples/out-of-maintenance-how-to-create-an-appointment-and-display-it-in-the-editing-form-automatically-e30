# How to create an appointment and display it in the editing form automatically


<p><u>Problem:</u> The <strong>New Appointment </strong>menu command invokes an editing form for an appointment but actually a new appointment is not created until the 'OK' button is clicked. Sometimes it is required to create an appointment first and then edit its properties via the  editing form. <br />
<u>Solution:</u> This example illustrates how to create an appointment and automatically display the appointment form when the end-user click the button on the web form. It appears that the most appropriate solution to this task is to use a custom callback command (see the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument5462"><u>Callback Commands</u></a> help section). This way, you can pass custom values ('Subject' in this example) to the server. <br />
The <strong><u>DevExpress.Web.ASPxScheduler.Internal.ShowAppointmentFormByServerIdCallbackCommand</u></strong><strong> </strong>provides<strong> </strong>the desired functionality.<strong> </strong>Note that since this command is located within an Internal namespace, it is undocumented and we do not guarantee its availability in future versions.</p><p>See also the <a href="https://www.devexpress.com/Support/Center/p/S32567">Add the capability to invoke an Appointment Edit form from a server-side code (e.g. using an existing Appointment ID)</a> suggestion.</p>

<br/>


