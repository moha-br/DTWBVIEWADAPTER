// Simple filter/search example
document.addEventListener("DOMContentLoaded", function () {
    const input = document.getElementById("searchBox");
    if (!input) return;

    input.addEventListener("keyup", function () {
        const filter = input.value.toLowerCase();
        const cards = document.querySelectorAll(".card");

        cards.forEach(card => {
            const text = card.textContent.toLowerCase();
            card.parentElement.style.display = text.includes(filter) ? "block" : "none";
        });
    });
});
