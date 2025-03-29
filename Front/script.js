document.addEventListener("DOMContentLoaded", async function () {
    const API_BASE_URL = "http://localhost:5044/api/cart";
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Session expired, please login again.");
        logout();
        return;
    }

    await loadCart();

    async function loadCart() {
        try {
            // جلب عدد المنتجات في السلة
            let countResponse = await fetch(`${API_BASE_URL}/count`, {
                headers: { "Authorization": `Bearer ${token}` }
            });
            let countData = await countResponse.json();
            document.getElementById("cart-count").innerText = countData.count;

            // جلب تفاصيل المنتجات
            let listResponse = await fetch(`${API_BASE_URL}/list`, {
                headers: { "Authorization": `Bearer ${token}` }
            });
            let cartItems = await listResponse.json();

            const cartContainer = document.getElementById("cart-items");
            document.getElementById("loading-text").style.display = "none";
            cartContainer.innerHTML = "";

            if (!Array.isArray(cartItems) || cartItems.length === 0) {
                cartContainer.innerHTML = "<p class='text-center text-muted'>Your cart is empty.</p>";
                return;
            }

            cartItems.forEach(item => {
                let imageUrl = item.ProductImage && item.ProductImage.trim() !== ""
                    ? (item.ProductImage.startsWith("http") ? item.ProductImage : `http://localhost:5044${item.ProductImage}`)
                    : "/images/default-product.png";

                const cartItem = document.createElement("div");
                cartItem.classList.add("cart-item");
                cartItem.innerHTML = `
                    <img src="${imageUrl}" alt="${item.ProductName}" width="70" height="70"
                        onerror="this.onerror=null; this.src='/images/default-product.png';"> 

                    <div class="cart-item-details">
                        <p>${item.ProductName}</p>
                        <p>${item.Quantity} x $${(item.SupPrice || 0).toFixed(2)}</p>
                    </div>

                    <div class="btn-group">
                        <button class="btn btn-primary btn-sm" onclick="updateQuantity(${item.Id}, 'increase')">+</button>
                        <button class="btn btn-warning btn-sm" onclick="updateQuantity(${item.Id}, 'decrease')">-</button>
                        <button class="btn btn-danger btn-sm" onclick="removeItem(${item.Id})">🗑</button>
                    </div>
                `;
                cartContainer.appendChild(cartItem);
            });
        } catch (error) {
            console.error("Error loading cart:", error);
        }
    }

    window.updateQuantity = async function (id, action) {
        try {
            let response = await fetch(`${API_BASE_URL}/${action}/${id}`, {
                method: "PUT",
                headers: { "Authorization": `Bearer ${token}` }
            });

            if (!response.ok) throw new Error("Failed to update quantity");

            await loadCart(); // تحديث السلة بدون إعادة تحميل الصفحة
        } catch (error) {
            console.error("Error updating quantity:", error);
        }
    };

    window.removeItem = async function (id) {
        try {
            let response = await fetch(`${API_BASE_URL}/remove/${id}`, {
                method: "DELETE",
                headers: { "Authorization": `Bearer ${token}` }
            });

            if (!response.ok) throw new Error("Failed to remove item");

            await loadCart(); // تحديث السلة بدون إعادة تحميل الصفحة
        } catch (error) {
            console.error("Error removing item:", error);
        }
    };

    window.logout = function () {
        localStorage.removeItem("token");
        localStorage.removeItem("token_expiry");
        window.location.href = "index.html";
    };
});
