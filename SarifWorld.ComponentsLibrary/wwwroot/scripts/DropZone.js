function handleDragEnter (event) {
    preventDefaults(event);
    highlight(true);
}

function handleDragOver (event) {
    preventDefaults(event);
}

function handleDragLeave (event) {
    preventDefaults(event);
    highlight(false);
}

function handleDrop (event) {
    preventDefaults(event);
    const files = event.dataTransfer.files;
    const filesArray = [...files];
    const count = filesArray.length;
    if (filesArray.length == 0) {
        analyze(0, null, null);
    } else {
        const file = filesArray[0];
        file.text().then(text => analyze(count, file.name, text));
    }
    highlight(false);
}

function preventDefaults (event) {
    event.preventDefault();
    event.stopPropagation();
}

function highlight (on) {
    if (on) {;
        let element = document.getElementById('dropZone');
        element.classList.add("drop-zone-active");
    } else {
        let element = document.getElementById('dropZone');
        element.classList.remove("drop-zone-active");
    }
}

analyze = function (count, name, text) {
    validator.invokeMethodAsync('ValidateDroppedFiles', count, name, text);
}
