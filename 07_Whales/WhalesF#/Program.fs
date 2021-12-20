let ReadInput filename =
  let lines = System.IO.File.ReadAllLines filename
  lines[0].Split(",")
  |> Array.map (fun (x) -> x |> int)
  |> List.ofArray

let HorizontalPositions data =
  seq { List.min data .. List.max data }

let calculateLightFuel target crabPosition =
  abs (crabPosition - target)

let calculateHeavyFuel target crabPosition =
  let diff = abs (crabPosition - target) |> float
  (diff / 2.0) * (1.0 + diff) |> int

let GetFuelcostForPosition calcFunc target crabPositions =
  (target, (List.fold (fun acc crabPos -> acc + (calcFunc target crabPos)) 0 crabPositions))

let GetAllFuelCosts calc horizontalPositions crabPositions =
  Seq.map (fun x -> calc x crabPositions) horizontalPositions

let crabPositions = ReadInput "testInput.txt"
let horizontalPositions = HorizontalPositions crabPositions
let fuelCosts = GetAllFuelCosts (GetFuelcostForPosition calculateLightFuel) horizontalPositions crabPositions
let minimum = Seq.minBy (fun (x,y) -> y) fuelCosts
printfn "%A" minimum
let fuelCosts2 = GetAllFuelCosts (GetFuelcostForPosition calculateHeavyFuel) horizontalPositions crabPositions 
let minimum2 = Seq.minBy (fun (x,y) -> y) fuelCosts2
printfn "%A" minimum2