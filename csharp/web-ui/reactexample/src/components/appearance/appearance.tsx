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

import {Separator} from "@/components/ui/separator.tsx";
import {useEffect} from "react";
import {Laptop, Moon, Sun} from "lucide-react";
import {ThemeBox} from "@/components/appearance/theme-box.tsx";
import {Combobox} from "@/components/ui/common/combobox.tsx";
import {AppearanceSchema} from "@/gen/prefs_pb.ts";
import {prefsClient} from "@/rpc/prefs-client.ts";
import {create} from "@bufbuild/protobuf";
import {useTheme} from "@/components/appearance/theme-provider.tsx";
import {useFontSize} from "@/components/appearance/font-size-provider.tsx";
import {
    defaultFontSize,
    FontSizeOption,
    fromFontSize,
    largeFontSize,
    smallFontSize,
    toFontSize
} from "@/converter/font-size.ts";
import {
    darkTheme,
    fromTheme,
    lightTheme,
    systemTheme,
    ThemeOption,
    toTheme
} from "@/converter/theme.ts";

/**
 * Available font size options.
 */
const fontsSizes: FontSizeOption[] = [
    smallFontSize,
    defaultFontSize,
    largeFontSize,
];

/**
 * A component that allows managing the appearance preferences.
 *
 * @constructor
 */
export function Appearance() {
    const {theme, setTheme} = useTheme();
    const {fontSize, setFontSize} = useFontSize();

    useEffect(() => {
        (async () => {
            const appearance = await prefsClient.getAppearance({});
            const theme = fromTheme(appearance.theme);
            setTheme(theme);

            const fontSize = fromFontSize(appearance.fontSize);
            setFontSize(fontSize);
        })();
    }, []);

    const onUpdateAppearance = ({
                                    newTheme = theme,
                                    newFontSize = fontSize,
                                }: {
        newTheme?: ThemeOption;
        newFontSize?: FontSizeOption,
    }) => {
        const newAppearance = create(AppearanceSchema, {
            theme: toTheme(newTheme),
            fontSize: toFontSize(newFontSize)
        });
        prefsClient.setAppearance(newAppearance);
    };

    function updateTheme(theme: ThemeOption) {
        onUpdateAppearance({newTheme: theme});
        setTheme(theme);
    }

    return (
        <div className="space-y-4">
            <h1 className="text-2xl font-semibold">Appearance</h1>
            <Separator className="my-4 h-[1px] w-full"/>
            <div className="items-center flex-wrap space-y-4 py-1">
                <div>
                    <p className="text-sm">Theme</p>
                    <p className="text-xs text-muted-foreground text-gray-500">
                        The color theme for the application: light, dark, or match your
                        system&nbsp;settings.
                    </p>
                </div>
                <div
                    className="flex flex-col sm:flex-row gap-x-2 gap-y-2 justify-center items-center">
                    <ThemeBox title="Light" isSelected={theme === lightTheme}
                              onSelect={() => updateTheme(lightTheme)} icon={Sun}/>
                    <ThemeBox title="Dark" isSelected={theme === darkTheme}
                              onSelect={() => updateTheme(darkTheme)} icon={Moon}/>
                    <ThemeBox title="System" isSelected={theme === systemTheme}
                              onSelect={() => updateTheme(systemTheme)} icon={Laptop}/>
                </div>
            </div>
            <div className="w-full flex items-center space-y-2 justify-between py-2">
                <div className="pr-8">
                    <p className="text-sm">Font size</p>
                    <p className="text-xs text-muted-foreground text-gray-500">
                        Adjust the size of the text in the application for better readability.
                    </p>
                </div>
                <Combobox options={fontsSizes} onSelect={(value) => {
                    const newFontSize = value as FontSizeOption;
                    onUpdateAppearance({newFontSize});
                    setFontSize(newFontSize);
                }} currentOption={fontSize}/>
            </div>
        </div>
    );
}
