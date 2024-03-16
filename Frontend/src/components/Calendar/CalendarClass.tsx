export interface CalendarProps {
    lstEvent: CalendarEvent[];
    OnDateClick: any;
    IsInitialView: boolean;
    onCalenderChange: any;
    views?: any;
    headerToolbar?: HeaderToolBar;
    nDayMaxEvents?: number;
    sDayHeaderClassNames?: string
    sDateDefault?: string
}

export interface CalendarEvent {
    id: string;
    groupId: string;
    start: string;
    end: string;
    title: string;
    backgroundColor?: string;
    textColor?: string;
    allDay?: boolean;
    nStatus?: number;
    IsMeetingRoom?: boolean;
}

export interface HeaderToolBar {
    left?: string;
    center?: string;
    right?: string;
}