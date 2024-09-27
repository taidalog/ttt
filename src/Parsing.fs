// ttt Version 0.1.0
// https://github.com/taidalog/ttt
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ttt/blob/main/LICENSE

namespace Ttt

open System
open Fermata.ParserCombinators.Parsers
open Ttt.Types

module Parsing =
    let digit: Parser<char> =
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

    let yyyy: Parser<int> =
        map' (List.map string >> List.fold (+) "" >> int) (repeat 4 digit)

    let mm: Parser<int> =
        map' (List.map string >> List.fold (+) "" >> int) (repeat 2 digit)

    let dd: Parser<int> =
        map' (List.map string >> List.fold (+) "" >> int) (repeat 2 digit)

    let yyyymmdd: Parser<DateTime> =
        map' (fun ((y: int, m), d) -> DateTime(y, m, d)) (yyyy <+&> char' '-' <&> mm <+&> char' '-' <&> dd)

    let yyyymmdd_yyyymmdd: Parser<DateTime * DateTime> =
        map' id (yyyymmdd <+&> char' '/' <&> yyyymmdd)

    let yyyymmdd_mmdd: Parser<DateTime * DateTime> =
        map'
            (fun ((x: DateTime, m), d) -> x, DateTime(x.Year, m, d))
            (yyyymmdd <+&> char' '/' <&> mm <+&> char' '-' <&> dd)

    let yyyymmdd_dd: Parser<DateTime * DateTime> =
        map' (fun (x: DateTime, d) -> x, DateTime(x.Year, x.Month, d)) (yyyymmdd <+&> char' '/' <&> dd)

    let pos' (parser: Parser<'T>) : Parser<unit> =
        fun (State(x, p)) ->
            match parser (State(x, p)) with
            | Ok _ -> Ok((), State(x, p))
            | Error _ -> Error("Parsing failed.", State(x, p))

    let neg' (parser: Parser<'T>) : Parser<unit> =
        fun (State(x, p)) ->
            match parser (State(x, p)) with
            | Ok _ -> Error("Parsing failed.", State(x, p))
            | Error _ -> Ok((), State(x, p))

    let duration: Parser<Date'> =
        map'
            Date'.Dudation
            ((yyyymmdd_yyyymmdd <+&> (neg' (digit <|> char' '-' <|> char' '/') <|> pos' end'))
             <|> (yyyymmdd_mmdd <+&> (neg' (digit <|> char' '-' <|> char' '/') <|> pos' end'))
             <|> (yyyymmdd_dd <+&> (neg' (digit <|> char' '-' <|> char' '/') <|> pos' end')))

    let single: Parser<Date'> =
        map' Date'.Single yyyymmdd
        <+&> (neg' (digit <|> char' '-' <|> char' '/') <|> pos' end')

    let date': Parser<Date'> = duration <|> single

    let oneOrMore (p: Parser<'T>) : Parser<'T list> =
        map' (fun (x, xs) -> x :: xs) (p <&> many p)

    let spaces: Parser<string> =
        map' (List.map string >> String.concat "") (oneOrMore (char' ' '))

    let remains: Parser<string> = map' (List.map string >> String.concat "") (many any)

    let line: Parser<Date' * string> = date' <+&> spaces <&> remains
