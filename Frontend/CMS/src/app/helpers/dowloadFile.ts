export function downloadFile(content :any, fileName :string){
    const type = fileName.endsWith('.txt') ? "text/plain" : "image/*"
    var blob = new Blob([content], {type: type});
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}