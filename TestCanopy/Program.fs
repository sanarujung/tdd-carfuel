open System
open canopy
open runner
open configuration
open reporters
 
let baseUrl = "http://localhost:46295" 
chromeDir <- "C:\\chromedriver" 
start firefox 
 
"Log in" &&& fun _ ->
    url (baseUrl + "/Account/Login")
    "#Email" << "pang@gmail.com"
    "#Password" << "Pang@123"
    click "input[type=submit]"
    on baseUrl
 
"Click add link then go to create page" &&& fun _ ->
    url (baseUrl + "/car")
    displayed "a#gotoAdd"
    click "a#gotoAdd"
    on (baseUrl + "/car/create")
 
"Add new car" &&& fun _ ->
    let make = "Tesla " + DateTime.Now.Ticks.ToString()
    url (baseUrl + "/car/create")
    "#Make" << make
    "#Model" << "Model 3"
    click "button#btnAdd"
 
    on (baseUrl + "/car")
    "td" *= make


"Add the second car" &&& fun _ ->
    let make = "Tesla " + DateTime.Now.Ticks.ToString()
    url (baseUrl + "/car/create")
    "#Make" << make
    "#Model" << "Model 3"
    click "button#btnAdd"
 
    on (baseUrl + "/car")
    "td" *= make


"Add the third car should failed" &&& fun _ ->
    let make = "Tesla " + DateTime.Now.Ticks.ToString()
    url (baseUrl + "/cars/create")
    "#Make" << make
    "#Model" << "Model 3"
    click "button#btnAdd"
 
    on (baseUrl + "/car")
    "td" *!= make
    contains "Cannot add more car" (read ".error")
 
run() 
//printfn "press [enter] to exit"
//System.Console.ReadLine() |> ignore 
quit()