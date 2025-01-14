document.addEventListener("DOMContentLoaded", () => {
    const table = document.getElementById("contactsTable");
    const tbody = table.querySelector("tbody");
    const rows = Array.from(tbody.rows);
    const filterInput = document.getElementById("tableFilterInput");

    // Filter functionality
    filterInput.addEventListener("input", () => {
        const filterValue = filterInput.value.trim().toLowerCase();

        rows.forEach(row => {
            const cells = Array.from(row.cells);
            const matches = cells.some(cell => {
                const input = cell.querySelector("input");
                const cellValue = input ? input.value.trim().toLowerCase() : cell.textContent.trim().toLowerCase();
                return cellValue.includes(filterValue);
            });

            row.style.display = matches ? "" : "none";
        });
    });

    // Inline Edit, Save, and Delete functionality
    table.addEventListener("click", async (e) => {
        const row = e.target.closest("tr");

        if (!row) return;

        if (e.target.classList.contains("edit-btn")) {
            const saveButton = row.querySelector(".save-btn");
            const cancelButton = row.querySelector(".cancel-btn");
            const editButton = e.target;
            const deleteButton = row.querySelector(".delete-btn");

            row.querySelectorAll(".form-control").forEach((input) => {
                input.disabled = false;
                input.style.backgroundColor = "#f9f9f9";
            });

            saveButton.style.display = "inline-block";
            cancelButton.style.display = "inline-block";
            editButton.style.display = "none";
            deleteButton.style.display = "none";
        }

        if (e.target.classList.contains("cancel-btn")) {
            const saveButton = row.querySelector(".save-btn");
            const cancelButton = e.target;
            const editButton = row.querySelector(".edit-btn");
            const deleteButton = row.querySelector(".delete-btn");

            row.querySelectorAll(".form-control").forEach((input) => {
                input.value = input.dataset.initial;
                input.disabled = true;
                input.style.backgroundColor = "";
            });

            saveButton.style.display = "none";
            cancelButton.style.display = "none";
            editButton.style.display = "inline-block";
            deleteButton.style.display = "inline-block";
        }

        if (e.target.classList.contains("save-btn")) {
            const id = row.dataset.id;
            const saveButton = e.target;
            const cancelButton = row.querySelector(".cancel-btn");
            const editButton = row.querySelector(".edit-btn");
            const deleteButton = row.querySelector(".delete-btn");

            const nameInput = row.querySelector('[data-field="Name"] input');
            const dateOfBirthInput = row.querySelector('[data-field="DateOfBirth"] input');
            const marriedInput = row.querySelector('[data-field="Married"] input');
            const phoneInput = row.querySelector('[data-field="Phone"] input');
            const salaryInput = row.querySelector('[data-field="Salary"] input');

            const contact = {
                id: parseInt(id),
                name: nameInput.value.trim(),
                dateOfBirth: dateOfBirthInput.value,
                married: marriedInput.value.trim().toLowerCase() === "true",
                phone: phoneInput.value.trim(),
                salary: parseFloat(salaryInput.value)
            };

            // Perform client-side validation
            const errors = [];
            if (!contact.name) {
                errors.push({ field: "Name", message: "Name is required." });
            }
            if (!contact.dateOfBirth) {
                errors.push({ field: "DateOfBirth", message: "Date of Birth is required." });
            }
            if (!["true", "false"].includes(marriedInput.value.trim().toLowerCase())) {
                errors.push({ field: "Married", message: "Married must be true or false." });
            }
            if (!/^\+?[0-9\s\-]+$/.test(contact.phone)) {
                errors.push({ field: "Phone", message: "Phone number is invalid." });
            }
            if (isNaN(contact.salary) || contact.salary <= 0) {
                errors.push({ field: "Salary", message: "Salary must be a positive number." });
            }

            if (errors.length > 0) {
                errors.forEach(error => {
                    const input = row.querySelector(`[data-field="${error.field}"] input`);
                    if (input) {
                        input.style.borderColor = "red";
                        const errorSpan = input.nextElementSibling;
                        errorSpan.textContent = error.message;
                        setTimeout(() => {
                            input.style.borderColor = "";
                            errorSpan.textContent = "";
                        }, 3000);
                    }
                });
                return;
            }

            try {
                const response = await fetch(`/Contact/${id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(contact)
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw errorData.errors;
                }

                row.querySelectorAll(".form-control").forEach((input) => {
                    input.disabled = true;
                    input.style.backgroundColor = "";
                });

                saveButton.style.display = "none";
                cancelButton.style.display = "none";
                editButton.style.display = "inline-block";
                deleteButton.style.display = "inline-block";
            } catch (errors) {
                alert("An unknown error occurred.");
            }
        }

        if (e.target.classList.contains("delete-btn")) {
            const id = row.dataset.id;
            const confirmDelete = confirm("Are you sure you want to delete this contact?");
            if (!confirmDelete) return;

            try {
                const response = await fetch(`/Contact/Delete/${id}`, {
                    method: "DELETE",
                });

                if (response.ok) {
                    row.remove(); 
                    alert("Contact deleted successfully.");
                } else {
                    const errorData = await response.json();
                    alert(`Failed to delete contact: ${errorData.error || "Unknown error"}`);
                }
            } catch (error) {
                alert("An error occurred while deleting the contact.");
            }
        }

    });
});
