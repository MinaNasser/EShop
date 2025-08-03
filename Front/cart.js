document.addEventListener("DOMContentLoaded", function () {
    const API_BASE_URL = "http://localhost:5044/api/cart";
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Session expired, please login again.");
        logout();
        return;
    }

    loadCart();

    function loadCart() {
        fetch(`${API_BASE_URL}/count`, { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => response.json())
            .then(data => document.getElementById("cart-count").innerText = data.count)
            .catch(error => console.error("Error fetching cart count:", error));


        
        fetch(`${API_BASE_URL}/list`, { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => response.json())
            .then(data => {
                const cartContainer = document.getElementById("cart-items");
                document.getElementById("loading-text").style.display = "none";
                cartContainer.innerHTML = "";

                if (!Array.isArray(data) || data.length === 0) {
                    cartContainer.innerHTML = "<p class='text-center text-muted'>Your cart is empty.</p>";
                    return;
                }

                data.forEach(item => {
                    let imageUrl = item.ProductImage && item.ProductImage.trim() !== ""
                        ? item.ProductImage
                        : "/images/default-product.png";

                    const cartItem = document.createElement("div");
                    cartItem.classList.add("cart-item");
                    cartItem.innerHTML = `
                        <img src="${imageUrl}" alt="${item.ProductName}" 
                            onerror="this.onerror=null; this.src='/images/default-product.png';"> 
                        <p>${item.ProductName} - ${item.Quantity} x $${(item.SupPrice || 0).toFixed(2)}</p>
                        <div class="btn-group">
                            <button class="btn btn-primary btn-small" onclick="updateQuantity(${item.Id}, 'increase')">+</button>
                            <button class="btn btn-warning btn-small" onclick="updateQuantity(${item.Id}, 'decrease')">-</button>
                            <button class="btn btn-danger btn-small" onclick="removeItem(${item.Id})">🗑</button>
                        </div>
                    `;
                    cartContainer.appendChild(cartItem);
                });
            })
            .catch(error => console.error("Error fetching cart items:", error));
    }

    window.updateQuantity = function (id, action) {
        fetch(`${API_BASE_URL}/${action}/${id}`, {
            method: "PUT",
            headers: { "Authorization": `Bearer ${token}` }
        })
        .then(response => {
            if (!response.ok) throw new Error("Failed to update quantity");
            return response.json();
        })
        .then(() => loadCart()) // تحديث السلة بدون إعادة تحميل الصفحة
        .catch(error => console.error("Error updating quantity:", error));
    }

    window.removeItem = function (id) {
        fetch(`${API_BASE_URL}/remove/${id}`, {
            method: "DELETE",
            headers: { "Authorization": `Bearer ${token}` }
        })
        .then(response => {
            if (!response.ok) throw new Error("Failed to remove item");
            return response.json();
        })
        .then(() => loadCart()) // تحديث السلة بدون إعادة تحميل الصفحة
        .catch(error => console.error("Error removing item:", error));
    }

    window.logout = function () {
        localStorage.removeItem("token");
        localStorage.removeItem("token_expiry");
        window.location.href = "index.html";
    }
});
