open System
let BITLENGTH = 12
let data =
  IO.File.ReadAllLines "input.txt"
  |> Array.map (fun x -> Convert.ToInt32(x, 2))
  |> List.ofArray

let sums = 
  [ for i in (List.rev [ 0 .. BITLENGTH - 1 ]) do
    let sum =
      List.map (fun x -> x >>> i) data
      |> List.map (fun x -> x &&& 1)
      |> List.sum
    yield (sum >= data.Length/2)]

let gamma = sums |> List.fold (fun acc elem -> (acc <<< 1) + if elem then 1 else 0) 0
let epsilon = sums |> List.fold (fun acc elem -> (acc <<< 1) + if elem then 0 else 1) 0

printfn "Power Consumption Rating: %A" (gamma * epsilon)