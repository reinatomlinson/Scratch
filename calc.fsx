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

(*
list of currencies
USD
ZAR
AUD
CAD
CHF
CNY
DKK
EUR
GBP
HKD
IDR
ILS
INR
JPY
MXN
NOK
NZD
RUB
SAR
SEK
SGD
TRY
TWD

*)

//let sums = Array.zeroCreate 23
let USDsum = 0.0

//searches each file for "a-dark-room" and selects those lines; do this for each game.
let ADRLines =
  for f in ListOfFileNames.[0..] do
    GetFileContent f |> Seq.filter(fun x -> if x.Contains("a-dark-room") then true else false)
    |> (fun x -> Seq.toArray(x))
    |> (fun line -> for x in line.[0..] do
                      x.Split [|' '; '\n'; '\t' |]
                      |> (fun arr -> let sDate = arr.[0]
                                     let eDate = arr.[1]
                                     let cost = Single.Parse(arr.[7])
                                     let curr = arr.[8]
                                     //printfn "Start: %A End: %A %A %A" sDate eDate cost curr
                                     //return cost?
                                     match curr with
                                       | "USD" -> printfn "%A" cost//(fun x -> sums[0] += x)
                                                               (*| "ZAR" -> (fun x -> sums[0] += x)
                                                               | "AUD" -> (fun x -> sums[0] += x)
                                                               | "CAD" -> (fun x -> sums[0] += x)
                                                               | "CHF" -> (fun x -> sums[0] += x)
                                                               | "CNY" -> (fun x -> sums[0] += x)
                                                               | "DKK" -> (fun x -> sums[0] += x)
                                                               | "EUR" -> (fun x -> sums[0] += x)
                                                               | "GBP" -> (fun x -> sums[0] += x)
                                                               | "HKD" -> (fun x -> sums[0] += x)
                                                               | "IDR" -> (fun x -> sums[0] += x)
                                                               | "ILS" -> (fun x -> sums[0] += x)
                                                               | "INR" -> (fun x -> sums[0] += x)
                                                               | "JPY" -> (fun x -> sums[0] += x)
                                                               | "MXN" -> (fun x -> sums[0] += x)
                                                               | "NOK" -> (fun x -> sums[0] += x)
                                                               | "NZD" -> (fun x -> sums[0] += x)
                                                               | "RUB" -> (fun x -> sums[0] += x)
                                                               | "SAR" -> (fun x -> sums[0] += x)
                                                               | "SEK" -> (fun x -> sums[0] += x)
                                                               | "SGD" -> (fun x -> sums[0] += x)
                                                               | "TRY" -> (fun x -> sums[0] += x)
                                                               | "TWD" -> (fun x -> sums[0] += x) *)
                                       | _ -> printfn "dunno"
                                     )//|> printfn "%A"

                       )
                      //(fun arr -> printfn "game: %A\nno. sold: %A\ncost each: %A\ntotal: %A\ncurrency: %A\n" arr.[4] arr.[5] arr.[6] arr.[7] arr.[8]) )//printfn "%A")
    //|> printfn "%A"
  //|> Seq.choose (fun x -> x.Split ' ') //|> printfn "%s" //(fun x -> x[2])//need to parse $ amts

//[4] is where the name of the game is
//[5;6;7;8] no. sold, cost each, total, currency

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
