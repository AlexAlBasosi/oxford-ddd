#load "B5_ShoppingBasket.fsx"

module Pay =
    open B5_ShoppingBasket.ShoppingBasket

    type Pay = ActiveBasketData -> Payment -> Order
