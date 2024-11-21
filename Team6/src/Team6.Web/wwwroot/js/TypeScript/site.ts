namespace Site {
    export class SiteManager {
        constructor() {
            this.initialize();
        }

        private initialize(): void {
            document.addEventListener('DOMContentLoaded', (): void => {
                console.log('Site initialized with TypeScript');
                this.setupDarkModeHandler();
            });
        }

        private setupDarkModeHandler(): void {
            const navbar: HTMLElement | null = document.getElementById('navbar');
            if (navbar) {
                this.updateNavbarTheme(navbar);
                window.matchMedia('(prefers-color-scheme: dark)')
                    .addEventListener('change', (): void => {
                        this.updateNavbarTheme(navbar);
                    });
            }
        }

        private updateNavbarTheme(navbar: HTMLElement): void {
            if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
                navbar.classList.remove('navbar-light');
                navbar.classList.add('navbar-dark');
            } else {
                navbar.classList.remove('navbar-dark');
                navbar.classList.add('navbar-light');
            }
        }
    }
}

// Initialize the site
const siteManager: Site.SiteManager = new Site.SiteManager();