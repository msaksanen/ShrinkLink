function expdate() {
    let loggedIn = document.getElementById('loggedIn');
    let expd = document.getElementById('expdate');
    if (loggedIn !== null)
        expd.removeAttribute("hidden");
}
window.onload = expdate;
