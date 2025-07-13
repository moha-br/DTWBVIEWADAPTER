document.addEventListener("DOMContentLoaded", () => {
    const cards = document.querySelectorAll(".selectable-card");

    cards.forEach(card => {
        card.addEventListener("click", () => {
            // Remove "selected" class from all cards
            cards.forEach(c => c.classList.remove("selected"));

            // Add "selected" class to clicked card
            card.classList.add("selected");

            // Retrieve data
            const selectedData = {
                id: card.dataset.id,
                enc: card.dataset.enc,
                linen: card.dataset.linen,
                prodids: card.dataset.prodids,
                sector: card.dataset.sector
            };

            // Output to console or display on page
            console.log("Selected Card:", selectedData);

            const output = document.getElementById("selectedOutput");
            if (output) {
                output.innerHTML = `
                    <strong>id:</strong> ${selectedData.id} |
                    <strong>enc:</strong> ${selectedData.enc} |
                    <strong>linen:</strong> ${selectedData.linen} |
                    <strong>prodids:</strong> ${selectedData.prodids} |
                    <strong>sector:</strong> ${selectedData.sector}
                `;
            }

            //  Send data to .NET host app only once per click
            if (window.chrome?.webview?.postMessage) {
                const message = {
                    type: "card_selected",
                    carddata: selectedData
                };
                window.chrome.webview.postMessage(JSON.stringify(message));
            }

         
            // Find the div containing <strong>ANuser:</strong> and get its text content
            const anuserDiv = Array.from(card.querySelectorAll('.info-grid > div'))
                .find(div => div.querySelector('strong')?.textContent.trim() === 'ANuser:');

            if (anuserDiv) {
                const anuserValue = anuserDiv.textContent.replace('ANuser:', '').trim();

                if (anuserValue === 'User0004') {
                    card.classList.add('highlight-anuser');

                    // Move this card to the top of its parent
                    const parent = card.parentElement;
                    if (parent) {
                        parent.prepend(card);
                    }
                    // Fix card at the top, only once
                    const container = card.parentElement;

                    // Only move if not already at the top
                    if (container && container.firstElementChild !== card) {
                        container.insertBefore(card, container.firstElementChild);
                    }
                }
            }



        });
    });
});
