// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (document.getElementById('callClassicLocalApiButton')) {

    document.getElementById('callClassicLocalApiButton').addEventListener('click', function () {
        fetch('/classiclocalapi/hello')
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

if (document.getElementById('callMinimalLocalApiButton')) {

    document.getElementById('callMinimalLocalApiButton').addEventListener('click', function () {
        fetch('/minimallocalapi/hello')
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

if (document.getElementById('callRemoteApiButton')) {

    document.getElementById('callRemoteApiButton').addEventListener('click', function () {
        fetch('/proxytoremoteapi/hello')
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