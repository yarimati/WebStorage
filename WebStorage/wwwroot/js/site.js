if ($(window).width() < 600) {
    const listOfMobileElements = document.getElementsByClassName('forMobile');
    for (var i = 0; i < listOfMobileElements.length; i++) {
        listOfMobileElements[i].style.display = 'block';
    }
}
else {
    const listOfComputerElements = document.getElementsByClassName('forComputers');
    for (var i = 0; i < listOfComputerElements.length; i++) {
        listOfComputerElements[i].style.display = 'block';
    }
}

$(document).ready(function () {
    $('form input').change(function () {
        $('form p').text(this.files.length + " file(s) selected");
    });
});

function fileInfo() {
    const output = document.getElementById('output');
    if (window.FileList && window.File) {
        document.getElementById('file-selector').addEventListener('change', event => {
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
        });
    }
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