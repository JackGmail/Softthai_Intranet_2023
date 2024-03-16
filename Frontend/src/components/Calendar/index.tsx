import React, { useEffect, useRef } from "react";
import { Grid, Box, Tooltip } from "@mui/material";
import AOS from "aos";
import "aos/dist/aos.css";
import FullCalendar from '@fullcalendar/react'
import interactionPlugin from "@fullcalendar/interaction"
import thLocale from '@fullcalendar/core/locales/th';
import enLocale from '@fullcalendar/core/locales/en-gb';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import dayGridPlugin from '@fullcalendar/daygrid'
import multimonth from '@fullcalendar/multimonth'
//CSS
import "./Calendar.css";
import moment from "moment";
import { CalendarEvent, CalendarProps } from "./CalendarClass";

//Icon
import HourglassTopRoundedIcon from '@mui/icons-material/HourglassTopRounded';
import CheckRoundedIcon from '@mui/icons-material/CheckRounded';
import { Convert } from "utilities/utilities";

// export default function Calendar({ Event, OnDateClick, initialView, onCalenderChange }) {
export default function Calendar(props: CalendarProps) {
    const CalendarRef = useRef(null);
    const { lstEvent, OnDateClick, IsInitialView, onCalenderChange, headerToolbar, nDayMaxEvents, sDayHeaderClassNames } = props;

    const renderEventContent = (eventInfo) => {

        const IsMeetingRoom = eventInfo.event.extendedProps.IsMeetingRoom ?? false;

        const nStatus = eventInfo.event.extendedProps.nStatus ?? 0;

        const IsAllDay = eventInfo.event.allDay ?? false;

        const sRoomName = eventInfo.event.extendedProps.sRoomName

        const title = sRoomName ? `${sRoomName}\t :${eventInfo.event.title}` : eventInfo.event.title;

        let timeText = eventInfo.timeText
        if (eventInfo.view.type == "dayGridMonth") {
            let start = moment(eventInfo.event.start).locale("en").format("HH.mm")
            let end = moment(eventInfo.event.end).locale("en").format("HH.mm")
            timeText = start + " - " + end
        }


        return <>
            <div style={{ cursor: "pointer", width: "100%" }} >
                <Tooltip placement="top" title={title}>
                    <div>
                        {IsAllDay ?
                            <p className="calendar-title">
                                {IsMeetingRoom ?
                                    (nStatus == 0) ?
                                        <>
                                            <span className="spanIcon"><HourglassTopRoundedIcon htmlColor="#FCEE9E" className="HourglassTopRoundedIconClass" /></span> <span className="spanTitle">{title}</span>
                                        </>
                                        :
                                        <>

                                            <span className="spanIcon"><CheckRoundedIcon htmlColor="#FFFFFF" className="CheckCircleRoundedIconClass" /></span> <span className="spanTitle">{title}</span>
                                        </>
                                    :
                                    <>
                                        <span className="spanTitle">{title}</span>
                                    </>
                                }
                            </p>
                            :
                            <p className="calendar-title">
                                {IsMeetingRoom ?
                                    (nStatus == 0) ?
                                        <>
                                            <span className="spanIcon"><HourglassTopRoundedIcon htmlColor="#FCEE9E" className="HourglassTopRoundedIconClass" /></span> <span className="spanTitle">{timeText} : {title}</span>
                                        </>
                                        :
                                        <>

                                            <span className="spanIcon"><CheckRoundedIcon htmlColor="#FFFFFF" className="CheckCircleRoundedIconClass" /></span> <span className="spanTitle">{timeText} : {title}</span>
                                        </>
                                    :
                                    <>
                                        <span className="spanTitle">{timeText} : {title}</span>
                                    </>
                                }
                            </p>
                        }
                    </div>
                </Tooltip>

            </div>
        </>
    }

    useEffect(() => {

        let date = props.sDateDefault;
        if (date != null && date != undefined) {
            CalendarRef.current.getApi().gotoDate(date);
            console.log("props.sDateDefault", date);
        }

    }, [props.sDateDefault]);

    return (
        <>
            <FullCalendar
                ref={CalendarRef}
                locale={enLocale}
                plugins={[dayGridPlugin,
                    interactionPlugin,
                    timeGridPlugin,
                    listPlugin,
                    multimonth
                ]}
                eventContent={renderEventContent}
                initialView={IsInitialView == true ? "dayGridMonth" : "timeGridWeek"}
                headerToolbar={headerToolbar}
                // headerToolbar={{
                //     center: "title",
                //     left: "prev", //"prev,today,next"
                //     right: "next"//"timeGridDay,timeGridWeek,dayGridMonth,multiMonthYear,timeGridFourDay,listWeek,customButton",
                // }}
                eventTimeFormat={{
                    hour: '2-digit',
                    minute: '2-digit',
                    meridiem: true
                }}
                views={{
                    timeGridFourDay: {
                        type: 'timeGrid',
                        duration: { days: 4 },
                        buttonText: '4 day'
                    }
                }}
                // timeZone="UTC" // Set the appropriate time zone
                eventOverlap={true}
                multiMonthMaxColumns={3}
                dayMaxEvents={nDayMaxEvents}
                eventClick={(e) => { OnDateClick(e); }}
                dateClick={(e) => { OnDateClick(e); }}
                eventClassNames={"calendar-event"}
                // events={Event}
                events={lstEvent || []}
                eventDisplay={"block"}
                datesSet={(e) => {
                    onCalenderChange(e);
                }}
                weekends={true} // default : true
                // hiddenDays={[2, 4]} // hide Tuesdays and Thursdays
                dayHeaders={true} // default : true
                dayHeaderClassNames={sDayHeaderClassNames || null}
                firstDay={0} // default : 1 Monday
                weekNumbers={false} // default : false
                weekText="Week" // default : W
                eventBackgroundColor="#CCCCCC" //"#A8D1E7"
                eventTextColor="#000000"
                handleWindowResize={true} // default : true
                customButtons={{
                    customButton: {
                        text: 'custom!',
                        click: function () {
                            alert('clicked the custom button!');
                        }
                    }
                }}
                nowIndicator={true} // default : false
                businessHours={{
                    daysOfWeek: [1, 2, 3, 4, 5], // Monday - Friday
                    startTime: '09:00',
                    endTime: '18:00',
                }}
            />
        </>
    );
}