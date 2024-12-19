const apiUrl = 'https://localhost:7077/api';
document.getElementById('account-data').style.display = 'none';
document.getElementById('profile-image').style.display = 'none';
document.getElementById('person-info').style.display = 'none';
document.getElementById('address-info').style.display = 'none';
document.getElementById('log-out-btn').style.display = 'none';
if (sessionStorage.getItem('token') !== null) {
    populateData();
}


const lithuanianCities = [
    "Vilnius", "Kaunas", "Klaipėda", "Šiauliai", "Panevėžys", "Alytus", "Marijampolė", "Mažeikiai", "Jonava", "Utena",
    "Kėdainiai", "Telšiai", "Visaginas", "Tauragė", "Ukmergė", "Plungė", "Šilutė", "Kretinga", "Radviliškis", "Druskininkai",
    "Palanga", "Rokiškis", "Biržai", "Gargždai", "Kuršėnai", "Elektrėnai", "Jurbarkas", "Garliava", "Vilkaviškis", "Raseiniai",
    "Naujoji Akmenė", "Anykščiai", "Lentvaris", "Prienai", "Kelmė", "Varėna", "Kaišiadorys", "Pasvalys", "Zarasai", "Širvintos",
    "Kazlų Rūda", "Švenčionys", "Kupiškis", "Šakiai", "Šalčininkai", "Molėtai", "Šilalė", "Trakai", "Pagėgiai", "Ignalina",
    "Rietavas", "Birštonas", "Vievis", "Neringa", "Kalvarija", "Akmenė", "Skuodas", "Kybartai", "Viekšniai", "Daugai",
    "Žagarė", "Salantai", "Vabalninkas", "Varniai", "Dūkštas", "Ežerėlis", "Gelgaudiškis", "Grigiškės", "Joniškėlis", "Joniškis",
    "Jurbarkas", "Kavarskas", "Kudirkos Naumiestis", "Lazdijai", "Linkuva", "Nemenčinė", "Obeliai", "Pabradė", "Pakalniškiai",
    "Panemunė", "Paparčiai", "Pivašiūnai", "Ramygala", "Rūdiškės", "Simnas", "Skaudvilė", "Smalininkai", "Subačius", "Tytuvėnai",
    "Užventis", "Vabalninkas", "Varniai", "Venta", "Viekšniai", "Vilkija", "Žagarė", "Žiežmariai"
];

function register(event) {
    event.preventDefault();
    const username = document.getElementById('signup-username').value;
    const password = document.getElementById('signup-password').value;

    fetch(`${apiUrl}/Account/Register?username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(errorText => {
                const errors = errorText.errors;
                let errorMessage = '';

                if (Array.isArray(errors)) {
                    errors.forEach(error => {
                        errorMessage += `${error}\n`;
                    });
                } else if (typeof errors === 'object') {
                    for (const field in errors) {
                        if (errors.hasOwnProperty(field)) {
                            errorMessage += `${field}: ${errors[field].join(', ')}\n`;
                        }
                    }
                } else {
                    errorMessage = errors;
                }
                showErrorMessage(errorMessage);
            });
        }
        hideErrorMessage();
        showSuccessMessage(`Account ${username} created sucessfully`);
        return response.json();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Signup failed!');
    });
}

function login(event) {
    event.preventDefault();
    const username = document.getElementById('login-username').value;
    const password = document.getElementById('login-password').value;

    fetch(`${apiUrl}/Account/Login?username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(errorText => {
                showErrorMessage(errorText);
                return null;
            });
        }
        return response.json();
    })
    .then(data => {
        if (data) {
            hideErrorMessage();
            sessionStorage.setItem('token', data.token);
            sessionStorage.setItem('accountId', data.accountId);
            sessionStorage.setItem('role', data.role);
            sessionStorage.setItem('username', username);
            if (sessionStorage.getItem('role') === 'Admin') {
                window.location.href = "admin.html";
                return;
            }
            populateData();
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Login failed!');
    });
}

