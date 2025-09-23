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

import {Language} from "@/gen/prefs_pb.ts";

/**
 * The language options as human-readable strings.
 */
export type LanguageOption = "English" | "German" | "French"

const englishLanguage: LanguageOption = "English";
const germanLanguage: LanguageOption = "German";
const frenchLanguage: LanguageOption = "French";

/**
 * Converts {@link Language} to LanguageOption.
 */
function fromLanguage(value: Language): LanguageOption {
    if (value === Language.ENGLISH) {
        return englishLanguage;
    } else if (value === Language.GERMAN) {
        return germanLanguage;
    } else if (value === Language.FRENCH) {
        return frenchLanguage;
    } else {
        throw new TypeError("Incorrect language.");
    }
}

/**
 * Converts LanguageOption to {@link Language}.
 */
function toLanguage(value: LanguageOption): Language {
    if (value === englishLanguage) {
        return Language.ENGLISH;
    } else if (value === germanLanguage) {
        return Language.GERMAN;
    } else {
        return Language.FRENCH;
    }
}

export {
    toLanguage,
    fromLanguage,
    englishLanguage,
    germanLanguage,
    frenchLanguage,
};
