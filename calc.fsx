open System.IO
open System
open System.Net
//open Microsoft.FSharp.Collections

//let readFile = File.ReadAllLines("/Users/reinatomlinson1/Desktop/Scratch/ios-sales/85830718_0114_AU.txt")
let ListOfFileNames =
  Directory.GetFiles("/Users/reinatomlinson1/Desktop/Scratch/ios-sales/")
//let ex = File.Exists("/Users/reinatomlinson1/Desktop/Scratch/ios-sales/85830718_0114_AU.txt")
//let first10 = readFile.Substring(0, 10)
//readFile is an array
//let line = readFile.[0]
//let line2 = readFile.[1]
//let line3 = readFile.[2]
//let first10 = samp.Substring(0,5)
//printfn "LINE 1:\n\n%s" line
//printfn "\n\n\nLINE 2:\n\n%s" line2
//printfn "\n\n\nLINE 3:\n\n%s" line3
//printfn "%b" ex

//readFile |> List.map

let GetFileContent f = File.ReadAllLines(f)

//list of game names
//a-dark-room
//theensign
//BeautifulGo
//flatland (Noble Circle)
//adr-bundle

//searches each file for "a-dark-room" and selects those lines; do this for each game.
let ADRLines =
  for f in ListOfFileNames.[0..] do
    GetFileContent f |> Seq.filter(fun x -> if x.Contains("a-dark-room") then true else false)
    |> (fun x -> Seq.toArray(x))
    |> (fun line -> for x in line.[0..] do
                      x.Split [|' '; '\n'; '\t' |]
                      |> printfn "%A")
    //|> printfn "%A"
  //|> Seq.choose (fun x -> x.Split ' ') //|> printfn "%s" //(fun x -> x[2])//need to parse $ amts

let str = "hey there"
let split = str.Split [|' '|]
printfn "%A" split
//printfn ADRLines
//ADRLines is type unit == void basically

//let ADRLinesSplit =
//  Seq.toArray(ADRLines)// |> printfn "%A"

//let ParseForADR f =
//  GetFileContent f |> Seq.filter(fun x -> if x.Contains("a-dark-room") then true else false)
//  |> printfn "%s" (ParseForADR f |> Seq.length).toString

//printfn "%d" (ParseForADR |> Seq.length)


//let PrintArray a = for i in a.[0..] do printfn "\n%A" a


//let ReadFiles =
//for f in FileList do
//  let TotalLines = File.ReadAllLines(f)
//|> Seq.filter(fun x -> if x.Contains("Total_Amount") then true else false)
    //|> Seq.filter(fun x -> if x.Contains("Total_Amount") then true else false)

//let ParseForTotal =
//  parseLines
//  |> Seq.filter(fun x -> if x.Contains("Total_Amount") then true else false)

//printfn "ADR lines: " + (ParseFileADR |> Seq.length).ToString()
//printfn "%A" ParseLines
(*
let ParseFileForWarnings =
  ParseFile
  |> Seq.filter(fun x -> if x.Contains("_warning_") then true else false)

let myFileSet =
  let mySubFileSet = Seq.append ParseFileForErrors ParseFileForWarnings
  let FileSummary = "Number of Entries : " + ParseFile.Length.ToString() + " Number of Errors : " + (ParseFileForErrors |> Seq.length).ToString()
  Seq.append ( FileSummary |> Seq.singleton) mySubFileSet
*)