function populateData() {
    document.getElementById('logged-in-as').textContent = "Logged in as: " + sessionStorage.getItem('username') + ' Role: ' + sessionStorage.getItem('role') ;
    document.getElementById('log-out-btn').style.display = 'block';

    document.getElementById('account-data').style.display = 'block';
    document.getElementById('signup-login').style.display = 'none';
    fetch(`${apiUrl}/Account/GetData/${sessionStorage.getItem('accountId')}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${sessionStorage.getItem('token')}`
        }
    })
    .then(response => response.json())
    .then(response => {
        document.getElementById('person-info').style.display = 'block';
        if (response.person != null) {
            sessionStorage.setItem('personId', response.person.personId);
            document.getElementById('PersonName').value = response.person.name;
            document.getElementById('PersonSurname').value = response.person.surname;
            document.getElementById('PersonCode').value = response.person.personCode;
            document.getElementById('PersonEmail').value = response.person.email;
            displayImage(response.person.profilePicture);
            document.getElementById('add-person-btn').style.display = 'none';
            document.getElementById('address-info').style.display = 'block';
            showPersonUpdateButtons(true);
        } else {
            document.getElementById('add-person-btn').style.display = 'block';
            document.getElementById('address-info').style.display = 'none';
            showPersonUpdateButtons(false);
        }
        if (response.address != null) {
            sessionStorage.setItem('addressId', response.person.addressId);
            populateCityOptions(response.address.city);
            document.getElementById('AddressStreet').value = response.address.street;
            document.getElementById('AddressNumber').value = response.address.number;
            document.getElementById('add-address-btn').style.display = 'none';
            showAddressUpdateButtons(true);
        } else if (response.person != null && response.address === null) {
            document.getElementById('address-info').style.display = 'block';
            document.getElementById('add-address-btn').style.display = 'block';
            populateCityOptions();
            showAddressUpdateButtons(false);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Failed to load person!');
    });
}


function addPerson(event) {
    event.preventDefault(event);
    const name = document.getElementById('PersonName').value;
    const surname = document.getElementById('PersonSurname').value;
    const personCode = document.getElementById('PersonCode').value;
    const email = document.getElementById('PersonEmail').value;
    const image = document.getElementById('PersonImage').files[0];

    const formData = new FormData();
    formData.append('Name', name);
    formData.append('Surname', surname);
    formData.append('PersonCode', personCode);
    formData.append('Email', email);
    formData.append('ProfilePicture', image)

    fetch(`${apiUrl}/Person/CreatePerson/${sessionStorage.getItem('accountId')}`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${sessionStorage.getItem('token')}`
        },
        body: formData
    })
    .then(response => {
        if (response.ok) {
            hideErrorMessage();
            populateData();
            showSuccessMessage(`Person added successfully`);
        } else {
            handleErrorResponse(response);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Failed to add person!');
    });
}

function addAddress(event) {
    event.preventDefault(event);
    const city = document.getElementById('AddressCity').value;
    const street = document.getElementById('AddressStreet').value;
    const number = document.getElementById('AddressNumber').value;

    const formData = new FormData();
    formData.append('City', city);
    formData.append('Street', street);
    formData.append('Number', number);

    fetch(`${apiUrl}/Address/CreateAddress/${sessionStorage.getItem('personId')}`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${sessionStorage.getItem('token')}`,
        },
        body: formData
    })
    .then(response => {
        if (response.ok) {
            hideErrorMessage();
            populateData();
            showSuccessMessage(`Address added successfully`);
        } else {
            handleErrorResponse(response);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Failed to add address!');
    });
}

function displayImage(base64String) {
    const imgElement = document.getElementById('profile-image');
    imgElement.src = `data:image/jpeg;base64,${base64String}`;
    document.getElementById('profile-image').style.display = 'block';    
}

