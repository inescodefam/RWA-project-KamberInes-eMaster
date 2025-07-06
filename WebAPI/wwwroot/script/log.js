let page = 0;
let pageSize = 10;

document.addEventListener('DOMContentLoaded', () => {
    if (!localStorage.getItem('jwt')) {
        window.location.href = 'login.html';
        return;
    }
    loadLogs();

    document.getElementById('prevPage').onclick = () => {
        if (page > 1) {
            page--;
            loadLogs();
        }
    };
    document.getElementById('nextPage').onclick = () => {
        let total = totalNumberOfLogs();
        if (page * pageSize >= total) retrun;
        page++;
        loadLogs();
    };
});

setPageSize = (size) => {
    pageSize = size;
    page = 0;
    loadLogs(pageSize);
}

totalNumberOfLogs = () => {
    $.ajax({
        method: "GET",
        url: `/api/log/count`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
        }
    }).done(function (data) {
        document.getElementById('totalLogs').textContent = `Total Logs: ${data.total}`;
    }).fail(function (err) {
        alert(err.responseText || 'Failed to load total logs.');
    });
}

loadLogs = (pageSize) => {
    $.ajax({
        method: "GET",
        url: `/api/log/get/${pageSize}?page=${page}`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
        }
    }).done(function (data) {
        displayLogs(data.logs);
        updatePagination(data.totalPages);
    }).fail(function (err) {
        alert(err.responseText || 'Failed to load logs.');
    });
}

displayLogs = (logs) => {
    const logTable = document.getElementById('logTableBody');
    logTable.innerHTML = '';
    logs.forEach(log => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${log.id}</td>
            <td>${log.timestamp}</td>
            <td>${log.level}</td>
            <td>${log.message}</td>
        `;
        logTable.appendChild(row);
    });
}

updatePagination = (totalPages) => {
    document.getElementById('prevPage').disabled = (page <= 0);
    document.getElementById('nextPage').disabled = (page >= totalPages - 1);
    document.getElementById('currentPage').textContent = `Page ${page + 1} of ${totalPages}`;
}