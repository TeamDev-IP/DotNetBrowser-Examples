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

import React, {createContext, useContext, useEffect, useState} from "react";
import {prefsClient} from "@/rpc/prefs-client.ts";
import {
    defaultFontSize,
    FontSizeOption,
    fromFontSize,
    smallFontSize
} from "@/converter/font-size.ts";
import {prefsStorage} from "@/storage/prefs-storage.ts";

/**
 * The FontSizeProvider's properties.
 */
type FontSizeProviderProps = {
    children: React.ReactNode
}

/**
 * The state for the FontSizeProvider component.
 */
type FontSizeProviderState = {
    fontSize: FontSizeOption,
    setFontSize: (theme: FontSizeOption) => void,
}

/**
 * The initial state for FontSizeProviderContext.
 */
const initialState: FontSizeProviderState = {
    fontSize: defaultFontSize,
    setFontSize: () => null,
};

const FontSizeProviderContext = createContext<FontSizeProviderState>(initialState);

const smallScaleFactor = 0.8;
const defaultScaleFactor = 1.0;
const largeScaleFactor = 1.2;

/**
 * Provides font size context to its child components and dynamically adjusts
 * font size styles based on the selected option.
 *
 * @param children the React nodes that will have access to the font size context
 * @constructor
 */
export function FontSizeProvider({children}: FontSizeProviderProps) {
    const [fontSize, setFontSize] = useState<FontSizeOption>(prefsStorage.fontSize());

    // Dynamically adjusts CSS font size variables based on the selected fontSize.
    const adjustFontSize = () => {
        const setFontSizeStyles = (scaleFactor: number) => {
            const style = document.documentElement.style;
            style.setProperty('--font-size-sm', `${0.875 * scaleFactor}rem`);
            style.setProperty('--font-size-xs', `${0.75 * scaleFactor}rem`);
            style.setProperty('--font-size-lg', `${1.125 * scaleFactor}rem`);
            style.setProperty('--font-size-2xl', `${1.5 * scaleFactor}rem`);
        };
        if (fontSize === smallFontSize) {
            setFontSizeStyles(smallScaleFactor);
        } else if (fontSize === defaultFontSize) {
            setFontSizeStyles(defaultScaleFactor);
        } else {
            setFontSizeStyles(largeScaleFactor);
        }
    }

    adjustFontSize();
    useEffect(() => {
        (async () => {
            const appearance = await prefsClient.getAppearance({});
            const fontSize = fromFontSize(appearance.fontSize);
            setFontSize(fontSize);
            prefsStorage.saveFontSize(fontSize);
        })();
    }, []);
    useEffect(adjustFontSize, [fontSize]);

    const value = {
        fontSize,
        setFontSize: (fontSize: FontSizeOption) => {
            prefsStorage.saveFontSize(fontSize);
            setFontSize(fontSize);
        },
    };

    return (
        <FontSizeProviderContext.Provider value={value}>
            {children}
        </FontSizeProviderContext.Provider>
    );
}

/**
 * A custom hook that provides access to the current font size and a function to change it.
 */
export const useFontSize = () => {
    const context = useContext(FontSizeProviderContext);

    if (context === undefined)
        throw new Error("useFontSize must be used within a FontSizeProvider");

    return context;
};
