﻿(* # Frank Self-Hosted Echo Server Sample

## License

Author: Ryan Riley <ryan.riley@panesofglass.org>
Copyright (c) 2010-2011, Ryan Riley.

Licensed under the Apache License, Version 2.0.
See LICENSE.txt for details.
*)

open System.Net
open System.Net.Http
open Frank
open Frank.Hosting

// Respond with the request content, if any.
let echo _ (content: HttpContent) =
  respond HttpStatusCode.OK (``Content-Type`` "text/plain") <| Some(content.ReadAsString())

// Create an application from an `HttpResource` that only responds to `POST` requests.
// Try sending a GET or other method to see a `405 Method Not Allowed` response.
// Also note that this application only responds to the root uri. A `404 Not Found`
// response should be returned for any other uri.
let resource = routeWithMethodMapping "/" [ post echo ]
let app = mountWithDefaults [ resource ]

let config = WebApi.configureWithTestClient app
let baseUri = "http://localhost:1000/"
let host = new Microsoft.ApplicationServer.Http.HttpServiceHost(typeof<WebApi.FrankApi>, config, [| baseUri |])
host.Open()

printfn "Host open.  Hit enter to exit..."
printfn "Use a web browser and go to %A or do it right and get fiddler!" baseUri
System.Console.Read() |> ignore

host.Close()
