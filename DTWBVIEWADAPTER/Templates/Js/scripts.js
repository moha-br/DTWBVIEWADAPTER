document.addEventListener("DOMContentLoaded", () => {
    const cards = document.querySelectorAll("#card-container .card");
    let currentIndex = 0;

    function showCard(index) {
        cards.forEach((card, i) => {
            card.style.display = i === index ? "block" : "none";
        });
    }

    function nextCard() {
        if (currentIndex < cards.length - 1) {
            currentIndex++;
            showCard(currentIndex);
        }
    }

    function prevCard() {
        if (currentIndex > 0) {
            currentIndex--;
            showCard(currentIndex);
        }
    }

    document.getElementById("nextBtn").addEventListener("click", nextCard);
    document.getElementById("prevBtn").addEventListener("click", prevCard);

    // Show the first card initially
    showCard(currentIndex);
});
