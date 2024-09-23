// ttt Version 0.1.0
// https://github.com/taidalog/ttt
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ttt/blob/main/LICENSE

namespace Ttt

open Browser.Dom
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Fermata
open Fermata.ParserCombinators.Parsers
open Ttt.Parsing
open Ttt.Types

module App =
    let main () =
        let textInput = document.getElementById "textInput" :?> HTMLInputElement
        let inputs = textInput.value |> String.split '\n'

        let dates =
            (yyyymmdd_yyyymmdd <|> yyyymmdd_mmdd <|> yyyymmdd_dd) (State(List.head inputs, 0))
            |> function
                | Ok((x, y), _) -> Some(x, y)
                | Error _ -> None

        match dates with
        | None -> ()
        | Some(date1, date2) ->
            let days = (date2 - date1).TotalDays + 1.

            let svg = document.createElementNS ("http://www.w3.org/2000/svg", "svg")
            svg.setAttribute ("xmlns", "http://www.w3.org/2000/svg")
            svg.setAttribute ("viewBox", $"0, 0, 1080, 1920")
            svg.setAttribute ("width", $"1080px")
            svg.setAttribute ("height", $"1920px")

            let title = document.createElementNS ("http://www.w3.org/2000/svg", "title")
            title.textContent <- "timeline"

            let lineElement = document.createElementNS ("http://www.w3.org/2000/svg", "line")
            lineElement.setAttribute ("x1", $"""40""")
            lineElement.setAttribute ("y1", $"""40""")
            lineElement.setAttribute ("x2", $"""40""")
            lineElement.setAttribute ("y2", $"""1880""")
            lineElement.setAttribute ("stroke", $"""#333333""")
            lineElement.setAttribute ("stroke-width", $"""4""")
            svg.appendChild lineElement |> ignore

            let g = document.createElementNS ("http://www.w3.org/2000/svg", "g")

            let textDate1 = document.createElementNS ("http://www.w3.org/2000/svg", "text")
            textDate1.setAttribute ("x", $"""20""")
            textDate1.setAttribute ("y", $"""20""")
            textDate1.textContent <- $"""%s{date1.ToString("yyyy-MM-dd")}"""
            g.appendChild textDate1 |> ignore

            inputs
            |> List.tail
            |> List.map (fun x -> State(x, 0))
            |> List.map line
            |> List.map (fun x ->
                match x with
                | Ok((d, s), _) ->
                    let text = document.createElementNS ("http://www.w3.org/2000/svg", "text")
                    text.setAttribute ("x", $"""60""")
                    text.setAttribute ("y", $"""%f{((d.fst - date1).TotalDays + 1.) / days * 1840. + 40.}""")
                    text.textContent <- $"""%s{string d} %s{s}"""
                    text
                | Error _ ->
                    let text = document.createElementNS ("http://www.w3.org/2000/svg", "text")
                    text)
            |> List.filter (fun x -> x.textContent <> "")
            |> List.iter (fun x -> g.appendChild x |> ignore)

            let textDate2 = document.createElementNS ("http://www.w3.org/2000/svg", "text")
            textDate2.setAttribute ("x", $"""20""")
            textDate2.setAttribute ("y", $"""1900""")
            textDate2.textContent <- $"""%s{date2.ToString("yyyy-MM-dd")}"""
            g.appendChild textDate2 |> ignore

            svg.appendChild g |> ignore

            let outputArea = document.getElementById "outputArea"
            outputArea.innerHTML <- ""
            outputArea.appendChild svg |> ignore

    let saveAsSvg _ =
        let svgUrl =
            (document.getElementById "outputArea").innerHTML
            |> fun x -> Browser.Blob.Blob.Create([| x |])
            |> fun x -> Browser.Url.URL.createObjectURL x

        let a = document.createElement ("a") :?> HTMLAnchorElement
        a.href <- svgUrl
        a.setAttribute ("download", "timeline.svg")
        document.body.appendChild a |> ignore
        a.click ()
        document.body.removeChild a |> ignore
        Browser.Url.URL.revokeObjectURL (svgUrl)

    let keyboardshortcut (e: KeyboardEvent) =
        match document.activeElement.id with
        | "textInput" ->
            match e.key with
            | "Escape" -> (document.getElementById "textInput").blur ()
            | "Enter" ->
                if e.ctrlKey then
                    main ()
            | _ -> ()
        | _ ->
            let helpWindow = document.getElementById "helpWindow"

            let isHelpWindowActive =
                helpWindow.classList
                |> (fun x -> JS.Constructors.Array?from(x))
                |> Array.contains "active"

            let informationPolicyWindow = document.getElementById "informationPolicyWindow"

            let isInformationPolicyWindowActive =
                informationPolicyWindow.classList
                |> (fun x -> JS.Constructors.Array?from(x))
                |> Array.contains "active"

            match e.key with
            | "Escape" ->
                if isHelpWindowActive then
                    helpWindow.classList.remove "active"

                if isInformationPolicyWindowActive then
                    informationPolicyWindow.classList.remove "active"
            | "?" ->
                if not isHelpWindowActive then
                    helpWindow.classList.add "active"
            | "\\" ->
                (document.getElementById "textInput").focus ()
                e.preventDefault ()
            | _ -> ()

    window.addEventListener (
        "DOMContentLoaded",
        (fun _ ->
            // help window
            [ "helpButton"; "helpClose" ]
            |> List.iter (fun x ->
                (document.getElementById x :?> HTMLButtonElement).onclick <-
                    fun _ -> (document.getElementById "helpWindow").classList.toggle "active")

            // information policy window
            (document.getElementById "informationPolicyLink").onclick <-
                fun event ->
                    event.preventDefault ()
                    (document.getElementById "informationPolicyWindow").classList.add "active"

            (document.getElementById "informationPolicyClose").onclick <-
                fun _ -> (document.getElementById "informationPolicyWindow").classList.remove "active"

            // real-time validation and preview
            let textInput = document.getElementById "textInput" :?> HTMLInputElement

            textInput.oninput <-
                fun _ ->
                    let validationArea = document.getElementById "validationArea"

                    if String.length textInput.value = 0 then
                        validationArea.innerText <- ""
                    else
                        let inputs = textInput.value |> String.split '\n'
                        let duration' = List.head inputs |> fun x -> State(x, 0) |> duration

                        match duration' with
                        | Error _ ->
                            printfn "1行目が正しくありません。"
                            validationArea.innerText <- "1行目が正しくありません。"
                        | Ok(d, _) ->
                            match d with
                            | Date'.Single _ ->
                                printfn "1行目が正しくありません。"
                                validationArea.innerText <- "1行目が正しくありません。"
                            | Date'.Dudation _ ->
                                let lines = inputs |> List.tail |> List.map (fun x -> State(x, 0)) |> List.map line

                                let isOk' x =
                                    match x with
                                    | Ok _ -> true
                                    | Error _ -> false

                                if List.forall isOk' lines then
                                    validationArea.innerText <- ""
                                    main ()
                                else
                                    lines
                                    |> List.indexed
                                    |> List.filter (fun (_, x) -> x |> isOk' |> not)
                                    |> List.map (fun (i, _) -> $"%d{i + 1}行目が正しくありません。")
                                    |> List.iter (fun x ->
                                        printfn "%s" x
                                        validationArea.innerText <- x)


            // keyboard shortcut
            document.onkeydown <- fun (e: KeyboardEvent) -> keyboardshortcut e

            // downloading
            (document.getElementById "downloadButton").onclick <- fun e -> saveAsSvg e)
    )
