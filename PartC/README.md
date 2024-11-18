[<- Back to Main](../README.md)

# Part C: Designing for Pipelines

Now that we’ve modelled our menu and shopping basket, let’s look at how we can use pipelines to add and remove items from the basket.

To implement the `addToBasket` functionality so that we can use it in a pipeline, first a few things must be implemented, starting with the type constraints.

First, we implemented the type constraint for `BasketQuantity`, ensuring it only returns a quantity if the value is between 1 and 10:

```
let MakeBasketQuantity (qty: int) =
    if qty >= 1 && qty <= 10 then
        Some(BasketQuantity qty)
    else
        None
```

Secondly, we only allow the `CustomerName` to be a string of characters 50 characters or less:

```
let MakeCustomerName (name: string) =
    if name.Length <= 50 then Some(CustomerName name) else None
```

Finally, we added a regular expression to match the customer’s `PhoneNumber` against, so that it matches the _07 123 456789_ format:

```
let MakePhoneNumber (number: string) =
    let isValid =
        Regex.IsMatch(
            number,
            "^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$"
        )

    if isValid then Some(PhoneNumber number) else None
```

Then, we implemented the `initBasket` function we defined in the previous section:

```
let initBasket: InitBasket =
    fun (basketId: BasketId) (customerDetails: CustomerDetails) (kitchenId: KitchenId) ->
        let activeData: ActiveBasketData =
            { BasketId = basketId
                CustomerDetails = customerDetails
                SelectedMenuItems = []
                KitchenId = kitchenId }

        EmptyBasketState activeData
```

This takes in the information about the basket, as well as the customer’s details and kitchen id, and created an `ActiveBasketData` object. In then wraps it in an `EmptyBasketState` and returns it as a `ShoppingBasket` object.

We also implement the `addToBasket` function:

```
let addToBasket: AddToBasket =
    fun (basketItem: BasketItem) (activeBasketData: ActiveBasketData) ->
        let newBasketItems: BasketItem list =
            basketItem :: activeBasketData.SelectedMenuItems

        let newBasketData =
            { BasketId = activeBasketData.BasketId
                CustomerDetails = activeBasketData.CustomerDetails
                SelectedMenuItems = newBasketItems
                KitchenId = activeBasketData.KitchenId }

        ActiveBasketState newBasketData
```

This takes the `basketItem`, as well as the `shoppingBasket`, and extracts the `activeBasketData` from the `shoppingBasket` by calling a helper function. It then prepends the new item to the current list of menu items. Then it creates a new object with the new item added to `SelectedMenuItems`, and returns that as a `ShoppingBasket` object in the `ActiveBasketState` state.

This is the `extractBasketData` helper function:

```
let extractBasketData: ExtractBasketData =
    fun (shoppingBasket: ShoppingBasket) ->
        match shoppingBasket with
        | EmptyBasketState activeBasketData -> activeBasketData
        | ActiveBasketState activeBasketData -> activeBasketData
```

This function takes in the `shoppingBasket` and, in both empty and active states, returns basket data associated with that basket.

Finally, we can create a `TestSuite`, that creates test objects based on all the types we have created thus far.

First we initialize the empty basket:

```
module TestSuite =
    open C1_ShoppingBasket.ShoppingBasket
    open C1_ConstrainedTypes.ConstrainedTypes.CustomerName
    open C1_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open C1_ConstrainedTypes.ConstrainedTypes.BasketQuantity
    open B3_Menu.Menu
    open C1_BasketImplementation.BasketImplementation


    // Initialise empty basket

    let basketId: BasketId = BasketId 1

    let customerId: CustomerId = CustomerId 1
    let customerName: CustomerName option = MakeCustomerName "John Smith"
    let phoneNumber: PhoneNumber option = MakePhoneNumber "074658392"

    let customerDetails: CustomerDetails =
        { CustomerId = customerId
          CustomerName = customerName
          PhoneNumber = phoneNumber }

    let kitchenId: KitchenId = KitchenId 1

    let emptyBasket: ShoppingBasket = initBasket basketId customerDetails kitchenId
```

