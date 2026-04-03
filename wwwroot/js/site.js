document.addEventListener('DOMContentLoaded', function () {

    // Hiện thông báo góc dưới phải
    function showMsg(msg, type) {
        document.querySelectorAll('.msg-js').forEach(t => t.remove());
        const d = document.createElement('div');
        d.className = 'msg-js';
        d.style.cssText =
            'position:fixed;bottom:24px;right:24px;z-index:9999;' +
            'padding:12px 20px;border-radius:99px;font-size:14px;' +
            'color:#fff;' +
            'background:' + (type === 'error' ? '#c0392b' : '#1a6b4a');
        d.textContent = (type === 'error' ? '✕ ' : '✓ ') + msg;
        document.body.appendChild(d);
        setTimeout(() => { d.style.opacity = '0'; d.style.transition = 'opacity 0.4s'; }, 2800);
        setTimeout(() => d.remove(), 3300);
    }
    window.showMsg = showMsg;

    // Cập nhật số trên icon giỏ hàng
    function updateBadge(count) {
        const btn = document.querySelector('.cart-btn');
        if (!btn) return;
        let badge = btn.querySelector('.cart-badge');
        if (count > 0) {
            if (!badge) {
                badge = document.createElement('span');
                badge.className = 'cart-badge';
                btn.appendChild(badge);
            }
            badge.textContent = count;
        } else {
            badge?.remove();
        }
    }
    window.updateBadge = updateBadge;

    // ── THÊM VÀO GIỎ HÀNG ──
    document.querySelectorAll('.btn-add-cart').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            const productId = parseInt(this.dataset.id);
            if (!productId) {
                showMsg('Không tìm thấy sản phẩm!', 'error');
                return;
            }

            const self = this;
            const originalHTML = self.innerHTML;
            self.disabled = true;

            fetch('/Cart/Add', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ productId: productId, quantity: 1 })
            })
                .then(function (r) { return r.json(); })
                .then(function (data) {
                    if (data.success) {
                        showMsg('Đã thêm vào giỏ hàng!');
                        updateBadge(data.count);
                    } else {
                        showMsg(data.message || 'Có lỗi!', 'error');
                    }
                })
                .catch(function (err) {
                    console.error('Cart error:', err);
                    showMsg('Có lỗi xảy ra!', 'error');
                })
                .finally(function () {
                    self.innerHTML = originalHTML;
                    self.disabled = false;
                });
        });
    });

    // ── TĂNG GIẢM SỐ LƯỢNG GIỎ HÀNG ──
    document.querySelectorAll('.qty-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const cartItemId = parseInt(this.dataset.id);
            const action = this.dataset.action;
            if (!cartItemId) return;

            const ctrl = this.closest('.qty-control');
            ctrl?.querySelectorAll('.qty-btn').forEach(b => b.disabled = true);

            fetch('/Cart/UpdateQty', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ cartItemId: cartItemId, action: action })
            })
                .then(r => r.json())
                .then(function (data) {
                    if (!data.success) return;

                    if (data.newQty <= 0) {
                        document.getElementById('cart-row-' + cartItemId)?.remove();
                        if (data.count === 0) location.reload();
                    } else {
                        const el = document.getElementById('qty-' + cartItemId);
                        if (el) el.textContent = data.newQty;
                    }

                    const fmt = n =>
                        new Intl.NumberFormat('vi-VN').format(Math.round(n)) + 'đ';
                    const t1 = document.getElementById('cart-total');
                    const t2 = document.getElementById('cart-total-main');
                    if (t1) t1.textContent = fmt(data.total);
                    if (t2) t2.textContent = fmt(data.total);
                    updateBadge(data.count);
                })
                .catch(err => console.error(err))
                .finally(() => {
                    ctrl?.querySelectorAll('.qty-btn').forEach(b => b.disabled = false);
                });
        });
    });

    // ── FLASH SALE COUNTDOWN ──
    const tH = document.getElementById('t-h');
    const tM = document.getElementById('t-m');
    const tS = document.getElementById('t-s');
    if (tH && tM && tS) {
        let s = 9999;
        setInterval(function () {
            if (s > 0) s--;
            tH.textContent = String(Math.floor(s / 3600)).padStart(2, '0');
            tM.textContent = String(Math.floor((s % 3600) / 60)).padStart(2, '0');
            tS.textContent = String(s % 60).padStart(2, '0');
        }, 1000);
    }

    // ── TOAST TEMPDATA TỰ ẨN ──
    const toast = document.getElementById('toast');
    if (toast) {
        setTimeout(() => {
            toast.style.transition = 'opacity 0.5s';
            toast.style.opacity = '0';
        }, 3000);
        setTimeout(() => toast.remove(), 3500);
    }

});