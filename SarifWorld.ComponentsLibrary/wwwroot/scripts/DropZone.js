const callbackMap = {};

function setCallbackTarget(id, target) {
    callbackMap[id] = target;
}

function handleDragEnter(event) {
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

function handleDrop(event) {
    // event.currentTarget is only available while the event is being handled, so it will _not_ be
    // available when the asynchronous call to callBack is made -- unless we save it here.
    const targetId = event.currentTarget.id;
    preventDefaults(event);
    const files = event.dataTransfer.files;
    const filesArray = [...files];
    if (filesArray.length == 0) {
        callBack(event.currentTarget.id, 0, null, null);
    } else {
        filesArray.forEach(file => file.text().then(text => callBack(targetId, file.name, text)));
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

function callBack(targetId, name, text) {
    callbackMap[targetId].invokeMethodAsync('HandleDroppedFile', name, text);
}
