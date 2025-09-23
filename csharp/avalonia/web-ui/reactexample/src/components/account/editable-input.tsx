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

import {Input} from "@/components/ui/input.tsx";
import {ChangeEventHandler, useState} from "react";

/**
 * The input's properties.
 */
interface EditableInputProps {
    title: string,
    value: string,
    id: string,
    isEmail: boolean,
    onChange: (value: string) => void
}

/**
 * An input field with the user's data.
 *
 * @param title the input's title
 * @param value the input's value
 * @param isEmail indicates if the input contains an email
 * @param onChange a callback that is invoked when the input's value has been changed,
 * specifically on blur
 * @constructor
 */
export function EditableInput({title, value, isEmail, onChange}: EditableInputProps) {
    const [isValid, setIsValid] = useState(true);

    const handleInputChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const value = e.target.value;
        if (isEmail) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            const isValid = emailRegex.test(value);
            setIsValid(isValid);
        }
    };
    return (
        <div className="w-full items-center space-y-2">
            <div className="w-full sm:flex items-center space-y-1 justify-between">
                <p className="text-sm">{title}</p>
                <div className={"xs:w-full sm:w-[50%] lg:w-[30%]"}>
                    <Input type={isEmail ? "email" : "text"}
                           className={`w-full text-sm ${!isValid && "bg-red-300"} focus-visible:border-control-select`}
                           onBlur={(event) => {
                               if (isValid) {
                                   onChange(event.target.value);
                               } else {
                                   event.target.value = value;
                                   setIsValid(true);
                               }
                           }}
                           onChange={handleInputChange}
                           defaultValue={value}/>
                    {!isValid && (
                        <span className="text-xs text-red-500">
                            Please enter a valid email address.
                        </span>
                    )}
                </div>
            </div>
        </div>
    );
}
