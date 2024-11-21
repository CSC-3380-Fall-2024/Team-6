"use strict";
var Calendar;
(function (Calendar) {
    class CalendarEvents {
        constructor() {
            this.tooltips = [];
            this.initializeEventHandlers();
        }
        initializeEventHandlers() {
            document.addEventListener('DOMContentLoaded', () => {
                const events = document.querySelectorAll('.event-item');
                events.forEach((event) => {
                    const eventElement = event;
                    const description = eventElement.getAttribute('title');
                    if (description) {
                        eventElement.addEventListener('mouseenter', (_) => this.showTooltip(eventElement, description));
                        eventElement.addEventListener('mouseleave', () => this.hideTooltip());
                    }
                    // Navigate to event details on click
                    eventElement.addEventListener('click', () => {
                        const eventId = eventElement.dataset.eventId;
                        if (eventId) {
                            window.location.href = `/Calendar/Event/${eventId}`;
                        }
                    });
                });
            });
        }
        showTooltip(event, description) {
            const tooltip = document.createElement('div');
            tooltip.className = 'event-tooltip';
            tooltip.textContent = description;
            document.body.appendChild(tooltip);
            const rect = event.getBoundingClientRect();
            tooltip.style.left = `${rect.left}px`;
            tooltip.style.top = `${rect.top - tooltip.offsetHeight - 5}px`;
            this.tooltips.push(tooltip);
        }
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