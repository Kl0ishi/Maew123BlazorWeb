window.saveAsFile = function (fileName, contentType, content) {
    const blob = new Blob([content], { type: contentType });

    // Check if the browser supports saving files
    if (navigator.msSaveBlob) {
        // For IE and Edge
        navigator.msSaveBlob(blob, fileName);
    } else {
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = fileName;
        link.click();
        window.URL.revokeObjectURL(link.href);
    }
}
