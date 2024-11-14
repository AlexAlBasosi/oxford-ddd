#load "B6_ShoppingBasket.fsx"
#load "B7_Order.fsx"

module BasketTransitions =
    open B6_ShoppingBasket.ShoppingBasket
    open B7_Order.Order

    type InitBasket = BasketId -> CustomerDetails -> BasketItem -> KitchenId -> ShoppingBasket

    type AddToBasket = BasketItem -> ActiveBasketData -> ShoppingBasket

    type RemoveFromBasket = BasketItem -> ActiveBasketData -> ShoppingBasket

    type Pay = ActiveBasketData -> Payment -> Order