We call the `initBasket` function we initialized using the test data we created here.

Then we initialize the first menu item, which is a pepperoni pizza:

```
    // Initialise menu item 1

    let pizzaName: PizzaName = PizzaName "Pepperoni Feast"
    let pizzaSize: PizzaSize = Large
    let PizzaRecipe: PizzaRecipe = Predefined

    let pepperoniPizza: Pizza =
        { Name = pizzaName
          Size = pizzaSize
          Recipe = PizzaRecipe }

    let coke: Drink = Coke

    let itemId1: ItemId = ItemId 1

    let itemChoice1: ItemChoice = Pizza pepperoniPizza

    let menuItem1: MenuItem =
        { ItemId = itemId1
          ItemChoice = itemChoice1 }

    let basketItemId1: BasketItemId = BasketItemId 1
    let quantityItem1: BasketQuantity option = MakeBasketQuantity 1

    let basketItem1: BasketItem =
        { BasketItemId = basketItemId1
          Item = menuItem1
          Quantity = quantityItem1 }
```

And then the second menu item, which is a coke:

```
    // Initialise menu item 2

    let itemId2: ItemId = ItemId 2

    let itemChoice2: ItemChoice = Drink coke

    let menuItem2: MenuItem =
        { ItemId = itemId2
          ItemChoice = itemChoice2 }

    let basketItemId2: BasketItemId = BasketItemId 2
    let quantityItem2: BasketQuantity option = MakeBasketQuantity 2

    let basketItem2: BasketItem =
        { BasketItemId = basketItemId2
          Item = menuItem2
          Quantity = quantityItem2 }
```

So we can finally use the pipeline below:

```
    emptyBasket
    |> addToBasket basketItem1
    |> addToBasket basketItem2
```

## Exercise C1: RemoveFromBasket Type

In pipeline-oriented programming, the output of the first item in the pipeline is fed as the last argument of the second argument, and so on.

Suppose we wanted to add two items to the basket and then remove one, like this:

```
emptyBasket
|> addToBasket basketItem1
|> addToBasket basketItem2
|> removeFromBasket basketItem1
```

`addToBasket` will accept the `BasketItem` and `ShoppingBasket` and then output the `ShoppingBasket`. In that case, the `ShoppingBasket` would need to be the last argument passed into the `removeFromBasket` function.

In that case, **design 1** would be a better design, as it takes `MenuItem` as the first argument and `ShoppingBasket` as the second argument and returns `ShoppingBasket`. If we went with design 2, which takes in `MenuItem` as the last argument, it will cause a compile-time error, as it’s expecting an object of type `MenuItem` but is instead receiving an object of type `ShoppingBasket`.

So it should look like this:

```
type RemoveFromBasket = MenuItem -> ShoppingBasket -> ShoppingBasket
```

## Exercise C2: Designing Debug

If we try and add the function that the developer created into the pipeline, we will receive runtime errors, because `addToBasket` is expecting an input of type `ShoppingBasket`, but instead is receiving a `unit` type because `printfn` returns a unit when printing a string.

To fix this, we would simply need to return the `shoppingBasket` in the `debug` function to ensure it can be passed along to the next `addToBasket` function.

Our `debug` function would now look like this:

```
let debug (shoppingBasket: ShoppingBasket) =
    printfn "Debugging: %A" shoppingBasket
    shoppingBasket
    : ShoppingBasket
```

So that our pipeline now looks like this:

```
emptyBasket
|> debug
|> addToBasket basketItem1
|> debug
|> addToBasket basketItem2
|> debug
```

That way, we can debug our pipeline at every step of the way and ensure that the information that needs to be passed along are being passed.

## Exercise C3: Adapting an Existing Function for a Pipeline

Suppose we want to apply a discount to the shopping basket at checkout and include that method in our existing pipeline. First, we would need to update our existing domain to include pricing of our menu items:

```
type ItemPrice = float

type MenuItem =
    { ItemId: ItemId
        ItemChoice: ItemChoice
        ItemPrice: ItemPrice }
```

