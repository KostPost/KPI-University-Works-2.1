function swapBlocks() {
    const block2 = document.getElementById('block2');
    const block5 = document.getElementById('block5');

    const block2Styles = window.getComputedStyle(block2);
    const block5Styles = window.getComputedStyle(block5);

    const temp = document.createElement('div');

    temp.innerHTML = block2.innerHTML;

    block2.innerHTML = block5.innerHTML;

    block5.innerHTML = temp.innerHTML;

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

const button = document.getElementById('swapButton');
button.addEventListener('click', swapBlocks);
window.onload = swapBlocks;





function calculateArea() {
    const sideLength = parseFloat(document.getElementById('sideLengthInput').value);

    if (isNaN(sideLength) || sideLength <= 0) {
        document.getElementById('resultArea').textContent = "Будь ласка, введіть дійсне значення для сторони.";
        return;
    }

    const area = (1 / 4) * Math.sqrt(5 * (5 + 2 * Math.sqrt(5))) * Math.pow(sideLength, 2);

    document.getElementById('resultArea').textContent = `Площа: ${area.toFixed(2)}`;
}

document.getElementById('calculateButton').addEventListener('click', calculateArea);



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























// Функція для відображення/приховування форми
function toggleForm() {
    const form = document.getElementById('styleForm');
    form.style.display = form.style.display === 'none' ? 'block' : 'none';
}

// Функція для збереження стилів
function saveStyles() {
    const property = document.getElementById('cssProperty').value;
    const value = document.getElementById('cssValue').value;

    if (!property || !value) {
        alert('Будь ласка, заповніть обидва поля');
        return;
    }

    // Отримуємо існуючі стилі або створюємо новий об'єкт
    let savedStyles = JSON.parse(localStorage.getItem('titleStyles')) || {};
    savedStyles[property] = value;

    // Зберігаємо оновлені стилі
    localStorage.setItem('titleStyles', JSON.stringify(savedStyles));

    // Застосовуємо стилі до всіх H1 в блоці block-7
    applyStyles();

    // Очищаємо поля форми
    document.getElementById('cssProperty').value = '';
    document.getElementById('cssValue').value = '';
}

// Функція для видалення стилів
function removeStyles() {
    localStorage.removeItem('titleStyles');
    // Знаходимо всі H1 в блоці block-7
    const block7 = document.querySelector('.block-7');
    const headings = block7.querySelectorAll('h1');

    // Видаляємо стилі з кожного H1
    headings.forEach(heading => {
        heading.removeAttribute('style');
    });
}

// Функція для застосування стилів
function applyStyles() {
    const block7 = document.querySelector('.block-7');
    const headings = block7.querySelectorAll('h1');
    const savedStyles = JSON.parse(localStorage.getItem('titleStyles')) || {};

    let styleString = '';
    for (let property in savedStyles) {
        styleString += `${property}: ${savedStyles[property]}; `;
    }

    headings.forEach(heading => {
        heading.style.cssText = styleString;
    });
}

document.addEventListener('DOMContentLoaded', function() {
    applyStyles();
});








