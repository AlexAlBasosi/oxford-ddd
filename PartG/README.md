[<- Back to Main](../README.md)

# Part G: Serialization

In order to serialize our domain object so that they may be transmitted as JSON objects over the wire, we must first convert some of our objects to Data Transfer Objects (DTOs).

## Exercise G1: Serializing Pizza

Let's start by serializing the `PizzaRecipe` and `Pizza` objects we created in our previous sections.

Let's start off with `PizzaRecipe`:

```
module PizzaDto =
    type ToppingDto =
        { tagTopping1: string
          tagTopping2: string }

    type PizzaRecipeDto = { tag: string; toppings: ToppingDto }
```

First, we have to convert our `Topping` type as it's used within the `PizzaRecipe` object. In our domain, this is what our `PizzaRecipe` looked like:

```
    type Topping =
        | Pepperoni
        | Ham
        | Mushrooms
        | Onions
        | Pineapple

    type PizzaRecipe =
        | Predefined
        | CreateYourOwn of Topping * Topping option
```

Notice that we supply two `Topping` objects, the second of which is optional. In F#, it is difficult to create `Nullable<string>`s because they're reference objects, so in our `ToppingDto` we create two `tag`s, and we can supply an empty string, `""` as the second argument if the user doesn't select a second topping.

For our choice types, we supply a `tag` field as a string, so that within `tagTopping1` the choices are `"pepperoni"`, `"ham"`, and so on. And within `PizzaRecipe` we supply a `tag` with two choices: `predefined` and `createyourown`. Similarly, if the user chooses `predefined` and doesn't require any toppings, both `tagTopping1` and `tagTopping2` can be supplied as empty strings.

Now, when we serialize `Pizza`, which looks like this in our domain:

```
type PizzaSize =
    | Large
    | Medium
    | Small

type Pizza =
    { Name: PizzaName
        Size: PizzaSize
        Recipe: PizzaRecipe }
```

Our resulting DTO will now look like this:

```
type PizzaSizeDto = { tag: string }

type PizzaDto =
    { name: string
        size: PizzaSizeDto
        recipe: PizzaRecipeDto }
```

First, we serialize the `PizzaSize` object, replacing our choices with a `tag` field of type `string`. Then, for the `Pizza` DTO, where the `Name` and `Size` referenced the aliases defined previously, they now reference `string` fields, and for the `recipe` we now reference our newly created `PizzaRecipeDto` instead of the `PizzaRecipe` defined in our domain.

## Exercise G2: Serializing MenuItem and Menu

Now, let's convert the rest of the `Menu` as a DTO object. This is what the remaining objects look like in our domain:

```
type Drink =
    | Coke
    | DietCoke
    | Fanta

type ItemChoice =
    | Pizza of Pizza
    | Drink of Drink

type ItemId = ItemId of int
type ItemPrice = float

type MenuItem =
    { ItemId: ItemId
        ItemChoice: ItemChoice
        ItemPrice: ItemPrice }

```

And this is what it looks like converted to a DTO:

```
module MenuDto =
    open G1_PizzaDto.PizzaDto

    type DrinkDto = { tag: string }

    type ItemChoiceDto = { pizza: PizzaDto; drink: DrinkDto }

    type MenuItemDto =
        { itemId: int
          itemChoice: ItemChoiceDto
          itemPrice: float }

    type MenuDto = MenuItemDto array
```

First, we create a `DrinkDto`, converting our drink choices to a `tag` field. Then, we create a `ItemChoiceDto`, using both the `PizzaDto` we defined in the previous section and our newly created `DrinkDto`.

Then, we define the `MenuItemDto`, where we convert our aliases to simple types, referencing the `ItemChoiceDto`.

Finally, we define our `MenuDto` which is an array of `MenuItemDto` objects.

Since our ultimate objective is to transfer our objects over the wire as JSON objects, serializing our domain types as DTO makes it a lot easier to convert to JSON, as it won't recognise our complex types. By converting them to DTOs in their more primitive form, we are now ready to convert them to JSON.

But first, let's implement the functions to convert our domain types to DTO objects.

## Exercise G3: Serializing from Domain Object to DTO

Let's start by defining the function signatures for the serializers:

```
module SerializePizza =
    open C3_Menu.Menu
    open G1_PizzaDTO.PizzaDto

    type PizzaToppingToDto = Topping -> string
    type PizzaRecipeToDto = PizzaRecipe -> PizzaRecipeDto
    type PizzaSizeToDto = PizzaSize -> string
    type PizzaToDto = Pizza -> PizzaDto
```

