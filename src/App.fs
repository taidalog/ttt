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

module App =
    let keyboardshortcut (e: KeyboardEvent) =
        match document.activeElement.id with
        | "textInput" ->
            match e.key with
            | "Escape" -> (document.getElementById "textInput").blur ()
            | "Enter" ->
                if e.ctrlKey then
                    // creates a timelime
                    ()
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

            // keyboard shortcut
            document.onkeydown <- fun (e: KeyboardEvent) -> keyboardshortcut e)
    )
