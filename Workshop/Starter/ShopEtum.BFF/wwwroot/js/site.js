// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (document.getElementById('btnLocalUserClaims')) {

    document.getElementById('btnLocalUserClaims').addEventListener('click', function () {
        fetch('/localapi/currentuserclaims')
            .then(response => response.json())
            .then(data => {
                let resultDiv = document.getElementById('apiResult');
                resultDiv.innerHTML = '<p>' + data.message + '</p>';
                data.claims.forEach(claim => {
                    resultDiv.innerHTML += `<li>${claim.type}: ${claim.value}</li>`;
                });
                resultDiv.innerHTML += '</ul>';
            })
            .catch(error => console.error('Error:', error));
    });
} 

if (document.getElementById('btnRemoteProducts')) {

    document.getElementById('btnRemoteProducts').addEventListener('click', function () {
        fetch('/proxy/products')
            .then(response => response.json())
            .then(data => {
                let resultDiv = document.getElementById('apiResult');
                data.forEach(product => {
                    resultDiv.innerHTML += `<li>${product.id} ${product.name} ${product.description} ${product.price}</li>`;
                });
                resultDiv.innerHTML += '</ul>';
            })
            .catch(error => console.error('Error:', error));
    });
}