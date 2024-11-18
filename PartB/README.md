[<- Back to Main](../README.md)

# Part B: Modelling the Domain

Now that we have converted the domain into a more documented format, we can start modelling it using F#.

## Exercise B1: Modelling the Menu

Let’s start by looking at the `Menu` model:

```
module Menu =

    type Topping =
        | Pepperoni
        | Ham
        | Mushrooms
        | Onions
        | Pineapple

    type PizzaName = string

    type PizzaRecipe =
        | Predefined
        | CreateYourOwn of Topping * Topping option

    type PizzaSize =
        | Large
        | Medium
        | Small

    type Pizza =
        { Name: PizzaName
          Size: PizzaSize
          Recipe: PizzaRecipe }

    type Drink =
        | Coke
        | DietCoke
        | Fanta

    type MenuItem =
        | Pizza of Pizza
        | Drink of Drink

    type Menu = MenuItem list
```

The first thing we define is the `Topping` type, which can be one of `Pepperoni`, `Ham`, `Mushrooms`, `Onions` or `Pineapple`.

Then, we define the `PizzaName` type, which will either be the “Create your own” string, or the pre-defined name such as “meat lovers pizza”, which is also a string.
Then we define the `PizzaRecipe` type, which will either be `Predefined` or a `CreateYourOwn` pizza, the latter consisting of two toppings, the second of which is optional.

Then we define the `PizzaSize`, which can either be `Large`, `Medium`, or `Small`.

Now we can define the `Pizza` type, which consists of a `Name`, a `Size`, and a `PizzaRecipe`, which we’ve defined previously.

Then we move onto `Drink`, which can either be a `Coke`, `DietCoke`, or a `Fanta`.

Finally, a `MenuItem` is either a `Pizza` or a `Drink`, and the resulting `Menu` is a list of `MenuItems`.

All the types related to `Menu` are wrapped in a module for modularity.

## Exercise B2: Modelling ShoppingBasket

Now, let’s move onto the `ShoppingBasket`:

```
module ShoppingBasket =
    open B1_Menu

    type CustomerName = CustomerName of string
    type PhoneNumber = PhoneNumber of string

    type CustomerDetails =
        { Name: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketQuantity = BasketQuantity of int

    type BasketItem =
        { Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type KitchenId = KitchenId of int

    type ShoppingBasket =
        { CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }
```

First, we start off by opening the `Menu` module, this allows us to access the types provided in the previous module.

Then we define the `CustomerName` and `PhoneNumber` types, which are both strings.

We then create a `CustomerDetails` type which includes the `Name`, of type `CustomerName`, and the `PhoneNumber`, of type `PhoneNumber`.

Then we model the `BasketItem`, which consists of `Item`, which is of type `MenuItem` that we created in the previous section, and `Quantity`, which is of type `ItemQuantity`.

Finally, we define the `ShoppingBasket`, which consists of `CustomerDetails`, `SelectedMenuItems` which is a list of `BasketItems`, and `KitchenId` which is an integer type.

## Exercise B3: Entities and Value Objects

Now that we’ve defined some of the models in our pizza ordering system, let’s look at some of them and determine whether they are entities, value objects, or aggregates:

- `PhoneNumber`: **Value Object**. Phone Numbers as a concept in the real world are generally unique to each individual. However, a person may upgrade their phone number or have multiple phone numbers. In that sense, they are interchangeable.
- `BasketQuantity`: **Value Object**. The `BasketQuantity` object determines how much of a particular item the customer wants to buy. There’s nothing uniquely identifiable about that quantity. Its value is in the number, and that number may change depending on how many items the customer wants to purchase.
- `ShoppingBasketItem`: **Entity**. The `ShoppingBasketItem` forms part of the order, and once that order goes through it will need to be uniquely identified by the support team. For instance, if part of the order is missing, the customer might be eligible for a full or partial refund. Hence, that item should be uniquely identifiable.
- `ShoppingBasket`: **Aggregate**. The `ShoppingBasket` is formed of many `ShoppingBasketItem` sub items, which are entities. While accessing these items are important, their identity is useless outside the context of that `ShoppingBasket` item. For instance, if a customer is eligible for a partial refund, the support team would need the identifier of the `ShoppingBasket` to be able to access the `ShoppingBasketItem` and issue the refund. So you can think of that as a top-level entity that is contained of sub-entities.