function logOut(event) {
    event.preventDefault(event);
    document.getElementById('account-data').style.display = 'none';
    document.getElementById('signup-login').style.display = 'block';
    document.getElementById('logged-in-as').textContent = "Account Management";
    document.getElementById('log-out-btn').style.display = 'none';
    document.getElementById('profile-image').style.display = 'none';
    document.getElementById('person-info').reset();
    document.getElementById('address-info').reset();
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('accountId');
    sessionStorage.removeItem('personId');
    sessionStorage.removeItem('addressId');
    sessionStorage.removeItem('role');
    sessionStorage.removeItem('username');
}

document.querySelectorAll('.update-person-btn').forEach(button => {
    button.addEventListener('click', function() {
        const fieldId = this.getAttribute('data-field');
        const message = this.getAttribute('message');
        const field = document.getElementById(fieldId);

        if (!field.checkValidity()) {
            field.reportValidity();
            return;
        }

        const value = field.type === 'file' ? field.files[0] : field.value;
        const formData = new FormData();
        formData.append(fieldId, value);
        formData.append('personId', sessionStorage.getItem('personId'));

        fetch(`${apiUrl}/Person/Update${fieldId}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${sessionStorage.getItem('token')}`,
            },
            body: formData
        })
        .then(response => {
            if (response.ok) {
                hideErrorMessage();
                populateData();
                showSuccessMessage(`Updated ${message} successfully`);
            } else {
                handleErrorResponse(response);
            }
        })
        .catch(error => {
            console.error(`Error updating ${fieldId}:`, error);
        });
    });
});

document.querySelectorAll('.update-address-btn').forEach(button => {
    button.addEventListener('click', function() {
        const fieldId = this.getAttribute('data-field');
        const message = this.getAttribute('message');
        const field = document.getElementById(fieldId);
        if (!field.checkValidity()) {
            field.reportValidity();
            return;
        }

        const formData = new FormData();
        formData.append(fieldId, field.value);
        formData.append('addressId', sessionStorage.getItem('addressId'));

        fetch(`${apiUrl}/Address/Update${fieldId}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${sessionStorage.getItem('token')}`,
            },
            body: formData
        })
        .then(response => {
            if (response.ok) {
                hideErrorMessage();
                populateData();
                showSuccessMessage(`Updated ${message} successfully`);
            } else {
                handleErrorResponse(response);
            }
        })
        .catch(error => {
            console.error(`Error updating ${fieldId}:`, error);
        });
    });
});

function togglePassword(fieldId) {
    const passwordField = document.getElementById(fieldId);
    const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordField.setAttribute('type', type);
}

function showPersonUpdateButtons(show) {
    const updateButtons = document.querySelectorAll('.update-person-btn');
    updateButtons.forEach(button => {
        show ? button.style.display = 'block' : button.style.display = 'none';
    });
}

function showAddressUpdateButtons(show) {
    const updateButtons = document.querySelectorAll('.update-address-btn');
    updateButtons.forEach(button => {
        show ? button.style.display = 'block' : button.style.display = 'none';
    });
}

function showSuccessMessage(message) {
    const successMessage = document.getElementById('success-message');
    successMessage.textContent = message;
    successMessage.style.display = 'block';

    setTimeout(() => {
        successMessage.style.display = 'none';
    }, 3000);
}

function showErrorMessage(message) {
    const errorMessage = document.getElementById('error-message');
    errorMessage.textContent = message;
    errorMessage.style.display = 'block';
}

function hideErrorMessage() {
    const errorMessage = document.getElementById('error-message');
    errorMessage.textContent = '';
    errorMessage.style.display = 'none';
}

function populateCityOptions(selectedCity) {
    const selectElement = document.getElementById('AddressCity');

    lithuanianCities.forEach(city => {
        const option = document.createElement('option');
        option.value = city;
        option.textContent = city;
        if (city === selectedCity) {
            option.selected = true;
        }
        selectElement.appendChild(option);
    });
}

function handleErrorResponse(response) {
    response.json().then(errorText => {
        const errors = errorText.errors;
        let errorMessage = '';
        for (const field in errors) {
            if (errors.hasOwnProperty(field)) {
                errorMessage += `${field}: ${errors[field].join(', ')}\n`;
            }
        }
        showErrorMessage(errorMessage);
    });
}