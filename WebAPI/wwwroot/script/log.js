let page = 0;
let pageSize = 10;
let totalPages;

document.addEventListener('DOMContentLoaded', () => {
    if (!localStorage.getItem('jwt')) {
        window.location.href = 'login.html';
        return;
    }
    loadLogs();

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

const totalNumberOfLogs = () => {
    $.ajax({
        method: "GET",
        url: `/api/log/count`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
        }
    }).done(function (data) {
        console.log("Ukupan broj logova je:", data); //delete
        totalPages = data;
        document.getElementById('totalLogs').textContent = `Total Logs: ${data}`;
    }).fail(function (err) {
        alert(err.responseText || 'Failed to load total logs.');
    });
}

const loadLogs = () => {
    $.ajax({
        method: "GET",
        url: `/api/log/get/${pageSize}?page=${page}`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
        }
    }).done(function (data) {
        console.log("Logs data:", data); //delete
        displayLogs(data);
        totalNumberOfLogs();
        updatePagination(totalPages);
    }).fail(function (err) {
        alert(err.responseText || 'Failed to load logs.');
    });
}

const displayLogs = (logs) => {
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
            <td>${log.logTimeStamp}</td>
            <td>${log.logLevel}</td>
            <td>${log.logMessage}</td>
        `;
        logTable.appendChild(row);
    });
}

const updatePagination = (totalPages) => {
    document.getElementById('prevPage').disabled = (page <= 0);
    document.getElementById('nextPage').disabled = (page >= totalPages - 1);
    document.getElementById('currentPage').textContent = `Page ${page + 1}`;
}