Given the observations we have provided, the `MenuItem`, `ShoppingBasketItem`, and `ShoppingBasket` entities should be updated with identifier strings, usually an id of some kind, to uniquely identify them.

We can update the `MenuItem` type accordingly:

```
    type ItemChoice =
        | Pizza of Pizza
        | Drink of Drink

    type ItemId = ItemId of int

    type MenuItem =
        { ItemId: ItemId
          ItemChoice: ItemChoice }
```

Now, the `MenuItem` type includes an `ItemId` and an `ItemChoice`, which can either be a `Pizza` or a `Drink`.

Similarly, we can update the `ShoppingBasket` accordingly:

```
module ShoppingBasket =
    open B3_Menu

    type CustomerId = CustomerId of string
    type CustomerName = CustomerName of string
    type PhoneNumber = PhoneNumber of string

    type CustomerDetails =
        { CustomerId: CustomerId
          CustomerName: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketQuantity = BasketQuantity of int
    type BasketItemId = BasketItemId of int

    type BasketItem =
        { BasketItemId: BasketItemId
          Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type KitchenId = KitchenId of int
    type BasketId = BasketId of int

    type ShoppingBasket =
        { BasketId: BasketId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }
```

Notice that we created new types for the `BasketItemId` and `BasketId`, as well as updated `CustomerDetails` to include a `CustomerId`, as the customer is also an entity.

In the real world, information about the customer, such as their passport number or social security number, might be considered an entity to the customer, as it uniquely identifies them. However, within the kitchen, information about the customer is not relevant. The only information that becomes an entity in the kitchen is their order number, so they can pass the order to the correct customer. On the other hand, the order information is considered an entity within the kitchen, such as the menu item the customer collected, as they require that information to prepare the correct items for the customer. That information, however, is a value object to the customer. It provides value, but can change.

## Exercise B4: BasketQuantity

Now, let’s look at adding some constraints onto the basket quantity:

```
module BasketQuantity =
    type BasketQuantity = private BasketQuantity of int

    type MakeBasketQuantity = BasketQuantity -> BasketQuantity option

    let getQuantity quantity =
        match quantity with
        | BasketQuantity quantity -> quantity
```

First, we define a new module to store all our new types in, `BasketQuantity`. Then, we define the type `Quantity`, which is of type integer. We make this field private, so that it may only be created by calling the `MakeBasketQuantity` constructor and accessed by calling `getQuantity`.

Then we define the function signature for the `MakeBasketQuantity` function. This function will take in the integer, and it will output one of two things: It will either output a `BasketQuantity`, providing it passes the checks or, specifically, is an integer between 1 and 10. The other option is that it returns `None`, or nothing, if it does not passes the checks. Hence, the value is optional.

Finally, we define the `getQuantity` function, which returns the quantity value. This method is required because the quantity field is private.

We can add similar constraints to both `CustomerName` and `PhoneNumber`:

```
module CustomerName =
    type CustomerName = private CustomerName of string

    type MakeCustomerName = CustomerName -> CustomerName option

    let getName name =
        match name with
        | CustomerName name -> name
```

```
module PhoneNumber =
    open System.Text.RegularExpressions
    type PhoneNumber = private PhoneNumber of string

    type MakePhoneNumber = PhoneNumber -> PhoneNumber option

    let getNumber number =
        match number with
        | PhoneNumber number -> number
```

And add those type constraints to our `ShoppingBasket`:

```
module ShoppingBasket =
    open B3_Menu
    open B4_ConstrainedTypes.ConstrainedTypes.CustomerName
    open B4_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open B4_ConstrainedTypes.ConstrainedTypes.BasketQuantity

    type CustomerId = CustomerId of string
    type BasketItemId = BasketItemId of int
    type BasketId = BasketId of int
    type KitchenId = KitchenId of int

    type CustomerDetails =
        { CustomerId: CustomerId
          CustomerName: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketItem =
        { BasketItemId: BasketItemId
          Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type ShoppingBasket =
        { BasketId: BasketId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }
```

## Exercise B5: ShoppingBasket Payment

We want to make it impossible to pay when the basket is empty. To implement this design, we are going to split the `ShoppingBasket` model into states. We can think of it as being in the following states:

- `EmptyBasketState`: this is before the customer adds any items into the basket.
- `ActiveBasketState`: the customer has added item(s) into the basket, but they have not paid yet.
- `PaidBasketState`: the customer has completed the payment, and now the items in the basket are part of the Order.

