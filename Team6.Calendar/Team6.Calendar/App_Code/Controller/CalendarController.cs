using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;

namespace TutorialCS.Controllers
{
    public class CalendarController : Controller
    {
        //
        // GET: /Backend/

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }


        class Dpc : DayPilotCalendar
        {
            protected override void OnInit(InitArgs e)
            {
                Update();
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                new EventManager().EventCreate(e.Start, e.End, "New event");
                Update();
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = new EventManager().FilteredData(VisibleStart, VisibleEnd).AsEnumerable();

                DataIdField = "id";
                DataTextField = "name";
                DataStartField = "eventstart";
                DataEndField = "eventend";
            }
        }

    }
}
