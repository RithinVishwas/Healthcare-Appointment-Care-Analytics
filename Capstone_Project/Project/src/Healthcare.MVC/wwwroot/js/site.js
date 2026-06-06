// Detailed JavaScript comments explain small UI/AJAX behavior. No JavaScript logic was changed.
// File: src/Healthcare.MVC/wwwroot/js/site.js
// Layer: MVC presentation layer
// Purpose: This file is the front-end JavaScript file used for UI behaviors, AJAX support, validation helpers, and dashboard interactions.
// Security note: MVC validation and antiforgery tokens help protect forms from invalid input and CSRF attacks.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

function refreshSummary(url) {
// AJAX/fetch call communicates with the backend without a full page refresh.
    fetch(url, { credentials: 'same-origin' })
        .then(response => response.ok ? response.json() : null)
        .then(data => {
            if (data && document.getElementById('totalPatients')) {
                document.getElementById('totalPatients').innerText = data.totalPatients;
            }
        });
}

function enableSlotCheck(url) {
    const doctor = document.getElementById('doctorId');
    const time = document.getElementById('appointmentTime');
    const duration = document.getElementById('duration');
    const status = document.getElementById('slotStatus');
    function check() {
        if (!doctor.value || !time.value || !duration.value) return;
        const query = `${url}?doctorId=${encodeURIComponent(doctor.value)}&dateTime=${encodeURIComponent(time.value)}&durationMinutes=${encodeURIComponent(duration.value)}`;
// AJAX/fetch call communicates with the backend without a full page refresh.
        fetch(query, { credentials: 'same-origin' })
            .then(response => response.json())
            .then(data => {
                status.textContent = data.message;
                status.className = data.available ? 'slot-status mt-3 ok' : 'slot-status mt-3 no';
            });
    }
// Event listener waits for the user/browser event before running UI logic.
    doctor.addEventListener('change', check);
// Event listener waits for the user/browser event before running UI logic.
    time.addEventListener('change', check);
// Event listener waits for the user/browser event before running UI logic.
    duration.addEventListener('change', check);
}

function loadDepartmentReport(url) {
// AJAX/fetch call communicates with the backend without a full page refresh.
    fetch(url, { credentials: 'same-origin' })
        .then(response => response.json())
        .then(rows => {
// querySelector locates the required HTML element for client-side behavior.
            const tbody = document.querySelector('#departmentReport tbody');
            tbody.innerHTML = '';
            rows.forEach(item => {
                const tr = document.createElement('tr');
                tr.innerHTML = `<td>${item.departmentName}</td><td>${item.totalAppointments}</td><td>${item.completedAppointments}</td><td>${item.scheduledAppointments}</td>`;
                tbody.appendChild(tr);
            });
        });
}
