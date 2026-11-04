using System;
using System.Web;

namespace MyStore.Models.ViewModel
{
    // Session giữ giỏ hàng theo từng người truy cập.
    public class CartService
    {
        private const string SessionKey = "MyStoreCart";
        private readonly HttpSessionStateBase session;

        public CartService(HttpSessionStateBase session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Cart GetCart()
        {
            var cart = session[SessionKey] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                session[SessionKey] = cart;
            }
            return cart;
        }

        public void ClearCart() => session.Remove(SessionKey);
    }
}
