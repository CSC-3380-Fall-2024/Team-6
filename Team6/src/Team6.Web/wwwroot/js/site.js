"use strict";
var Site;
(function (Site) {
    class SiteManager {
        constructor() {
            this.initialize();
        }
        initialize() {
            document.addEventListener('DOMContentLoaded', () => {
                console.log('Site initialized with TypeScript');
                this.setupDarkModeHandler();
            });
        }
        setupDarkModeHandler() {
            const navbar = document.getElementById('navbar');
            if (navbar) {
                this.updateNavbarTheme(navbar);
                window.matchMedia('(prefers-color-scheme: dark)')
                    .addEventListener('change', () => {
                    this.updateNavbarTheme(navbar);
                });
            }
        }
        updateNavbarTheme(navbar) {
            if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
                navbar.classList.remove('navbar-light');
                navbar.classList.add('navbar-dark');
            }
            else {
                navbar.classList.remove('navbar-dark');
                navbar.classList.add('navbar-light');
            }
        }
    }
    Site.SiteManager = SiteManager;
})(Site || (Site = {}));
// Initialize the site
const siteManager = new Site.SiteManager();
//# sourceMappingURL=site.js.map