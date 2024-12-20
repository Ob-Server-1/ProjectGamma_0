window.onclick = function (event) {
  if (!event.target.matches(".dropbtn")) {
    var dropdowns = document.getElementsByClassName("dropdown-content");
    for (var i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains("show")) {
        openDropdown.classList.remove("show");
      }
    }
  }
};

function myFunction(button) {
  var hasChildren = button.querySelector(".dropdown-content"); // Проверяем наличие вложенных элементов
  if (!hasChildren) {
    // Если у кнопки нет вложенных элементов, то открываем/закрываем меню
    var dropdown = button.nextElementSibling;
    var dropdowns = document.getElementsByClassName("dropdown-content");
    Array.from(dropdowns).forEach(function (elem) {
      if (elem !== dropdown && elem.classList.contains("show")) {
        elem.classList.remove("show");
      }
    });
    dropdown.classList.toggle("show");
  }
}
function redirectToPage(url) {
  window.location.href = url; // Перенаправляем пользователя на указанный URL
}