This is what our `ShoppingBasket` looks like now:

```
type undefined = exn

module ShoppingBasket =
    open B3_Menu
    open B4_ConstrainedTypes.ConstrainedTypes.CustomerName
    open B4_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open B4_ConstrainedTypes.ConstrainedTypes.BasketQuantity

    type CustomerId = CustomerId of string
    type BasketItemId = BasketItemId of int
    type BasketId = BasketId of int
    type KitchenId = KitchenId of int

    type Payment = undefined

    type CustomerDetails =
        { CustomerId: CustomerId
          CustomerName: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketItem =
        { BasketItemId: BasketItemId
          Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type ActiveBasketData =
        { BasketId: BasketId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }

    type Order =
        { CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          Payment: Payment }

    type ShoppingBasket =
        | EmptyBasketState
        | ActiveBasketState of ActiveBasketData
        | PaidBasketState of Order
```

Notice how we have added a few new types into our `ShoppingBasket`. The first among them is `Payment`, which is `undefined`. Secondly, we added `ActiveBasketData`, which contains all the information about the active shopping basket, including the customer’s details, the selected menu items, and the id of the kitchen we want the order to be sent to.
We also added the `Order`, which contains the state of the basket after the order has been paid, which includes the items in the order as well as details related to the payment.

Finally, our new `ShoppingBasket` can be in our three states: `EmptyBasketState`, which requires no additional data, `ActiveBasketState`, which includes data related to the active basket, and `PaidBasketState`, which includes details related to the order and the payment.

Now, our `Pay` function will look like this:

```
module Pay =
    open B5_ShoppingBasket.ShoppingBasket

    type Pay = ActiveBasketData -> Payment -> Order
```

The function takes in the `ActiveBasketData` and the `Payment` and returns `Order`.

## Exercise B6: Alternatives for the Design of Order

We’ve looked at different ways of managing the state of the shopping basket, by following it through the stages of being empty, active, and paid. But what about the order itself? How do we track the stages of the order once it has been made? We can refactor the order type to achieve this.

This is what the `Order` type looks like now:

```
type Order =
    { CustomerDetails: CustomerDetails
        SelectedMenuItems: BasketItem list
        KitchenId: KitchenId
        Payment: Payment }
```

We have information about the customer, the menu items the customer has selected, the kitchen the order will be picked up from, and information about the payment.

Let’s refactor that further:

```
module OrderAlt =
    open B6_ShoppingBasket.ShoppingBasket

    type OrderId = OrderId of int
    type DateTime = System.DateTime
    type PaidAmount = PaidAmount of float
    type PaidTime = PaidDate of DateTime
    type PreparingTime = PreparingTime of DateTime
    type ReadyTime = ReadyTime of DateTime
    type CompletedTime = CompletedTime of DateTime

    type Payment =
        { PaidAmount: PaidAmount
          PaidTime: PaidTime }

    type PaidOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          Payment: Payment }

    type PreparingOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          PreparingTime: PreparingTime }

    type ReadyOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          ReadyOrder: ReadyTime }

    type CompletedOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          CompletedTime: CompletedTime }

    type Order =
        | Paid of PaidOrder
        | Preparing of PreparingOrder
        | Ready of ReadyOrder
        | Completed of CompletedOrder
```

First, let’s introduce a few new types. Since the `Order` object is an entity that needs to be tracked throughout its lifecycle, let’s add an `OrderId` type. We also want to track the date and time that the order was paid, started preparing, became ready, and completed, so we added a few types to track them by using the built-in `System.DateTime` library.

Now, we can split our `Order` type into four distinct states: `PaidOrder`, `PreparingOrder`, `ReadyOrder`, and `CompletedOrder`. When the customer is checks out from the `ShoppingBasket`, the state will be `ActiveBasketState` in the `ShoppingBasket`, and we track information about the customer, and the items in the basket. Then when the user makes a payment, it moves to the `PaidOrder` state in `Order`, which adds information about the payment. We’ve created a new `Payment` type to include the `PaidAmount` and the `PaidTime`.

Next, when the cook scans the ticket and picks up the order, it moves into the `PreparingOrder` stage, and the time the ticket was scanned is logged in the `PreparingTime` field and a notification is sent to the customer. When the order has finished baking, the cook scans the ticket again, and it moves into the `ReadyOrder` state, logging the time the order was ready.

