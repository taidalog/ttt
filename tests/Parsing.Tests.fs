// ttt Version 0.1.0
// https://github.com/taidalog/ttt
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ttt/blob/main/LICENSE

module Parsing.tests

open System
open Xunit
open Fermata.ParserCombinators.Parsers
open Ttt.Parsing
open Ttt.Types

[<Fact>]
let ``digit 1`` () =
    let expected = Ok('2', State("2024-09-22", 1))
    let actual = digit (State("2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``digit 2`` () =
    let expected = Error("Parsing failed.", State("Date: 2024-09-22", 0))
    let actual = digit (State("Date: 2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyy 1`` () =
    let expected = Ok(2024, State("2024-09-22", 4))
    let actual = yyyy (State("2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyy 2`` () =
    let expected = Error("Parsing failed.", State("24-09-22", 0))
    let actual = yyyy (State("24-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyy 3`` () =
    let expected = Error("Parsing failed.", State("09-22", 0))
    let actual = yyyy (State("09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyy 4`` () =
    let expected = Error("Parsing failed.", State("Date: 2024-09-22", 0))
    let actual = yyyy (State("Date: 2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``mm 1`` () =
    let expected = Ok(9, State("2024-09-22", 7))
    let actual = mm (State("2024-09-22", 5))
    Assert.Equal(expected, actual)

[<Fact>]
let ``mm 2`` () =
    let expected = Error("Parsing failed.", State("2024-9-22", 5))
    let actual = mm (State("2024-9-22", 5))
    Assert.Equal(expected, actual)

[<Fact>]
let ``mm 3`` () =
    let expected = Error("Parsing failed.", State("Date: 2024-09-22", 0))
    let actual = mm (State("Date: 2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``dd 1`` () =
    let expected = Ok(22, State("2024-09-22", 10))
    let actual = dd (State("2024-09-22", 8))
    Assert.Equal(expected, actual)

[<Fact>]
let ``dd 2`` () =
    let expected = Error("Position exceeded input length.", State("9", 0))
    let actual = dd (State("9", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``dd 3`` () =
    let expected = Error("Parsing failed.", State("Date: 2024-09-22", 0))
    let actual = dd (State("Date: 2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd 1`` () =
    let expected = Ok(DateTime(2024, 9, 22), State("2024-09-22", 10))
    let actual = yyyymmdd (State("2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd 2`` () =
    let expected = Error("Parsing failed.", State("24-09-22", 0))
    let actual = yyyymmdd (State("24-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd 3`` () =
    let expected = Error("Parsing failed.", State("09-22", 0))
    let actual = yyyymmdd (State("09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd 4`` () =
    let expected = Error("Parsing failed.", State("Date: 2024-09-22", 0))
    let actual = yyyymmdd (State("Date: 2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd_yyyymmdd 1`` () =
    let expected =
        Ok((DateTime(2024, 9, 22), DateTime(2025, 10, 3)), State("2024-09-22/2025-10-03", 21))

    let actual = yyyymmdd_yyyymmdd (State("2024-09-22/2025-10-03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd_mmdd 1`` () =
    let expected =
        Ok((DateTime(2024, 9, 22), DateTime(2024, 10, 3)), State("2024-09-22/10-03", 16))

    let actual = yyyymmdd_mmdd (State("2024-09-22/10-03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``yyyymmdd_dd 1`` () =
    let expected =
        Ok((DateTime(2024, 9, 22), DateTime(2024, 9, 3)), State("2024-09-22/03", 13))

    let actual = yyyymmdd_dd (State("2024-09-22/03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``duration 1`` () =
    let expected =
        Ok(Date'.Dudation(DateTime(2024, 9, 22), DateTime(2025, 10, 3)), State("2024-09-22/2025-10-03", 21))

    let actual = duration (State("2024-09-22/2025-10-03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``duration 2`` () =
    let expected =
        Ok(Date'.Dudation(DateTime(2024, 9, 22), DateTime(2024, 10, 3)), State("2024-09-22/10-03", 16))

    let actual = duration (State("2024-09-22/10-03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``duration 3`` () =
    let expected =
        Ok(Date'.Dudation(DateTime(2024, 9, 22), DateTime(2024, 9, 3)), State("2024-09-22/03", 13))

    let actual = duration (State("2024-09-22/03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``single 1`` () =
    let expected = Ok(Date'.Single(DateTime(2024, 9, 22)), State("2024-09-22", 10))
    let actual = single (State("2024-09-22", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``date' 1`` () =
    let expected =
        Ok(Date'.Dudation(DateTime(2024, 9, 22), DateTime(2025, 10, 3)), State("2024-09-22/2025-10-03", 21))

    let actual = date' (State("2024-09-22/2025-10-03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``date' 2`` () =
    let expected =
        Ok(Date'.Dudation(DateTime(2024, 9, 22), DateTime(2024, 10, 3)), State("2024-09-22/10-03", 16))

    let actual = date' (State("2024-09-22/10-03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``date' 3`` () =
    let expected =
        Ok(Date'.Dudation(DateTime(2024, 9, 22), DateTime(2024, 9, 3)), State("2024-09-22/03", 13))

    let actual = date' (State("2024-09-22/03", 0))
    Assert.Equal(expected, actual)

[<Fact>]
let ``date' 4`` () =
    let expected = Ok(Date'.Single(DateTime(2024, 9, 22)), State("2024-09-22", 10))
    let actual = date' (State("2024-09-22", 0))
    Assert.Equal(expected, actual)
