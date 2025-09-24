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
import {fromTheme, systemTheme, ThemeOption} from "@/converter/theme.ts";
import {prefsStorage} from "@/storage/prefs-storage.ts";

/**
 * The ThemeProvider's properties.
 */
type ThemeProviderProps = {
    children: React.ReactNode
}

/**
 * The state for the ThemeProvider component.
 */
type ThemeProviderState = {
    theme: ThemeOption,
    setTheme: (theme: ThemeOption) => void
}

/**
 * The initial state for ThemeProviderContext.
 */
const initialState: ThemeProviderState = {
    theme: systemTheme,
    setTheme: () => null,
};

const ThemeProviderContext = createContext<ThemeProviderState>(initialState);

/**
 * Provides a context to dynamically manage and apply a theme for the application.
 *
 * @param children the React nodes that will have access to the theme context
 * @constructor
 */
export function ThemeProvider({
                                  children,
                              }: ThemeProviderProps) {
    const [theme, setTheme] = useState<ThemeOption>(prefsStorage.theme());

    /**
     * Applies the current theme to the root element by adding the appropriate class.
     */
    const setRootTheme = () => {
        const root = window.document.documentElement;
        root.classList.remove("light", "dark");
        if (theme === "system") {
            const systemTheme = window.matchMedia("(prefers-color-scheme: dark)")
                .matches
                ? "dark"
                : "light";

            root.classList.add(systemTheme);
            return;
        }
        root.classList.add(theme);
    };
    setRootTheme();
    useEffect(() => {
        (async () => {
            const appearance = await prefsClient.getAppearance({});
            const theme = fromTheme(appearance.theme);
            setTheme(theme);
            prefsStorage.saveTheme(theme);
        })();
    }, []);
    useEffect(setRootTheme, [theme]);

    const value = {
        theme,
        setTheme: (theme: ThemeOption) => {
            prefsStorage.saveTheme(theme);
            setTheme(theme);
        },
    };

    return (
        <ThemeProviderContext.Provider value={value}>
            {children}
        </ThemeProviderContext.Provider>
    );
}

/**
 * A custom hook that provides access to the current theme and a function to update it.
 */
export const useTheme = () => {
    const context = useContext(ThemeProviderContext);

    if (context === undefined)
        throw new Error("useTheme must be used within a ThemeProvider");

    return context;
};
