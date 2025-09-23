/*
 *  Copyright (c) 2025 TeamDev
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

import {Theme} from "@/gen/prefs_pb.ts";

/**
 * The theme options as human-readable strings.
 */
export type ThemeOption = "dark" | "light" | "system";

const lightTheme: ThemeOption = "light";
const darkTheme: ThemeOption = "dark";
const systemTheme: ThemeOption = "system";

/**
 * Converts {@link Theme} to ThemeOption.
 */
function fromTheme(value: Theme): ThemeOption {
    if (value === Theme.LIGHT) {
        return lightTheme;
    } else if (value === Theme.DARK) {
        return darkTheme;
    } else if (value === Theme.SYSTEM) {
        return systemTheme;
    } else {
        throw new TypeError("Incorrect two-factor authentication.");
    }
}

/**
 * Converts ThemeOption to {@link Theme}.
 */
function toTheme(value: ThemeOption): Theme {
    if (value === lightTheme) {
        return Theme.LIGHT;
    } else if (value === darkTheme) {
        return Theme.DARK;
    } else {
        return Theme.SYSTEM;
    }
}

export {
    fromTheme,
    toTheme,
    lightTheme,
    darkTheme,
    systemTheme
};
