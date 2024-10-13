# ttt

Version 0.1.0

ttt: text to timeline.

## Synopsis

- A text-to-timeline convertor. You can just input your schedules to get a timeline image.

## Usage

1. Visit [ttt](https://taidalog.github.io/ttt/).
1. Input your schedules to the input area. (See the example below.)
1. Your timeline image will be generated.
1. Cliick the "Download" button to save the generated image.

## Schedule format

```
YYYY-MM-DD/YYYY-MM-DD
YYYY-MM-DD XXXX
YYYY-MM-DD XXXX
...
```

where,

- `YYYY-MM-DD` represents a date.
- `XXXX` represents a note for the date.
- The first line has to include two dates, the first day and the last day of the duration, separated with a "/".
- From the second line, each line has to include a date and a note for a schedule, separated with a space.

## Example

```
2024-04-01/2025-03-31
2024-04-20 Gendo's birthday
2024-05-05 Shoko's birthday
2024-06-06 Shinji's birthday
2024-07-01 Maya's birthday
2024-09-13 Kaworu's birthday
2024-10-03 Madoka's birthday
2024-12-04 Asuka's birthday
2025-03-30 Rei's birthday
2025-03-31 Mari's birthday
```

The text above will be converted into the timeline image below:

![The output](https://raw.githubusercontent.com/taidalog/ttt/main/docs/image/timeline.en.png)

## Recommended environment

- Mozilla Firefox 130.0.1 (64 bit) or later.
- Google Chrome 129.0.6668.71 (64 bit) or later.
- Microsoft Edge 129.0.2792.65 (64 bit) or later.

## Terms of Service

- The copyright is owned by the creator (I).
- The communications charge required for use of this site will be borne by the user.
- The creator is not responsible for any damage caused by computer viruses, data loss, or any other disadvantages caused by usinfg this site.
- You can use the source code, but please keep the copyright notice and license notice when redistributing.

## Known Issue

- Lines overlap on the output if the time interval between those schedules are too short.

## Release Notes

[Releases on GitHub](https://github.com/taidalog/ttt/releases)

## License

This application is licensed under MIT License.
