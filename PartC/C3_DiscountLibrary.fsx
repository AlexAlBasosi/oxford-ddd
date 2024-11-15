#load "C3_ShoppingBasket.fsx"
#load "C3_BasketImplementation.fsx"

module DiscountLibrary =
    open C3_ShoppingBasket.ShoppingBasket
    open C3_BasketImplementation.BasketImplementation

    type DiscountPercent = int
    type SetDiscountPercent = ShoppingBasket -> DiscountPercent -> ShoppingBasket
    type ExtractPrice = TotalPrice -> float
    type SetDiscountPercent2 = DiscountPercent -> ShoppingBasket -> ShoppingBasket
    type SetDiscountPercentWrapper = SetDiscountPercent -> DiscountPercent -> ShoppingBasket -> ShoppingBasket

    let extractPrice: ExtractPrice =
        fun (totalPrice: TotalPrice) ->
            match totalPrice with
            | TotalPrice price -> price

    let setDiscountPercent: SetDiscountPercent =
        fun (shoppingBasket: ShoppingBasket) (discountPercent: DiscountPercent) ->
            let activeBasketData: ActiveBasketData = extractBasketData shoppingBasket
            let totalPrice: float = extractPrice activeBasketData.TotalPrice
            let discountedTotalPrice: float = (float discountPercent * 100.0) * totalPrice

            let newBasketData: ActiveBasketData =
                { BasketId = activeBasketData.BasketId
                  CustomerDetails = activeBasketData.CustomerDetails
                  SelectedMenuItems = activeBasketData.SelectedMenuItems
                  KitchenId = activeBasketData.KitchenId
                  TotalPrice = TotalPrice discountedTotalPrice }

            ActiveBasketState newBasketData

    let setDiscountPercent2: SetDiscountPercent2 =
        fun (discountPercent: DiscountPercent) (shoppingBasket: ShoppingBasket) ->
            let activeBasketData: ActiveBasketData = extractBasketData shoppingBasket
            let totalPrice: float = extractPrice activeBasketData.TotalPrice
            let discountedTotalPrice: float = (float discountPercent * 100.0) * totalPrice

            let newBasketData: ActiveBasketData =
                { BasketId = activeBasketData.BasketId
                  CustomerDetails = activeBasketData.CustomerDetails
                  SelectedMenuItems = activeBasketData.SelectedMenuItems
                  KitchenId = activeBasketData.KitchenId
                  TotalPrice = TotalPrice discountedTotalPrice }

            ActiveBasketState newBasketData

    let setDiscountPercentWrapper: SetDiscountPercentWrapper =
        fun (setDiscountPercent: SetDiscountPercent) (discountPercent: DiscountPercent) (shoppingBasket: ShoppingBasket) ->
            setDiscountPercent shoppingBasket discountPercent