Here is the implementation for `PizzaToppingToDto`:

```
let pizzaToppingToDto: PizzaToppingToDto =
    fun (topping: Topping) ->
        match topping with
        | Pepperoni -> "pepperoni"
        | Ham -> "ham"
        | Mushrooms -> "mushrooms"
        | Onions -> "onions"
        | Pineapple -> "pineapple"
```

For each choice option, we want to return a corresponding string. `Pepperoni` becomes `"pepperoni"`, `Ham` becomes `"ham"`, and so on.

Now, let's look at `PizzaRecipeToDto`:

```
let pizzaRecipeToDto: PizzaRecipeToDto =
    fun (pizzaRecipe: PizzaRecipe) ->
        match pizzaRecipe with
        | Predefined ->
            { tag = "predefined"
                toppings = { tagTopping1 = ""; tagTopping2 = "" } }
        | CreateYourOwn(topping1, topping2) ->
            { tag = "createyourown"
                toppings =
                { tagTopping1 = pizzaToppingToDto topping1
                    tagTopping2 =
                    match topping2 with
                    | Some topping -> pizzaToppingToDto topping
                    | None -> "" } }
```

For `PizzaRecipe`, there are two choice options: `Predefined` and `CreateYourOwn`. For `Predefined`, we return the DTO object with the `tag` `"predefined"`, and two `topping` fields with empty strings, as the recipe is predefined.

For `CreateYourOwn`, we take in a tuple of `Topping`s, one of which is optional. So the way we implement it is by assigning `"createyourown"` to the tag, and for the `topping`s, we call our `pizzaToppingToDto` on both `topping`s. On the second `topping`, because it's optional, we have to match every case. If a `topping` exists, it will return the `topping`. Otherwise it will return an empty string.

Here is the implementation for `PizzaSizeToDto`:

```
let pizzaSizeToDto: PizzaSizeToDto =
    fun (pizzaSize: PizzaSize) ->
        match pizzaSize with
        | Large -> "large"
        | Medium -> "medium"
        | Small -> "small"
```

For `PizzaSizeToDto` we simply match the choice type for each `PizzaSize` with its corresponding string.

Finally, serializing our `Pizza` object:

```
let pizzaToDto: PizzaToDto =
    fun (pizza: Pizza) ->
        { name =
            let (PizzaName name) = pizza.Name
            name
            size =
            match pizza.Size with
            | Large -> { tag = pizzaSizeToDto Large }
            | Medium -> { tag = pizzaSizeToDto Medium }
            | Small -> { tag = pizzaSizeToDto Small }
            recipe = pizzaRecipeToDto pizza.Recipe }
```

First, we extract the `PizzaName` from the `Pizza` object. Then, for each `PizzaSize`, we create a DTO object, calling our `pizzaSizeToDto` function so that we can return its corresponding string, and assign it to the `tag` field.

Finally, we extract the `RecipeDto` from the `Recipe` and assign it to `recipe`.

## Exercise G4: Serializing from Domain Object to JSON

Now we can finally go ahead and convert our DTO object to JSON.

First, we define the `serializeJson` function:

```
module ToDto =
    open C3_Menu.Menu
    open G3_SerializePizza.SerializePizza
    open System.Text.Json

    let serializeJson = JsonSerializer.Serialize
```

This function is built into the language, and imported from `System.Text.Json`.

Then, we define a `Pizza` object that we want to first serialize into a DTO:

```
let pizzaName: PizzaName = PizzaName "Pepperoni Feast"
let pizzaSize: PizzaSize = Large
let PizzaRecipe: PizzaRecipe = CreateYourOwn(Pepperoni, Some Ham)

let myPizza: Pizza =
    { Name = pizzaName
        Size = pizzaSize
        Recipe = PizzaRecipe }
```

We create a `Large` `Pizza` with a custom recipe that consists of two toppings, `Pepperoni` and `Ham`.

Finally, we build a small pipeline that feeds `myPizza` into the `pizzaToDto` function we defined in the previous section, and then that DTO is fed into the `serializeJson` function we just defined:

```
myPizza |> pizzaToDto |> serializeJson
```

And the code compiles! We finally have our `Pizza` object that has been serialized, made easy to convert by being transformed into a DTO object, and converted to a JSON object which is now ready to be transmitted over the wire and made available on the website.

[<- Back to Main](../README.md)
