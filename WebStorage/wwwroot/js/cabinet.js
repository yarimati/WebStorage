function copyLinkCabinet(el) {
    el.select();
    document.execCommand('copy');
}

var call = function (elementId) {
    var valueOfInput = "https://" + document.getElementById(elementId).value
    window.location = valueOfInput;
}