

// scroll to top smoothly
function IDScrollIntoView() {
    window.scroll(0, 400); //document.getElementById(id).scrollIntoView()
}

// to open a link in a new window tab
function BlazorOpenNew(url) {
    window.open(url, "_blank"); //document.getElementById(id).scrollIntoView()
}


// to open a link in a new window tab
function RandomPointlessJavaFunction(url) {
    window.open(url, "_blank"); //document.getElementById(id).scrollIntoView()
}


//for authorization management and autolog out
function initializeInactivityTimer(dotnetHelper) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 3000);
    };


    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    };
};
