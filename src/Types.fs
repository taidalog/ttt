// ttt Version 0.1.0
// https://github.com/taidalog/ttt
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ttt/blob/main/LICENSE

namespace Ttt

open System

module Types =
    [<StructuredFormatDisplay("{DisplayText}")>]
    type Date' =
        | Single of DateTime
        | Dudation of DateTime * DateTime

        override this.ToString() =
            match this with
            | Single x -> x.ToString("M/d")
            | Dudation(x, y) -> $"""%s{x.ToString("M/d")}~%s{y.ToString("M/d")}"""

        member this.DisplayText = this.ToString()
