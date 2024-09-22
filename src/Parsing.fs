// ttt Version 0.1.0
// https://github.com/taidalog/ttt
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ttt/blob/main/LICENSE

namespace Ttt

open System
open Fermata.ParserCombinators.Parsers

module Parsing =
    let digit =
        char' '0'
        <|> char' '1'
        <|> char' '2'
        <|> char' '3'
        <|> char' '4'
        <|> char' '5'
        <|> char' '6'
        <|> char' '7'
        <|> char' '8'
        <|> char' '9'

    let yyyy = map' (List.map string >> List.fold (+) "" >> int) (repeat 4 digit)
    let mm = map' (List.map string >> List.fold (+) "" >> int) (repeat 2 digit)
    let dd = map' (List.map string >> List.fold (+) "" >> int) (repeat 2 digit)

    let yyyymmdd =
        map' (fun ((y: int, m), d) -> DateTime(y, m, d)) (yyyy <+&> char' '-' <&> mm <+&> char' '-' <&> dd)

    let yyyymmdd_yyyymmdd = map' id (yyyymmdd <+&> char' '/' <&> yyyymmdd)

    let yyyymmdd_mmdd =
        map'
            (fun ((x: DateTime, m), d) -> x, DateTime(x.Year, m, d))
            (yyyymmdd <+&> char' '/' <&> mm <+&> char' '-' <&> dd)

    let yyyymmdd_dd =
        map' (fun (x: DateTime, d) -> x, DateTime(x.Year, x.Month, d)) (yyyymmdd <+&> char' '/' <&> dd)
