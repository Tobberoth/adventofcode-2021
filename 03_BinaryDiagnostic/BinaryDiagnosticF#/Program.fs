open System
let BITLENGTH = 12
let data =
  IO.File.ReadAllLines "input.txt"
  |> Array.map (fun x -> Convert.ToInt32(x, 2))
  |> List.ofArray

let getSums list = 
  [ for i in (List.rev [ 0 .. BITLENGTH - 1 ]) do
    let sum =
      List.map (fun x -> x >>> i) list
      |> List.map (fun x -> x &&& 1)
      |> List.sum
    yield (sum >= list.Length/2)]

let calcPowerConsumption list =
  let sums = getSums list
  let gamma = sums |> List.fold (fun acc elem -> (acc <<< 1) + if elem then 1 else 0) 0
  let epsilon = sums |> List.fold (fun acc elem -> (acc <<< 1) + if elem then 0 else 1) 0
  gamma * epsilon

let filterCommon shift sums elem =
  let isOneCommon = sums |> List.rev |> List.item shift
  if isOneCommon then ((elem >>> shift) &&& 1) = 1 else ((elem >>> shift) &&& 1) = 0

let filterUncommon shift sums elem =
  let isOneCommon = sums |> List.rev |> List.item shift
  if isOneCommon then ((elem >>> shift) &&& 1) = 0 else ((elem >>> shift) &&& 1) = 1

let getSingle filterFunc list =
  let rec loop acc list =
    let sums = getSums list
    match list with
    | [_] -> list
    | _ -> list |> List.filter (fun x -> filterFunc acc sums x) |> loop (acc - 1)
  (loop (BITLENGTH - 1) list).Head

let calcLifeSupport list =
  let oxygen = getSingle filterCommon list
  let scrubber = getSingle filterUncommon list
  oxygen * scrubber

printfn "Power Consumption Rating: %A" (calcPowerConsumption data)
printfn "Life Support Rating: %A" (calcLifeSupport data)