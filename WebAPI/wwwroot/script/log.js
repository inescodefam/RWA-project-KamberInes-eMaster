let page = 0;
let pageSize = 10;

document.addEventListener('DOMContentLoaded', () => {
    if (!localStorage.getItem('jwt')) {
        window.location.href = 'login.html';
        return;
    }
    loadLogs();

    document.getElementById('pageSizeSelect').addEventListener('change', function () {
        pageSize = parseInt(this.value, 10);
        loadLogs();
    });

    document.getElementById('prevPage').onclick = () => {
        if (page > 0) {
            page--;
            loadLogs();
        }
    };
    document.getElementById('nextPage').onclick = () => {
        let total = totalNumberOfLogs();
        if (page != 0 && page * pageSize >= total) retrun;
        page++;
        loadLogs();
    };
});

const setPageSize = (size) => {
    pageSize = size;
    page = 0;
    loadLogs();
}

const totalNumberOfLogs = () => 
    $.ajax({
        method: "GET",
        url: `/api/log/count`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
        }
    }).done(function (data) {
        const totalPages = Math.ceil(data / pageSize);
        document.getElementById('totalLogs').textContent = `Total Logs: ${data}`;
        updatePagination(totalPages)
    }).fail(function (err) {
        alert(err.responseText || 'Failed to load total logs.');
    })

const loadLogs = () =>
    $.ajax({
        method: "GET",
        url: `/api/log/get/${pageSize}?page=${page}`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
        }
    }).done(function (data) {
        displayLogs(data);
    }).fail(function (err) {
        alert(err.responseText || 'Failed to load logs.');
    });

const displayLogs = (logs) => {
    totalNumberOfLogs();
    const logTable = document.getElementById('logTableBody');
    logTable.innerHTML = '';
    if (!logs || !Array.isArray(logs)) {
        const row = document.createElement('tr');
        row.innerHTML = `<td colspan="4">No logs found.</td>`;
        logTable.appendChild(row);
        return;
    }
    logs.forEach(log => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${log.idLog}</td>
            <td>${formatDateTime(log.logTimeStamp )}</td>
            <td>${log.logLevel}</td>
            <td>${log.logMessage}</td>
        `;
        logTable.appendChild(row);
    });
}

const formatDateTime = (dateTimeString) => dateTimeString ? new Date(dateTimeString).toLocaleString() : '';

const updatePagination = (totalPages) => {
    document.getElementById('prevPage').disabled = (page <= 0);
    document.getElementById('nextPage').disabled = (page >= totalPages - 1);
    document.getElementById('currentPage').textContent = `Page ${page + 1} of ${totalPages}`;
}