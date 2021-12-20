let getDist (direction, momentum) (horizontal, depth, aim) =
  match direction with
  | "up" -> (horizontal, depth, aim - momentum)
  | "down" -> (horizontal, depth, aim + momentum)
  | "forward" | _ -> (horizontal + momentum, depth + momentum * aim, aim)

let calcDistance list =
  let rec loop list acc =
    match list with
    | head :: tail -> loop tail (getDist head acc)
    | [] -> acc
  let (horizontal, depth, _) = loop list (0, 0, 0)
  horizontal * depth

let inputToTokens file =
  let input = System.IO.File.ReadAllLines file
  let tokens = input  |> Array.map (fun x -> x.Split ' ' |> Array.pairwise)
  let tokenList = tokens |> Array.concat |> List.ofArray
  tokenList |> List.map (fun (x,y) -> (x, y |> int))

printfn "%A" (calcDistance (inputToTokens "input.txt"))