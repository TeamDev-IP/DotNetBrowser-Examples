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

import {FontSize} from "@/gen/prefs_pb.ts";

/**
 * The font size options as human-readable strings.
 */
export type FontSizeOption = "Small" | "Default" | "Large";

const smallFontSize: FontSizeOption = "Small";
const defaultFontSize: FontSizeOption = "Default";
const largeFontSize: FontSizeOption = "Large";

/**
 * Converts {@link FontSize} to FontSizeOption.
 */
function fromFontSize(value: FontSize): FontSizeOption {
    if (value === FontSize.SMALL) {
        return smallFontSize;
    } else if (value === FontSize.DEFAULT) {
        return defaultFontSize;
    } else if (value === FontSize.LARGE) {
        return largeFontSize;
    } else {
        throw new TypeError("Incorrect font size.");
    }
}

/**
 * Converts FontSizeOption to {@link FontSize}.
 */
function toFontSize(value: FontSizeOption): FontSize {
    if (value === smallFontSize) {
        return FontSize.SMALL;
    } else if (value === defaultFontSize) {
        return FontSize.DEFAULT;
    } else {
        return FontSize.LARGE;
    }
}

export {
    smallFontSize,
    defaultFontSize,
    largeFontSize,
    fromFontSize,
    toFontSize
};
