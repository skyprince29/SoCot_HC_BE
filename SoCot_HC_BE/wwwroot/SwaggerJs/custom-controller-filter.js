window.addEventListener("load", () => {
    setTimeout(() => {
        const swaggerInfo = document.querySelector(".swagger-ui .info");

        if (swaggerInfo && !document.querySelector(".controller-filter-buttons")) {
            // Create container
            const buttonContainer = document.createElement("div");
            buttonContainer.className = "controller-filter-buttons";
            buttonContainer.style.marginTop = "10px";

            // 🔍 Create input for filtering buttons
            const input = document.createElement("input");
            input.type = "text";
            input.placeholder = "Filter controllers...";
            input.style.marginBottom = "10px";
            input.style.padding = "6px";
            input.style.border = "1px solid #ccc";
            input.style.borderRadius = "6px";
            input.style.width = "100%";
            input.style.boxSizing = "border-box";
            buttonContainer.appendChild(input);
            input.style.padding = "15px";

            // Grab all tags
            const sections = document.querySelectorAll(".opblock-tag-section");
            const controllerNames = Array.from(sections).map(section =>
                section.querySelector(".opblock-tag").textContent.trim()
            );

            const uniqueControllers = [...new Set(controllerNames)];

            // Keep references to the buttons for filtering
            const controllerButtons = [];

            // Style helper function
            function styleButton(btn, baseColor, hoverColor, borderColor) {
                btn.style.margin = "0 5px 5px 0";
                btn.style.padding = "6px 12px";
                btn.style.border = `1px solid ${borderColor}`;
                btn.style.backgroundColor = baseColor;
                btn.style.borderRadius = "30px";
                btn.style.cursor = "pointer";
                btn.style.transition = "background-color 0.2s ease";
                btn.style.fontWeight = "bold";
            }

            // Controller filter buttons
            uniqueControllers.forEach(name => {
                const btn = document.createElement("button");
                btn.textContent = name;
                styleButton(btn, "#e7f1ff", "#d0e7ff", "#007bff");

                btn.onclick = () => {
                    sections.forEach(section => {
                        const label = section.querySelector(".opblock-tag").textContent.trim();
                        section.style.display = label === name ? "block" : "none";
                    });
                };

                controllerButtons.push({ name, btn });
                buttonContainer.appendChild(btn);
            });

            // "Show All" button
            const showAllBtn = document.createElement("button");
            showAllBtn.textContent = "Show All";
            styleButton(showAllBtn, "#e9fbe9", "#d2f5d2", "#28a745");

            showAllBtn.onclick = () => {
                sections.forEach(section => section.style.display = "block");
            };

            buttonContainer.appendChild(showAllBtn);

            // 🔍 Input filter logic
            input.addEventListener("input", () => {
                const search = input.value.toLowerCase();
                controllerButtons.forEach(({ name, btn }) => {
                    btn.style.display = name.toLowerCase().includes(search) ? "inline-block" : "none";
                });
            });

            // Insert after Swagger title
            swaggerInfo.parentNode.insertBefore(buttonContainer, swaggerInfo.nextSibling);
        }
    }, 500); // Delay to ensure Swagger UI loads
});
