const callbackMap = {};

let errorMultipleFilesDropped = "";

function setCallbackTarget(id, callbackTarget, allowMultiple) {
    callbackMap[id] = {
        callbackTarget: callbackTarget,
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
    const targetElement = event.currentTarget;
    const id = targetElement.id;
    const callbackMapEntry = callbackMap[id];
    const callbackTarget = callbackMapEntry.callbackTarget;
    const allowMultiple = callbackMapEntry.allowMultiple;

    preventDefaults(event);
    const files = event.dataTransfer.files;
    const filesArray = [...files];
    if (filesArray.length === 0) {
        callBack(callbackTarget, 0, null, null);
    } else {
        if (filesArray.length === 1 || allowMultiple) {
            filesArray.forEach(file => file.text().then(text => {
                setDropZoneClass(id, "drop-area-text-busy");
                callBack(callbackTarget, file.name, text)
            }));
        } else {
            alert(errorMultipleFilesDropped);
        }
    }
    highlight(targetElement, false);
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

function callBack(callbackTarget, name, text) {
    callbackTarget.invokeMethodAsync('HandleDroppedFile', name, text);
}

function setDropZoneClass(id, textClass) {
    console.log(id + ": " + textClass);
    const element = document.getElementById(id);
    element.classList.remove("drop-area-text-input");
    element.classList.remove("drop-area-text-busy");
    element.classList.remove("drop-area-text-complete");
    element.classList.add(textClass);
}
