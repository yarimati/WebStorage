$(document).ready(function () {
    $('form input').change(function () {
        $('form p').text(this.files.length + " file(s) selected");
    });
});

var inputFiles = document.getElementById('file-selector');
if (inputFiles !== null) {
    inputFiles.addEventListener('change', fileInfo, false);
}

function fileInfo() {
    const output = document.getElementById('output');
    output.innerHTML = '';
    totalWeight = 0;
    for (const file of event.target.files) {
        const li = document.createElement('li');
        const name = file.name ? file.name : 'NOT SUPPORTED';
        const size = file.size ? file.size : 'NOT SUPPORTED';
        totalWeight += file.size;
        li.textContent = `${name}, size: ${fileSize(size)}`;
        output.appendChild(li);
    }
    allowUpload(totalWeight);
    document.getElementById("spanToHide").style.display = 'none';
    document.getElementById("totalSize").innerHTML = `Total size is ${fileSize(totalWeight)}`;
}


function fileSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    if (i == 0) return bytes + ' ' + sizes[i];
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
};


function allowUpload(totalSize) {
    if (totalSize > 20971520) {
        window.alert(`Files cannot weight more than 20 MB,
Your files weight ${fileSize(totalSize)}`);
        window.location.reload(true);
    }
}

function copyToClipboard() {
    var textBox = document.getElementById("currentUrl");
    textBox.select();
    document.execCommand("copy");
}

