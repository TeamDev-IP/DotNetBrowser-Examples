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

import {LucideIcon} from "lucide-react";

/**
 * The theme box's properties.
 */
interface ThemeBoxProps {
    /**
     * The theme's title.
     */
    title: string;
    /**
     * The theme's icon.
     */
    icon: LucideIcon;
    /**
     * Indicates is the theme is selected right now.
     */
    isSelected: boolean;
    /**
     * A callback that is invoked when the user selects the theme.
     */
    onSelect: () => void;
}

/**
 * A component representing one theme option to select.
 *
 * @param props the theme's properties
 * @constructor
 */
export function ThemeBox(props: ThemeBoxProps) {
    return (
        <div onClick={props.onSelect}
             className={`cursor-pointer ${!props.isSelected ? "hover:border-control-hover" : ""} ${props.isSelected ?
                 "border-control-select" : ""} rounded-lg p-2 items-center border-2 flex w-full justify-center gap-x-2 select-none`}>
            <props.icon className={"text-right"}/>
            <span className="justify-end text-center text-sm">
                      {props.title}
            </span>
        </div>

    );
}
