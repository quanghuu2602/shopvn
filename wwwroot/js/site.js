// Toast tự ẩn sau 3 giây
document.addEventListener('DOMContentLoaded', function () {
    const toast = document.getElementById('toast');
    if (toast) setTimeout(() => toast.style.opacity = '0', 3000);

    // Countdown flash sale
    const timerH = document.getElementById('t-h');
    const timerM = document.getElementById('t-m');
    const timerS = document.getElementById('t-s');
    if (timerH) {
        let secs = 2 * 3600 + 47 * 60 + 33;
        setInterval(() => {
            secs--;
            if (secs < 0) secs = 0;
            const h = Math.floor(secs / 3600);
            const m = Math.floor((secs % 3600) / 60);
            const s = secs % 60;
            timerH.textContent = String(h).padStart(2, '0');
            timerM.textContent = String(m).padStart(2, '0');
            timerS.textContent = String(s).padStart(2, '0');
        }, 1000);
    }

    // Quantity buttons trong giỏ hàng (AJAX)
    document.querySelectorAll('.qty-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            const cartItemId = this.dataset.id;
            const action = this.dataset.action; // 'increase' hoặc 'decrease'
            const qtyEl = this.closest('.qty-control').querySelector('.qty-val');

            fetch(`/Cart/UpdateQty`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': getToken() },
                body: JSON.stringify({ cartItemId, action })
            })
                .then(r => r.json())
                .then(data => {
                    if (data.success) {
                        qtyEl.textContent = data.newQty;
                        if (data.newQty === 0) location.reload();
                        // Cập nhật tổng tiền
                        const totalEl = document.getElementById('cart-total');
                        if (totalEl) totalEl.textContent = formatMoney(data.total);
                        const countEl = document.getElementById('cart-count');
                        if (countEl) countEl.textContent = data.count;
                    }
                });
        });
    });

    // Thêm vào giỏ hàng
    document.querySelectorAll('.btn-add-cart').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            const productId = this.dataset.id;
            fetch('/Cart/Add', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': getToken() },
                body: JSON.stringify({ productId, quantity: 1 })
            })
                .then(r => r.json())
                .then(data => {
                    if (data.success) showToast('Đã thêm vào giỏ hàng!');
                    const badge = document.querySelector('.cart-badge');
                    if (badge) badge.textContent = data.count;
                    else {
                        const cartBtn = document.querySelector('.cart-btn');
                        if (cartBtn) {
                            const span = document.createElement('span');
                            span.className = 'cart-badge';
                            span.textContent = data.count;
                            cartBtn.appendChild(span);
                        }
                    }
                });
        });
    });
});

function getToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
}

function showToast(msg, type = 'success') {
    const t = document.createElement('div');
    t.className = `toast toast-${type}`;
    t.innerHTML = `<span>${type === 'success' ? '✓' : '✕'}</span> ${msg}`;
    document.body.appendChild(t);
    setTimeout(() => t.style.opacity = '0', 3000);
    setTimeout(() => t.remove(), 3300);
}

function formatMoney(n) {
    return new Intl.NumberFormat('vi-VN').format(n) + 'đ';
}