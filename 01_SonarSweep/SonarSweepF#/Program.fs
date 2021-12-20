let diffSum list =
  list |> List.pairwise |> List.map (fun (x,y) -> if y - x > 0 then 1 else 0) |> List.sum

let diffSumWindow list =
  let rec loop list acc =
    match list with
    | [_;_] -> acc
    | head :: tail -> (head + tail.Head + tail.Tail.Head) :: loop tail acc
    | _ -> acc
  loop list []

let lines = System.IO.File.ReadAllLines("input.txt") |> List.ofArray
let nums = lines |> List.map (fun x -> x |> int)

printfn "Depth increased %i times" (diffSum nums)
printfn "Depth window increased %i times" (diffSum (diffSumWindow nums))