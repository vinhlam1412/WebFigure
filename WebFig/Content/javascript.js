// Hàm tăng giảm số lượng sản phẩm nhưng bất thành

// const value = documents.querySelector("#value");
// const btns = documents.querySelectorAll(".btn");

// btns.forEach(function(btn) {
//     btn.addEventListener("click", function(e) {
//         const styles = e.currentTarget.classList;
//         if (styles.contains("decrease")) {
//             count--;
//         } else if (styles.contains("increase")) {
//             count++;
//         } else {
//             count = 0;
//         }
//         value.textContent = count;
//     });
// });
const bigImg = document.querySelector(".main-img-item")
const smallImg = document.querySelectorAll(".sub-img img")
smallImg.forEach(function(imgItem) {
    imgItem.addEventListener("click", function() {
        bigImg.src = imgItem.src
    })
})