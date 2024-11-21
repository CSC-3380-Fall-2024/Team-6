"use strict";
// define interfaces for custom HTML elements
var Calendar;
(function (Calendar) {
    class CalendarEvents {
        constructor() {
            this.tooltips = [];
            // initialize event handlers when class is instantiated
            this.initializeEventHandlers();
        }
        // set up event listeners for calendar events
        initializeEventHandlers() {
            // wait for DOM to be fully loaded
            document.addEventListener('DOMContentLoaded', () => {
                // get all event elements
                const events = document.querySelectorAll('.event-item');
                // add handlers to each event element
                events.forEach((event) => {
                    const eventElement = event;
                    const description = eventElement.getAttribute('title');
                    // add tooltip handlers if description exists
                    if (description) {
                        eventElement.addEventListener('mouseenter', (_) => this.showTooltip(eventElement, description));
                        eventElement.addEventListener('mouseleave', () => this.hideTooltip());
                    }
                    // add click handler for navigation
                    eventElement.addEventListener('click', () => {
                        const eventId = eventElement.dataset.eventId;
                        if (eventId) {
                            window.location.href = `/Calendar/Event/${eventId}`;
                        }
                    });
                });
            });
        }
        // create and position tooltip
        showTooltip(event, description) {
            // create tooltip element
            const tooltip = document.createElement('div');
            tooltip.className = 'event-tooltip';
            tooltip.textContent = description;
            document.body.appendChild(tooltip);
            // position tooltip above event
            const rect = event.getBoundingClientRect();
            tooltip.style.left = `${rect.left}px`;
            tooltip.style.top = `${rect.top - tooltip.offsetHeight - 5}px`;
            // track tooltip for cleanup
            this.tooltips.push(tooltip);
        }
        // remove all tooltips
        hideTooltip() {
            this.tooltips.forEach((tooltip) => {
                tooltip.remove();
            });
            this.tooltips = [];
        }
    }
    Calendar.CalendarEvents = CalendarEvents;
})(Calendar || (Calendar = {}));
// Initialize the calendar events
const calendarEvents = new Calendar.CalendarEvents();
//# sourceMappingURL=calendar.js.map