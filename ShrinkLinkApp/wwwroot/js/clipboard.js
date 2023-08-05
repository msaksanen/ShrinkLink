async function clipcopy() {
    try {
        let text = document.getElementById('clipboard').value;
        await navigator.clipboard.writeText(`${text}`);
        console.log(`Content copied to clipboard: ${text}`);
    } catch (err) {
        console.error('Failed to copy: ', err);
    }
}