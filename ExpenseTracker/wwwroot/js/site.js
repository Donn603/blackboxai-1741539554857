// Initialize DataTables
$(document).ready(function () {
    // Initialize all DataTables with common configuration
    $('.table').DataTable({
        "order": [[0, "desc"]], // Sort by first column (usually date) descending
        "pageLength": 10,
        "responsive": true,
        "language": {
            "search": "Search: ",
            "lengthMenu": "Show _MENU_ entries",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": "Next",
                "previous": "Previous"
            }
        }
    });

    // Initialize date inputs to current date if empty
    $('input[type="date"]').each(function() {
        if (!$(this).val()) {
            $(this).val(new Date().toISOString().split('T')[0]);
        }
    });

    // Add confirmation for delete actions
    $('form[action*="Delete"]').submit(function(e) {
        if (!confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
            e.preventDefault();
        }
    });

    // Enable tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Format currency inputs
    $('.currency-input').on('input', function() {
        let value = $(this).val();
        value = value.replace(/[^\d.-]/g, '');
        value = parseFloat(value).toFixed(2);
        if (!isNaN(value)) {
            $(this).val(value);
        }
    });

    // Highlight active navigation item
    const currentPath = window.location.pathname.toLowerCase();
    $('.navbar-nav .nav-link').each(function() {
        const href = $(this).attr('href').toLowerCase();
        if (currentPath.includes(href) && href !== '/') {
            $(this).addClass('active');
        }
    });

    // Add animation to cards
    $('.card').hover(
        function() { $(this).addClass('shadow-lg'); },
        function() { $(this).removeClass('shadow-lg'); }
    );

    // Handle form validation
    $('form').on('submit', function() {
        if (!this.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        }
        $(this).addClass('was-validated');
    });

    // Format numbers as currency
    $('.currency').each(function() {
        const value = parseFloat($(this).text());
        if (!isNaN(value)) {
            $(this).text(new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD'
            }).format(value));
        }
    });

    // Responsive chart resizing
    function resizeCharts() {
        if (typeof categoryChart !== 'undefined') {
            categoryChart.resize();
        }
        if (typeof trendChart !== 'undefined') {
            trendChart.resize();
        }
    }

    // Handle window resize
    let resizeTimeout;
    $(window).resize(function() {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(resizeCharts, 250);
    });
});

// Show loading spinner
function showLoading() {
    const spinner = `
        <div class="position-fixed top-50 start-50 translate-middle" style="z-index: 9999">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
    `;
    $('body').append(spinner);
}

// Hide loading spinner
function hideLoading() {
    $('.spinner-border').parent().remove();
}

// Format date to local string
function formatDate(dateString) {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
}

// Handle AJAX errors
$(document).ajaxError(function(event, jqXHR, settings, error) {
    const errorMessage = jqXHR.responseText || 'An error occurred while processing your request.';
    alert(errorMessage);
});
