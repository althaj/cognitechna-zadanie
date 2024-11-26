"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/api/tasks").build();

//Disable the send button until connection is established.
document.getElementById("submitTask").disabled = true;

connection.on("TaskCreated", function (task) {
    createListElement(task);
});

connection.start().then(function () {
    document.getElementById("submitTask").disabled = false;

    connection.invoke("GetTasks").then(taskList => {
        console.log(taskList);
    });

}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("submitTask").addEventListener("click", function (event) {
    var task = {
        Title: document.getElementById("titleInput").value,
        Description: document.getElementById("descriptionInput").value
    };

    await connection.invoke("SubmitTask", task).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function createListElement(task) {
    var li = document.createElement("li");
    document.getElementById("taskList").appendChild(li);

    var title = document.createElement("strong");
    title.textContent = task.title;

    var description = document.createElement("small");
    description.textContent = task.description;

    li.appendChild(title);
    li.appendChild(document.createElement("br"));
    li.appendChild(description);
}