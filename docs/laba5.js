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

        document.cookie = "reversedNumber=" + reversed + ";path=/";

        document.getElementById("inputForm").style.display = "none";

        showCookieMessage();
    }
}

function showCookieMessage() {
    let cookies = document.cookie.split(';');
    let reversedCookie = cookies.find(cookie => cookie.trim().startsWith('reversedNumber='));

    if (reversedCookie) {
        let result = reversedCookie.split('=')[1];
        alert("Дані в cookies: " + result + "\nПісля натискання кнопки «ОК» дані будуть видалені.");

        document.cookie = "reversedNumber=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";

        setTimeout(() => {
            alert("Cookies видалено!");
            location.reload(); 
        }, 100);
    }
}

window.onload = function() {
    showCookieMessage();
};






function toggleForm() {
    const form = document.getElementById('styleForm');
    form.style.display = form.style.display === 'none' ? 'block' : 'none';
}

function saveStyles() {
    const property = document.getElementById('cssProperty').value;
    const value = document.getElementById('cssValue').value;

    if (!property || !value) {
        alert('Будь ласка, заповніть обидва поля');
        return;
    }

    let savedStyles = JSON.parse(localStorage.getItem('titleStyles')) || {};
    savedStyles[property] = value;

    localStorage.setItem('titleStyles', JSON.stringify(savedStyles));

    applyStyles();

    document.getElementById('cssProperty').value = '';
    document.getElementById('cssValue').value = '';
}

function removeStyles() {
    localStorage.removeItem('titleStyles');
    const block7 = document.querySelector('.block-7');
    const headings = block7.querySelectorAll('h1');

    headings.forEach(heading => {
        heading.removeAttribute('style');
    });
}

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












function changeBorderColor() {
    const selectedColor = document.getElementById('colorPicker').value;
    const selectedBlock = document.getElementById('blockSelect').value;

    if (selectedBlock === 'all') {
        const blocks = document.querySelectorAll('[class^="block-"]');
        blocks.forEach(block => {
            block.style.borderColor = selectedColor;
        });
    }

    localStorage.setItem('borderColor', selectedColor);
    console.log('Сохранен цвет:', selectedColor);
}

window.onload = () => {
    const savedColor = localStorage.getItem('borderColor');

    if (savedColor) {
        console.log('Загружен сохраненный цвет:', savedColor);
        document.getElementById('colorPicker').value = savedColor;
        changeBorderColor();
    }

    document.getElementById('confirmColor').addEventListener('click', changeBorderColor);
};

