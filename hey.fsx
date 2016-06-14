let rec quicksort list =
  match list with
    | [] -> []
    | firstElem::otherElements ->
      let smallerElements =
        otherElements
        |> List.filter (fun e -> e <firstElem)
        |> quicksort
      let largerElements =
        otherElements
        |> List.filter(fun e -> e >= firstElem)
        |> quicksort
      List.concat [smallerElements; [firstElem]; largerElements]

printfn "%A" (quicksort [1;7;2;12;4;0])
