'use strict';
(function() {
    const $jwt = 'eyJhbGciOiJSUzI1NiIsImtpZCI6ImRiZDk4YWI3MjQ2YWM1ZDQ4YTkwNjE3NjQzNjdmZDIxIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzYzNDcxMzEsImV4cCI6MTU3NjM1MDczMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJyZXNvdXJjZWFwaSJdLCJjbGllbnRfaWQiOiJhbmd1bGFyX3NwYSIsInN1YiI6IjYzOWYwOTM4LTY2MGUtNGM5Zi1hMTA2LWM2MzgwMmRjNGM2NSIsImF1dGhfdGltZSI6MTU3NjM0NzEyOSwiaWRwIjoibG9jYWwiLCJnaXZlbl9uYW1lIjoidGVzdDExOCIsImVtYWlsIjoidGVzdDExOEB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ0ZXN0MTE4QHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI2MzlmMDkzOC02NjBlLTRjOWYtYTEwNi1jNjM4MDJkYzRjNjUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ0YXhpZHJpdmVyIiwic2NvcGUiOlsiYXBpLnJlYWQiXSwiYW1yIjpbInB3ZCJdfQ.NZls6ykmgnphO140xT5JDWGNg1rQgzFTsm2vFq-jWWnZ9kKhKqbZbv7hjJTfogMAhpv8yq__0hTPU8Kld9TWwL5m3GCpcFWfu4p8Bpylhtud7Z8SlpxAKR6UThe7q_kEtq99OMR13MIpPvddtHuQmUz1Akr0Td0W28hO4XKWE8OI41BSJ0aMyzBs5xE9NdYWO42yThZNa0QFEMCuISZYjL-YMyEM7zbMuIpHLWScQtILcLQazHp0Dk0dJIGNSbxK6TEh53eYoEU0WqqPgx92DOLErUCzm16kAhtW314yLjE0w6ig4-QThShyEZKcKZ4iRPZz4AL4mPsfouuMGXxD3Q';
    const $connect = document.getElementById("connect");
    const $messages = document.getElementById("messages");
    const connection = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5007/duataxi')
        .configureLogging(signalR.LogLevel.Information)
        .build();

    $connect.onclick = function () {
       
        const jwt = 'eyJhbGciOiJSUzI1NiIsImtpZCI6ImRiZDk4YWI3MjQ2YWM1ZDQ4YTkwNjE3NjQzNjdmZDIxIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzYzNDcxMzEsImV4cCI6MTU3NjM1MDczMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJyZXNvdXJjZWFwaSJdLCJjbGllbnRfaWQiOiJhbmd1bGFyX3NwYSIsInN1YiI6IjYzOWYwOTM4LTY2MGUtNGM5Zi1hMTA2LWM2MzgwMmRjNGM2NSIsImF1dGhfdGltZSI6MTU3NjM0NzEyOSwiaWRwIjoibG9jYWwiLCJnaXZlbl9uYW1lIjoidGVzdDExOCIsImVtYWlsIjoidGVzdDExOEB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ0ZXN0MTE4QHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI2MzlmMDkzOC02NjBlLTRjOWYtYTEwNi1jNjM4MDJkYzRjNjUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ0YXhpZHJpdmVyIiwic2NvcGUiOlsiYXBpLnJlYWQiXSwiYW1yIjpbInB3ZCJdfQ.NZls6ykmgnphO140xT5JDWGNg1rQgzFTsm2vFq-jWWnZ9kKhKqbZbv7hjJTfogMAhpv8yq__0hTPU8Kld9TWwL5m3GCpcFWfu4p8Bpylhtud7Z8SlpxAKR6UThe7q_kEtq99OMR13MIpPvddtHuQmUz1Akr0Td0W28hO4XKWE8OI41BSJ0aMyzBs5xE9NdYWO42yThZNa0QFEMCuISZYjL-YMyEM7zbMuIpHLWScQtILcLQazHp0Dk0dJIGNSbxK6TEh53eYoEU0WqqPgx92DOLErUCzm16kAhtW314yLjE0w6ig4-QThShyEZKcKZ4iRPZz4AL4mPsfouuMGXxD3Q';
        if (!jwt || /\s/g.test(jwt)){
            alert('Invalid JWT.')
            return;
        }

        appendMessage("Connecting to DuaTaxi Hub...");
        connection.start()
            .then(() => {
            connection.invoke('initializeAsync', $jwt);
        })
        .catch(err => appendMessage(err));
    }
    
    connection.on('connected', _ => {
        appendMessage("Connected.", "primary");
    });

    connection.on('disconnected', _ => {
        appendMessage("Disconnected, invalid token.", "danger");
    });

    connection.on('operation_pending', (operation) => {
        appendMessage('Operation pending.', "light", operation);
    });

    connection.on('operation_completed', (operation) => {
        appendMessage('Operation completed.', "success", operation);
    });

    connection.on('operation_rejected', (operation) => {
        console.log(operation);
        appendMessage('Operation rejected.', "danger", operation);
    });
    
    function appendMessage(message, type, data) {
        var dataInfo = "";
        if (data) {
            dataInfo += "<div>" + JSON.stringify(data) + "</div>";
        }
        $messages.innerHTML += `<li class="list-group-item list-group-item-${type}">${message} ${dataInfo}</li>`;
    }
})();