Finally, the counter staff adds the drinks and scans the ticket again when the customer picks up the order, and the order can move to the last stage, `CompletedOrder`. This takes all the information about the payment and the times the ticket was scanned, so it can be stored for management reports.

This is what the `ShoppingBasket` looks like now:

```
type ShoppingBasket =
    | EmptyBasketState of ActiveBasketData
    | ActiveBasketState of ActiveBasketData
```

Now that we’ve created the types for the various states, you might notice a few things. First among them is that a lot of information is being passed around between the states that doesn’t need to be. And the second is that people generally refer to the order as having a status, and the model we created for `Order` doesn’t reflect that.

So, we can refactor our `Order` type even further:

```
module Order =
    open B6_ShoppingBasket.ShoppingBasket

    type OrderId = OrderId of int
    type DateTime = System.DateTime
    type PaidAmount = PaidAmount of float
    type PaidTime = PaidDate of DateTime
    type PreparingTime = PreparingTime of DateTime
    type ReadyTime = ReadyTime of DateTime
    type CompletedTime = CompletedDate of DateTime

    type Payment =
        { PaidAmount: PaidAmount
          PaidTime: PaidTime }

    type PaidOrderInfo = { OrderId: OrderId; Payment: Payment }

    type PreparingOrderInfo =
        { OrderId: OrderId
          PreparingTime: PreparingTime }

    type ReadyOrderInfo =
        { OrderId: OrderId
          ReadyTime: ReadyTime }

    type CompletedOrderInfo =
        { OrderId: OrderId
          Payment: Payment
          PreparingTime: PreparingTime
          ReadyTime: ReadyTime
          CompletedTime: CompletedTime }

    type OrderStatus =
        | Paid of PaidOrderInfo
        | Preparing of PreparingOrderInfo
        | Ready of ReadyOrderInfo
        | Completed of CompletedOrderInfo

    type Order =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          Status: OrderStatus }
```

Firstly, notice that throughout each stage, we’re only capturing information relevant to that stage. Secondly, we created a new type, `OrderStatus`, which tracks the lifecycle of the order and captures information relevant to that state. And finally, all the information relevant to the order, such as the customer’s details, the items in the basket, and the kitchen, is only referenced once in the `Order` type, where we also include the `Status` of that order.

This allows our new model to be more concise, cleaner, and easier to read.

## Exercise B7: Order Must Have Items

To make our `Pay` function a total function, we need to ensure that all inputs to the function correspond to an output. This means that any state that may result in an exception being raised must be handled in such a way that it’s accounted for and may return `None`.

Let’s look at what this means for our `Payment` type:

```
type PayFailedTime = PayFailedTime of DateTime
type PayFailedReason = PayFailedReason of string

type PaymentSuccessInfo =
    { PaidAmount: PaidAmount
        PaidTime: PaidTime }

type PaymentFailedInfo =
    { PayFailedTime: PayFailedTime
        PayFailedReason: PayFailedReason }

type PaymentStatus =
    | Success of PaymentSuccessInfo
    | Failed of PaymentFailedInfo

type Payment = { Status: PaymentStatus }
```

To account for payments not going through, we refactored the `Payment` type to include a `PaymentStatus`, which might either be a `Success` or `Failed`, and information associated with both of those states.

That way, we’re constraining the input into the `Pay` function to ensure that the order does not go through if the payment is unsuccessful.

Finally, let’s look at what this will now look like for our `Pay` function:

```
module BasketTransitions =
    open B6_ShoppingBasket.ShoppingBasket
    open B7_Order.Order

    type Pay = ActiveBasketData -> Payment -> Order
```

By constraining both of the inputs: that of `ActiveBasketData` by ensuring that the state transitions to `PaidOrder` only if the `ShoppingBasket` is in the `ActiveBasketState`, and `Payment` by ensuring it progresses only when it is in the `Success` state, we ensure that every input to the `Pay` function corresponds to an output, rendering it a total function.

Here are all the event transitions for the shopping basket:

```
type InitBasket = BasketId -> CustomerDetails -> BasketItem -> KitchenId -> ShoppingBasket

type AddToBasket = BasketItem -> ActiveBasketData -> ShoppingBasket

type RemoveFromBasket = BasketItem -> ActiveBasketData -> ShoppingBasket

type Pay = ActiveBasketData -> Payment -> Order
```

[<- Back to Main](../README.md)
