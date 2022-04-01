export function downloadFile(text :string, fileName :string){
    var blob = new Blob([text], {type: "text/plain"});
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}