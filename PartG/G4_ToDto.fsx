#load "../PartC/C3_Menu.fsx"
#load "G3_SerializePizza.fsx"

module ToDto =
    open C3_Menu.Menu
    open G3_SerializePizza.SerializePizza
    open System.Text.Json

    let serializeJson = JsonSerializer.Serialize

    let pizzaName: PizzaName = PizzaName "Pepperoni Feast"
    let pizzaSize: PizzaSize = Large
    let PizzaRecipe: PizzaRecipe = CreateYourOwn(Pepperoni, Some Ham)

    let myPizza: Pizza =
        { Name = pizzaName
          Size = pizzaSize
          Recipe = PizzaRecipe }

    myPizza |> pizzaToDto |> serializeJson
