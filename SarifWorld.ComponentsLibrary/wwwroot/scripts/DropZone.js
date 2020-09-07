function handleDragEnter (event) {
    preventDefaults(event);
    highlight(event.currentTarget, true);
}

function handleDragOver (event) {
    preventDefaults(event);
}

function handleDragLeave (event) {
    preventDefaults(event);
    highlight(event.currentTarget, false);
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
    highlight(event.currentTarget, false);
}

function preventDefaults (event) {
    event.preventDefault();
    event.stopPropagation();
}

function highlight (element, on) {
    if (on) {;
        element.classList.add("drop-area-active");
    } else {
        element.classList.remove("drop-area-active");
    }
}

analyze = function (count, name, text) {
    validator.invokeMethodAsync('ValidateDroppedFiles', count, name, text);
}
