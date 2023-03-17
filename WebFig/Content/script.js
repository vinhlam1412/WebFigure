const bigImg = document.querySelector(".product-content-left-big-img img")
const smallImg = document.querySelectorAll(".product-content-left-small-img img")
smallImg.forEach(function(imgItem, X) {
    imgItem.addEventListener("click", function() {
        bigImg.src = imgItem.src
    })
})




const chitiet = document.querySelector(".chitiet")
const huongdan = document.querySelector(".huongdan")
if (chitiet) {
    chitiet.addEventListener("click", function() {
        document.querySelector(".product-content-right-bottom-content-mota").style.display = "block"
        document.querySelector(".product-content-right-bottom-content-huongdan").style.display = "none"

    })
}
if (huongdan) {
    huongdan.addEventListener("click", function() {
        document.querySelector(".product-content-right-bottom-content-mota").style.display = "none"
        document.querySelector(".product-content-right-bottom-content-huongdan").style.display = "block"

    })
}
const butTon = document.querySelector(".product-content-right-bottom-top")
if (butTon) {
    butTon.addEventListener("click", function() {
        document.querySelector(".product-content-right-bottom-content-big").classList.toggle("activeB")
    })
}