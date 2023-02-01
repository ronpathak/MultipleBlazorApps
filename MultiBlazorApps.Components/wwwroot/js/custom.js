// to open a link in a new window tab
function BlazorOpenNew(url) {
    window.open(url, "_blank"); //document.getElementById(id).scrollIntoView()
}

function openRdrawer() {
    document.getElementById("myRsidedrawer").classList.add("RsidedrawerOpen");
}

function closeRdrawer() {
    document.getElementById("myRsidedrawer").classList.remove("RsidedrawerOpen");
}

function consolePrint() {
    console.log("button pressed");
}