We’ve added an `ItemPrice` type to our `Menu` to reflect the fact that each item has a corresponding price.

That way, we can include a `TotalPrice` field in the `ActiveBasketData`, which will be calculated by performing a sum of all the individual item prices:

```
type TotalPrice = TotalPrice of float

type ActiveBasketData =
    { BasketId: BasketId
        CustomerDetails: CustomerDetails
        SelectedMenuItems: BasketItem list
        KitchenId: KitchenId
        TotalPrice: TotalPrice }
```

In our implementation, we would need to update `AddToBasket` to calculate the total price of the basket whenever a new item is added into the basket:

```
let addToBasket: AddToBasket =
    fun (basketItem: BasketItem) (shoppingBasket: ShoppingBasket) ->

        let activeBasketData: ActiveBasketData = extractBasketData shoppingBasket

        let newBasketItems: BasketItem list =
            basketItem :: activeBasketData.SelectedMenuItems

        let totalPrice: TotalPrice = calculateTotalPrice newBasketItems

        let newBasketData: ActiveBasketData =
            { BasketId = activeBasketData.BasketId
                CustomerDetails = activeBasketData.CustomerDetails
                SelectedMenuItems = newBasketItems
                KitchenId = activeBasketData.KitchenId
                TotalPrice = totalPrice }

        ActiveBasketState newBasketData
```

After the new basket items to be added into the basket are extracted from the `shoppingBasket`, we call a helper function that calculates the total price and include it in our `newBasketData` object which is returned from the function.

Here is the helper function that calculates the total price:

```
let calculateTotalPrice: CalculateTotalPrice =
    fun (itemList: BasketItem list) ->
        let mutable totalPrice: float = 0

        for basketItem: BasketItem in itemList do
            let itemPrice: float = basketItem.Item.ItemPrice
            totalPrice <- totalPrice + itemPrice

        TotalPrice totalPrice
```

The function takes in a list of `BasketItem`s and iterates through the list, adding the price of the item to the total price, before returning it wrapped in a `TotalPrice` object.

And finally, we implement the `DiscountLibrary` function that applies the discount to the shopping basket:

```
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
```

The `setDiscountPercent` function extracts the basket data from the shopping basket, and the total price (using helper functions), and applies the discount to the total price. It then adds that back to the object and returns it as a `ShoppingBasket` object.

Now, when we try to add the `setDiscountPercent` function to our existing pipeline, so that it looks like this:

```
emptyBasket
|> addToBasket basketItem1
|> setDiscountPercent 10
|> addToBasket basketItem2

```

It causes a compile-time error because it is expected an input of type `ShoppingBasket` as the last parameter, as that is what is being outputted from the `addToBasket` function.

Let’s implement a second function, `setDiscountPercent2` that corrects this:

```
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
```

We simply swap the order of the two parameters so that `shoppingBasket` is the last parameter, allowing it to fit into our existing pipeline:

```
emptyBasket
|> addToBasket basketItem1
|> setDiscountPercent2 10
|> addToBasket basketItem2
```

This will now compile without any errors.

What if we specifically want to call `setDiscountPercent` in our pipeline? We can create a generic wrapper function that accepts arguments in the correct order, and call the `setDiscountPercent` function, regardless of the order of its arguments:

```
let setDiscountPercentWrapper: SetDiscountPercentWrapper =
    fun (setDiscountPercent: SetDiscountPercent) (discountPercent: DiscountPercent) (shoppingBasket: ShoppingBasket) ->
        setDiscountPercent shoppingBasket discountPercent
```

Our function accepts the `setDiscountPercent` as an argument, as well as the arguments in the correct order, and is calls the function in the order which it was implemented.

Allowing us to use it in a pipeline like this:

```
emptyBasket
|> addToBasket basketItem1
|> setDiscountPercentWrapper setDiscountPercent 10
|> addToBasket basketItem2
```

This time there will be no compilation errors because it’s now accepting and returning the correct arguments.

[<- Back to Main](../README.md)
