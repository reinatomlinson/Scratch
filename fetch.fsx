open System.Net
open System
open System.IO

let fetchURL callback url =
  let req = WebRequest.Create(Uri(url))
  use resp = req.GetResponse()
  use stream = resp.GetResponseStream()
  use reader = new IO.StreamReader(stream)
  callback reader url

let myCallback (reader:IO.StreamReader) url =
  let html = reader.ReadToEnd()
  let html1000 = html.Substring(0, 100)
  printfn "Downloaded %s. First 1000 is %s" url html1000
  html

let google = fetchURL myCallback "http://google.com"
