function swapBlocks() {
    const block2 = document.getElementById('block2');
    const block5 = document.getElementById('block5');

    // Копируем стили блоков
    const block2Styles = window.getComputedStyle(block2);
    const block5Styles = window.getComputedStyle(block5);

    // Временный контейнер для хранения одного из блоков
    const temp = document.createElement('div');

    // Копируем содержимое блока 2 в временный контейнер
    temp.innerHTML = block2.innerHTML;

    // Копируем содержимое блока 5 в блок 2
    block2.innerHTML = block5.innerHTML;

    // Копируем содержимое временного контейнера в блок 5
    block5.innerHTML = temp.innerHTML;

    // Применяем одинаковые стили для блоков, чтобы они не изменили позиционирование
    block2.style.position = block2Styles.position;
    block2.style.top = block2Styles.top;
    block2.style.left = block2Styles.left;
    block2.style.transform = block2Styles.transform;
    block2.style.transform = block2Styles.transform;

    block5.style.position = block5Styles.position;
    block5.style.top = block5Styles.top;
    block5.style.left = block5Styles.left;
    block5.style.transform = block5Styles.transform;
}
window.onload = swapBlocks;


function calculateArea() {
    // Get the side length input value
    const sideLength = parseFloat(document.getElementById('sideLengthInput').value);

    // Check if the input is a valid number
    if (isNaN(sideLength) || sideLength <= 0) {
        document.getElementById('resultArea').textContent = "Будь ласка, введіть дійсне значення для сторони.";
        return;
    }

    // Formula for the area of a regular pentagon
    const area = (1/4) * Math.sqrt(5 * (5 + 2 * Math.sqrt(5))) * Math.pow(sideLength, 2);

    // Display the result in the block 2
    document.getElementById('resultArea').textContent = `Площа п’ятикутника: ${area.toFixed(2)} кв. одиниць`;
}
window.onload = calculateArea;


function reverseNumber() {
    let num = document.getElementById("numberInput").value;
    if (num) {
        let reversed = num.split('').reverse().join('');
        alert("Перевернуте число: " + reversed);

        // Зберігаємо результат у cookies
        document.cookie = "reversedNumber=" + reversed + ";path=/";

        // Ховаємо форму після обробки
        document.getElementById("inputForm").style.display = "none";

        // Перевірка наявності cookies при завантаженні сторінки
        showCookieMessage();
    }
}

// Функція для перевірки наявності cookies і показу повідомлення
function showCookieMessage() {
    let cookies = document.cookie.split(';');
    let reversedCookie = cookies.find(cookie => cookie.trim().startsWith('reversedNumber='));

    if (reversedCookie) {
        let result = reversedCookie.split('=')[1];
        alert("Дані в cookies: " + result + "\nПісля натискання кнопки «ОК» дані будуть видалені.");

        // Видалення cookies після натискання «ОК»
        document.cookie = "reversedNumber=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";

        // Показуємо повідомлення про видалення
        setTimeout(() => {
            alert("Cookies видалено!");
            location.reload(); // Перезавантажуємо сторінку після видалення cookies
        }, 100);
    }
}

// При завантаженні сторінки перевіряємо наявність cookies
window.onload = function() {
    showCookieMessage();
};




// Функция для применения цвета рамки
function changeBorderColor(color) {
    const selectedColor = document.getElementById('color-picker').value;

    const blocks = document.querySelectorAll('.number-block');

    blocks.forEach(block => {
        
        console.log("123" + block.style.borderColor)
        
        block.style.borderColor = selectedColor;
    });

    console.log("length" +  localStorage.length);

    console.log("selected" +  selectedColor);

    localStorage.setItem('borderColor', selectedColor);
    
}

// Завантаження збереженого кольору при завантаженні сторінки
window.onload = () => {
    const savedColor = localStorage.getItem('borderColor');

 


    if (savedColor) {

        console.log("saved color" + savedColor)

        // Встановлюємо збережений колір як значення color picker
        document.getElementById('color-picker').value = savedColor;

        // Застосовуємо збережений колір до рамок
        changeBorderColor(savedColor);
    }
};

document.getElementById('apply-color').addEventListener('click', changeBorderColor);



















// Функция для добавления формы при клике на блок 4
document.getElementById('block-4').addEventListener('click', function() {
    console.log('Клик по блоку 4. Форма будет добавлена.');

    // Создаём элементы формы
    var form = document.createElement('form');

    // Создаём и настраиваем элементы формы
    var labelProperty = document.createElement('label');
    labelProperty.setAttribute('for', 'cssProperty');
    labelProperty.innerText = 'Властивість CSS:';

    var inputProperty = document.createElement('input');
    inputProperty.setAttribute('type', 'text');
    inputProperty.setAttribute('id', 'cssProperty');
    inputProperty.setAttribute('placeholder', 'color');

    var labelValue = document.createElement('label');
    labelValue.setAttribute('for', 'cssValue');
    labelValue.innerText = 'Значення:';

    var inputValue = document.createElement('input');
    inputValue.setAttribute('type', 'text');
    inputValue.setAttribute('id', 'cssValue');
    inputValue.setAttribute('placeholder', 'red');

    var buttonSave = document.createElement('button');
    buttonSave.innerText = '1: Зберегти CSS';
    buttonSave.setAttribute('type', 'button');
    buttonSave.setAttribute('onclick', 'saveCSSToLocalStorage()');

    var buttonClear = document.createElement('button');
    buttonClear.innerText = '2: Очистити CSS';
    buttonClear.setAttribute('type', 'button');
    buttonClear.setAttribute('onclick', 'clearCSSFromLocalStorage()');

    // Добавляем элементы в форму
    form.appendChild(labelProperty);
    form.appendChild(inputProperty);
    form.appendChild(labelValue);
    form.appendChild(inputValue);
    form.appendChild(buttonSave);
    form.appendChild(buttonClear);

    // Добавляем форму в блок 4
    document.getElementById('block-4').appendChild(form);
});

// Функция для сохранения CSS в localStorage и применения его
function saveCSSToLocalStorage() {
    var cssProperty = document.getElementById('cssProperty').value;
    var cssValue = document.getElementById('cssValue').value;

    // Логируем данные в консоль для отладки
    console.log('CSS Property:', cssProperty);
    console.log('CSS Value:', cssValue);

    // Сохраняем CSS свойство и значение в localStorage
    localStorage.setItem(cssProperty, cssValue);

    // Применяем это свойство к тегу (например, к блоку 1)
    document.getElementById('block-1').style[cssProperty] = cssValue;

    alert("CSS збережено");
}

// Функция для очистки CSS из localStorage
function clearCSSFromLocalStorage() {
    // Логируем, что очищаем localStorage
    console.log('Очищаем CSS из localStorage');

    // Удаляем все CSS-свойства из localStorage
    localStorage.clear();

    // Сбрасываем стиль для соответствующего тега
    document.getElementById('block-1').removeAttribute('style');

    alert("CSS очищено");
}














