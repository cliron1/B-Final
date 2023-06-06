const loginPanel = document.querySelector('#loginPanel');
const actionsPanel = document.querySelector('#actionsPanel');
const tbToken = document.querySelector('#tbToken');

const api = 'https://localhost:7248'

let token = sessionStorage.getItem('t');
if (token) {
  loggedin(token);
}

document.querySelector('.btn-login')
  .addEventListener('click', async function (e) {
    e.preventDefault();
    const payload = {
      email: document.querySelector('#login_email').value,
      pwd: document.querySelector('#login_pwd').value,
    };
    const response = await fetch(`${api}/auth/login`, {
      method: 'POST',
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });
    if (!response.ok) {
      const msg = await response.text();
      alert(`${response.status} : ${msg}`);
      return;
    }

    const result = await response.json();
    token = result.token;
    sessionStorage.setItem('t', token);
    loggedin(token);
  });

document.querySelector('.btn-signup')
  .addEventListener('click', async function (e) {
    e.preventDefault();
    const payload = {
      name: document.querySelector('#signup_name').value,
      email: document.querySelector('#signup_email').value,
      pwd: document.querySelector('#signup_pwd').value,
    };
    const response = await fetch(`${api}/auth/signup`, {
      method: 'POST',
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });
    if (!response.ok) {
      const msg = await response.text();
      alert(`${response.status} : ${msg}`);
      return;
    }

    const result = await response.json();
    token = result.token;
    sessionStorage.setItem('t', token);
    loggedin(token);
  });

document.querySelector('#btnLogout')
  .addEventListener('click', function (e) {
    e.preventDefault();
    loggedout();
  })

document.querySelector('#btnGetProds')
  .addEventListener('click', async function (e) {
    e.preventDefault();
    const response = await fetch(`${api}/products`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
    if (!response.ok) {
      const msg = await response.text();
      alert(`${response.status} : ${msg}`);
      return;
    }

    const result = await response.json();

    let lis = '';
    for (const prod of result) {
      lis += `<li class="list-group-item">${prod.name} - ${prod.price}$</li>`
    }
    document.querySelector('.prods').innerHTML = lis;
  })

function loggedin(token) {
  tbToken.value = token;
  loginPanel.classList.add('d-none');
  actionsPanel.classList.remove('d-none');
}
function loggedout() {
  tbToken.value = token;
  sessionStorage.removeItem('t');
  loginPanel.classList.remove('d-none');
  actionsPanel.classList.add('d-none');
  document.querySelector('.prods').innerHTML = '';
}