const callbackMap = {};

let errorMultipleFilesDropped = "";

function setCallbackTarget(id, target, allowMultiple) {
    callbackMap[id] = {
        target: target,
        allowMultiple: allowMultiple
    };
}

function setAlertMessages(multipleFilesDropped) {
    errorMultipleFilesDropped = multipleFilesDropped;
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
    const callbackMapEntry = callbackMap[event.currentTarget.id];
    const target = callbackMapEntry.target;

    preventDefaults(event);
    const files = event.dataTransfer.files;
    const filesArray = [...files];
    if (filesArray.length == 0) {
        callBack(target, 0, null, null);
    } else {
        if (filesArray.length == 1 || callbackMapEntry.allowMultiple) {
            filesArray.forEach(file => file.text().then(text => callBack(target, file.name, text)));
        } else {
            alert(errorMultipleFilesDropped);
        }
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

function callBack(target, name, text) {
    target.invokeMethodAsync('HandleDroppedFile', name, text);
}
