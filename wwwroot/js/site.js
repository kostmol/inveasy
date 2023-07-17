// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function trigger() {
    fileInput.click()
}

function fileHandler(e) {
    const file = this.files[0]
    document.getElementById('avatar').src = URL.createObjectURL(file)
}

const image = document.getElementById('avatar')
image.addEventListener("mouseup", trigger)
