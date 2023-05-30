async function reloadMocks() {
    const response = await fetch("mocks");
    const latestMocks = await response.json();
    Alpine.store("mocks").mocks = latestMocks;
}

document.addEventListener('alpine:init', () => {
    Alpine.store('mocks', {
        mocks: []
    });
})

reloadMocks();