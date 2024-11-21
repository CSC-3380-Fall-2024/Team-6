namespace Calendar {
    interface EventElement extends HTMLElement {
        getAttribute(name: string): string | null;
        getBoundingClientRect(): DOMRect;
        dataset: DOMStringMap;
    }

    interface TooltipElement extends HTMLDivElement {
        style: CSSStyleDeclaration;
    }

    export class CalendarEvents {
        private tooltips: HTMLElement[];

        constructor() {
            this.tooltips = [];
            this.initializeEventHandlers();
        }

        private initializeEventHandlers(): void {
            document.addEventListener('DOMContentLoaded', (): void => {
                const events: NodeListOf<Element> = document.querySelectorAll('.event-item');
                events.forEach((event: Element): void => {
                    const eventElement = event as EventElement;
                    const description: string | null = eventElement.getAttribute('title');
                    
                    if (description) {
                        eventElement.addEventListener('mouseenter',
                            (_: Event): void => this.showTooltip(eventElement, description));
                        eventElement.addEventListener('mouseleave',
                            (): void => this.hideTooltip());
                    }

                    // Navigate to event details on click
                    eventElement.addEventListener('click', (): void => {
                        const eventId = eventElement.dataset.eventId;
                        if (eventId) {
                            window.location.href = `/Calendar/Event/${eventId}`;
                        }
                    });
                });
            });
        }

        private showTooltip(event: EventElement, description: string): void {
            const tooltip: TooltipElement = document.createElement('div') as TooltipElement;
            tooltip.className = 'event-tooltip';
            tooltip.textContent = description;
            document.body.appendChild(tooltip);

            const rect: DOMRect = event.getBoundingClientRect();
            tooltip.style.left = `${rect.left}px`;
            tooltip.style.top = `${rect.top - tooltip.offsetHeight - 5}px`;
            
            this.tooltips.push(tooltip);
        }

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