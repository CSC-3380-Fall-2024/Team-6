// define interfaces for custom HTML elements
namespace Calendar {
    // interface for event elements with required methods/properties
    interface EventElement extends HTMLElement {
        getAttribute(name: string): string | null;
        getBoundingClientRect(): DOMRect; // gets element position
        dataset: DOMStringMap; // stores data attributes
    }
 
    // interface for tooltip elements with style requirements
    interface TooltipElement extends HTMLDivElement {
        style: CSSStyleDeclaration;
    }
 
    export class CalendarEvents {
        // array to track active tooltips
        private tooltips: HTMLElement[];
 
        constructor() {
            this.tooltips = [];
            // initialize event handlers when class is instantiated
            this.initializeEventHandlers();
        }
 
        // set up event listeners for calendar events
        private initializeEventHandlers(): void {
            // wait for DOM to be fully loaded
            document.addEventListener('DOMContentLoaded', (): void => {
                // get all event elements
                const events: NodeListOf<Element> = document.querySelectorAll('.event-item');
                
                // add handlers to each event element
                events.forEach((event: Element): void => {
                    const eventElement = event as EventElement;
                    const description: string | null = eventElement.getAttribute('title');
 
                    // add tooltip handlers if description exists
                    if (description) {
                        eventElement.addEventListener('mouseenter',
                            (_: Event): void => this.showTooltip(eventElement, description));
                        eventElement.addEventListener('mouseleave',
                            (): void => this.hideTooltip());
                    }
 
                    // add click handler for navigation
                    eventElement.addEventListener('click', (): void => {
                        const eventId = eventElement.dataset.eventId;
                        if (eventId) {
                            window.location.href = `/Calendar/Event/${eventId}`;
                        }
                    });
                });
            });
        }
 
        // create and position tooltip
        private showTooltip(event: EventElement, description: string): void {
            // create tooltip element
            const tooltip: TooltipElement = document.createElement('div') as TooltipElement;
            tooltip.className = 'event-tooltip';
            tooltip.textContent = description;
            document.body.appendChild(tooltip);
 
            // position tooltip above event
            const rect: DOMRect = event.getBoundingClientRect();
            tooltip.style.left = `${rect.left}px`;
            tooltip.style.top = `${rect.top - tooltip.offsetHeight - 5}px`;
            
            // track tooltip for cleanup
            this.tooltips.push(tooltip);
        }
 
        // remove all tooltips
        private hideTooltip(): void {
            this.tooltips.forEach((tooltip: HTMLElement): void => {
                tooltip.remove();
            });
            this.tooltips = [];
        }
    }
 }

// Initialize the calendar events
const calendarEvents: Calendar.CalendarEvents = new Calendar.CalendarEvents();