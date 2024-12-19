var token = sessionStorage.getItem('token');
var accountId = sessionStorage.getItem('accountId');
const apiUrl = 'https://localhost:7077/api';
getAllUsers();

function getAllUsers() {
    fetch(`${apiUrl}/Admin/GetAllAccounts/`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error');
        }
        return response.json();
    })
    .then(data => {
        showAllusers(data);
    })
    .catch(error => {
        console.error('Error');
    });
}

function showAllusers(users) {
    console.log(users);
    const adminDiv = document.getElementById('admin');
    adminDiv.innerHTML = ''; 

    const table = document.createElement('table');
    table.classList.add('user-table');

    // Create table header
    const thead = document.createElement('thead');
    const headerRow = document.createElement('tr');

    const headers = ['Account ID', 'Username', 'Actions'];
    headers.forEach(headerText => {
        const th = document.createElement('th');
        th.textContent = headerText;
        headerRow.appendChild(th);
    });

    thead.appendChild(headerRow);
    table.appendChild(thead);

    // Create table body
    const tbody = document.createElement('tbody');

    users.forEach(user => {
        const row = document.createElement('tr');

        const accountIdCell = document.createElement('td');
        accountIdCell.textContent = user.accountId;
        row.appendChild(accountIdCell);

        const usernameCell = document.createElement('td');
        usernameCell.textContent = user.username;
        row.appendChild(usernameCell);

        const actionsCell = document.createElement('td');
        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.onclick = () => deleteUser(user.accountId);
        actionsCell.appendChild(deleteButton);
        row.appendChild(actionsCell);

        tbody.appendChild(row);
    });

    table.appendChild(tbody);
    adminDiv.appendChild(table);
}

function deleteUser(accountId) {
    fetch(`${apiUrl}/Admin/Delete/?accountId=${encodeURIComponent(accountId)}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    })
    .then(response => {
        if (response.ok) {
            getAllUsers();
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Failed to load all users');
    });
}

function logOut(event) {
    event.preventDefault(event);
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('accountId');
    window.location.href = "index.html";
}
