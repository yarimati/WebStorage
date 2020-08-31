window.onload = function () {
    this.document.getElementById('currentUrl').value = window.location.href;
}

var linkButton = document.getElementById("copyLinkButton");
if (linkButton !== null) {
    linkButton.addEventListener('click', copyLink, false);
}

function copyLink() {
    var textBox = document.getElementById("currentUrl");
    textBox.select();
    document.execCommand("copy");
}
