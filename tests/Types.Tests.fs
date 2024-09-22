// ttt Version 0.1.0
// https://github.com/taidalog/ttt
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ttt/blob/main/LICENSE

module Types.tests

open System
open Xunit
open Ttt.Types

[<Fact>]
let ``Date' 1`` () =
    let expected = "9/22"
    let actual = Date'.Single(DateTime(2024, 9, 22)).ToString()
    Assert.Equal(expected, actual)

[<Fact>]
let ``Date' 2`` () =
    let expected = "9/22~10/3"
    let actual = Date'.Dudation(DateTime(2024, 9, 22), DateTime(2024, 10, 3)).ToString()
    Assert.Equal(expected, actual)
