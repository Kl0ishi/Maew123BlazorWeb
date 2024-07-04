// wwwroot/scrollBehavior.js
window.scrollBehavior = {
    init: function (elementId) {
        let lastScrollTop = 0;
        const threshold = 500; // Set the threshold value in pixels

        function handleScroll() {
            const currentScrollTop = window.scrollY;
            const element = document.getElementById(elementId);

            if (currentScrollTop > threshold && currentScrollTop > lastScrollTop) {
                // Scrolling down beyond the threshold, hide the tab
                element.style.top = "-15%";
            } else {
                // Scrolling up or not beyond the threshold, show the tab
                element.style.top = "0";
            }

            lastScrollTop = currentScrollTop;
        }

        // Initial check to show the tab
        handleScroll();

        // Event listener for scroll
        window.addEventListener("scroll", handleScroll);
    }
};
