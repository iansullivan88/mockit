const dialog = document.getElementById('mock-dialog');

async function reloadMocks() {
    const response = await fetch('mocks');
    const latestMocks = await response.json();
    Alpine.store('mocks').mocks = latestMocks;
}

async function saveMock(mock) {
    const response = await fetch('mocks', {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(mock)
    });

}

function mockClicked(e) {
    const editModel = Alpine.store('mocks').editMock;

    editModel.id = this.mock.id;
    editModel.enabled = this.mock.enabled;
    editModel.method = this.mock.method;
    editModel.host = this.mock.host;
    editModel.path = this.mock.path;
    editModel.responseStatusCode = this.mock.responseStatusCode;
    // just support text for now
    editModel.responseText = this.mock.responseContentBase64 ? base64StringToString(this.mock.responseContentBase64) : '';
    editModel.responseHeaders = cloneHeaders(this.mock.responseHeaders);

    dialog.showModal();
}

async function saveMockClicked() {
    const editModel = Alpine.store('mocks').editMock;

    const entity = {
        id: editModel.id,
        enabled: editModel.enabled,
        method: editModel.method,
        host: editModel.host,
        path: editModel.path,
        responseStatusCode: editModel.responseStatusCode,
        // just support text for now
        responseContentBase64: editModel.responseText ? stringToBase64String(editModel.responseText) : null,
        responseHeaders: cloneHeaders(editModel.responseHeaders)
    }

    await saveMock(entity);

    await reloadMocks();

    dialog.close();
}

function cloneHeaders(headers) {
    const newHeaders = [];
    for (header of headers) {
        newHeaders.push({ ...header });
    }
    return newHeaders;
}


function base64StringToString(input) {
    const binaryString = atob(input);
    const bytes = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    const arrayBuffer = bytes.buffer;

    const decoder = new TextDecoder("utf-8");
    return decoder.decode(arrayBuffer);
}

function stringToBase64String(input) {
    const encoder = new TextEncoder();
    const data = encoder.encode(input);
    return btoa(String.fromCharCode.apply(null, data));
}

document.addEventListener('alpine:init', async () => {
    Alpine.store('mocks', {
        mocks: [],
        editMock: { responseHeaders: [] },
        saveMockClicked: saveMockClicked,
        mockClicked: mockClicked
    });

    await reloadMocks();
});